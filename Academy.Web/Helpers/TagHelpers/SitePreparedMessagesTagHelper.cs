using Academy.Domain.Enums.PreparedMessage;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;


[HtmlTargetElement("site-prep-messages")]
public class SitePreparedMessagesTagHelper : TagHelper
{
    private const string SectionHtmlAttributeName = "section";
    private const string ModelPropertyHtmlAttributeName = "for";

    [HtmlAttributeName(SectionHtmlAttributeName)]
    public PreparedMessageSection? Section { get; set; }

    [HtmlAttributeName(ModelPropertyHtmlAttributeName)]
    public ModelExpression? Property { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if(Property is null) throw new ArgumentNullException(nameof(Property));
        if(Section is null) throw new ArgumentNullException(nameof(Section));
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "bounceIn form-group");
        var htmlContent =
            $"<label class=\"form-label\">پیام های آماده</label>\n<select class=\"form-control\" id=\"modal-prepared-message\" data-select2-url=\"/site/prepared-messages-select?Section={Section}\" onchange=\"onPreparedMessageSelect('{Property.Name}')\"></select>";
        output.Content.SetHtmlContent(htmlContent);
    }
}