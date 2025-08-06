using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

[HtmlTargetElement("input" ,Attributes = "placeholder-for")]
[HtmlTargetElement("textarea" ,Attributes = "placeholder-for")]
[HtmlTargetElement("select" ,Attributes = "placeholder-for")]
public class PlaceholderTagHelper : TagHelper
{   
    [HtmlAttributeName("placeholder-for")]
    public ModelExpression Property { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        string placeholder = Property.Metadata.DisplayName ?? Property.Metadata.PropertyName ?? "";
        
        output.Attributes.Add("placeholder" , placeholder);
    }
}