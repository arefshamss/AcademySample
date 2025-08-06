using System.Reflection;
using Academy.Application.Extensions;
using Academy.Domain.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Academy.Web.ActionFilters;

public sealed class SanitizeActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if(context.HttpContext.Request.Path.ToString().ToLower().Contains("/admin"))
            return;
        var actionParameters = context.ActionDescriptor.Parameters;

        foreach (var key in context.ActionArguments.Keys)
        {
      
            var parameterDescriptor = actionParameters
                .FirstOrDefault(p => p.Name == key) as ControllerParameterDescriptor;
            
            if (parameterDescriptor?.ParameterInfo
                    ?.GetCustomAttribute<IgnoreSanitizeAttribute>() != null) continue;
            

            var argument = context.ActionArguments[key];
            
            if (argument != null)
            {
                context.ActionArguments[key] = SanitizeObject(argument);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    private object SanitizeObject(object obj)
    {
        if (obj == null) return obj;

        var objType = obj.GetType();

        if (objType.GetCustomAttribute<IgnoreSanitizeAttribute>() is not null )
            return obj;
        
        if (obj is string str)
        {
            return str.SanitizeText();
        }

        foreach (var property in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetCustomAttribute<IgnoreSanitizeAttribute>() is not null || !property.CanWrite || property.GetIndexParameters().Length > 0)
                continue; 
            
            var propertyValue = property.GetValue(obj);

            if (propertyValue is string stringValue)
            {
                property.SetValue(obj, stringValue.SanitizeText());
            }
            else if (propertyValue != null && !property.PropertyType.IsPrimitive && property.PropertyType.IsClass)
            {
                var sanitizedNestedObject = SanitizeObject(propertyValue);
                property.SetValue(obj, sanitizedNestedObject);
            }
        }

        return obj;
    }
}