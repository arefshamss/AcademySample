using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Academy.Web.Extensions;

internal static class ModelStateExtensions
{
    internal static string GetModelErrorsAsString(this ModelStateDictionary modelState)
    {
        var modelStateErrors = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            .ToList();
        StringBuilder stringBuilder  = new ();
        stringBuilder.AppendLine("لطفا خطا های زیر را برطرف نمایید : ");
        int i = 1;
        modelStateErrors.ForEach(error =>
        {
            stringBuilder.AppendLine(i++ + ". " + error);
        });
        
        return stringBuilder.ToString();
    }
}