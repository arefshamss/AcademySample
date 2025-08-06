using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

[HtmlTargetElement("validation-span", TagStructure = TagStructure.WithoutEndTag)]
public class ValidationTagHelper(IHtmlGenerator generator) : ValidationMessageTagHelper(generator)
{
    #region Attribute Names

    public const string ModeAttributeName = "mode";

    #endregion

    #region Attributes

    [HtmlAttributeName(ModeAttributeName)] public ValidationSpanMode Mode { get; set; } = ValidationSpanMode.Site;

    #endregion

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        switch (Mode)
        {
            case ValidationSpanMode.Site:
                output.Attributes.SetAttribute("class", "mt-1 text-danger");
                break;

            case ValidationSpanMode.Admin:
                break;

            case ValidationSpanMode.UserPanel:
                break;
        }


        output.TagMode = TagMode.StartTagAndEndTag;

        await base.ProcessAsync(context, output);
    }
}

public enum ValidationSpanMode : byte
{
    Site,
    Admin,
    UserPanel
}