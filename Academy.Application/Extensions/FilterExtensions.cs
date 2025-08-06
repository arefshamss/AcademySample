using System.Reflection;
using Academy.Domain.Attributes;

namespace Academy.Application.Extensions;

public static class FilterExtensions
{
    
    /// <summary>
    /// checks the properties of the given model that have <see cref="FilterInputAttribute"/> on it for any applied filter inside the model.
    /// this method only checks the declared properties of model that is public and the inherited members are excluded  
    /// </summary>
    /// <param name="model">the model to check for filter,s</param>
    /// <returns>A boolean that indicates whether an filter applied to the model or not</returns>
    public static bool HasFilters<TFilter>(this TFilter model) 
        where TFilter : class, new()
    {
        model ??= new TFilter();
        bool hasFilters = false;
        var properties = typeof(TFilter).GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                       BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.DeclaredOnly);
        foreach (var property in properties)
        {
            try
            {
                if (!Attribute.IsDefined(property, typeof(FilterInputAttribute))) continue;
                object? value = property.GetValue(model);
                if (value == null || value.Equals(default)) continue;
                if(property.PropertyType.IsEnum && Convert.ToInt32(value)  == 0) continue;
                hasFilters = true;
                break;
            }
            catch (TargetException) {continue;}
            catch (Exception) {break;}
        }

        return hasFilters;
    }
    
    
    public static int CountActiveFilters<TFilter>(this TFilter model)
        where TFilter : class, new()
    {
        model ??= new TFilter();

        int count = 0;
        var properties = typeof(TFilter).GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                       BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.DeclaredOnly);
        foreach (var property in properties)
        {
            try
            {
                if (!Attribute.IsDefined(property, typeof(FilterInputAttribute)))
                    continue;

                var value = property.GetValue(model);
                if (value == null)
                    continue;

                if (property.PropertyType == typeof(string))
                {
                    if (!string.IsNullOrWhiteSpace(value as string))
                        count++;
                    continue;
                }

                if (property.PropertyType.IsEnum)
                {
                    if (Convert.ToInt32(value) != 0)
                        count++;
                    continue;
                }

                var underlying = Nullable.GetUnderlyingType(property.PropertyType);
                if (underlying != null)
                {
                    var defaultVal = Activator.CreateInstance(underlying);
                    if (!value.Equals(defaultVal))
                        count++;
                    continue;
                }

                var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
                if (!value.Equals(defaultValue))
                    count++;
            }
            catch (TargetException) { continue; }
            catch (Exception) { continue; }
        }

        return count;
    }
}