using Academy.Application.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;


[HtmlTargetElement("a", Attributes = "asp-ajax")]
[HtmlTargetElement("form", Attributes = "asp-ajax")]
public class AjaxTagHelper : TagHelper
{
    [HtmlAttributeName("asp-ajax-validation")]
    public bool ValidateForm { get; set; }

    [HtmlAttributeName("asp-ajax")] public bool IsActive { get; set; }

    [HtmlAttributeName("asp-ajax-method")] public AjaxMethod Method { get; set; } = AjaxMethod.Get;

    [HtmlAttributeName("asp-ajax-update")] public string? UpdateTarget { get; set; }

    [HtmlAttributeName("asp-ajax-success")]
    public string? OnSuccess { get; set; }

    [HtmlAttributeName("asp-ajax-failure")]
    public string? OnFailure { get; set; }

    [HtmlAttributeName("asp-ajax-begin")] public string? OnBegin { get; set; }

    [HtmlAttributeName("asp-ajax-mode")] public AjaxMode Mode { get; set; } = AjaxMode.Replace;

    [HtmlAttributeName("asp-ajax-url")] public string? Url { get; set; }

    [HtmlAttributeName("asp-ajax-modal-title")]
    public string? ModalTitle { get; set; }

    [HtmlAttributeName("asp-ajax-modal-type")]
    public ModalType ModalType { get; set; } = ModalType.None;

    [HtmlAttributeName("asp-ajax-modal-index")]
    public int ModalIndex { get; set; } = 1;
    
    [HtmlAttributeName("asp-ajax-upload")]
    public bool AjaxUpload { get; set; }
    
    [HtmlAttributeName("asp-ajax-reinitialize")]
    public bool ReinitializeComponents { get; set; } = true;
    
    [HtmlAttributeName("asp-ajax-confirmable")]
    public bool Confirmable { get; set; }
    
    [HtmlAttributeName("asp-ajax-confirmable-title")]
    public string? ConfirmableTitle { get; set; }

    [HtmlAttributeName("asp-ajax-confirmable-message")]
    public string? ConfirmableMessage { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!IsActive) return;
        if (output.TagName.ToLower().Trim() == "button")
        {
            output.Attributes.SetAttribute("type", "button");
        }

        if (ValidateForm)
        {
            bool isAttrExist = context.AllAttributes.TryGetAttribute("onsubmit", out var submitAttr);
            output.Attributes.SetAttribute("onsubmit",
                isAttrExist ? $"validateFormByElement(this);{submitAttr.Value}" : $"validateFormByElement(this);");
        }

        output.Attributes.SetAttribute("data-ajax", "true");
        output.Attributes.SetAttribute("data-ajax-method", Method.ToString().ToLower());
        output.Attributes.SetAttribute("data-ajax-mode", Mode.GetEnumName().ToLower());
        string targetModalBody = GetModalBodyId(ModalType , ModalIndex);
        output.Attributes.SetAttribute("data-ajax-update", UpdateTarget ?? targetModalBody);
        string modalFunction = ModalType switch
        {
            ModalType.Large => $"opLgModal('{ModalTitle}' , {ModalIndex});",
            ModalType.Medium => $"opModal('{ModalTitle}', {ModalIndex});",
            ModalType.Small => $"opSmModal('{ModalTitle}' , {ModalIndex});",
            ModalType.Left => $"opLeftModal('{ModalTitle}' , {ModalIndex});",
            ModalType.Right => $"opRightModal('{ModalTitle}' , {ModalIndex});",
            ModalType.None => string.Empty,
            _ => string.Empty,
        };
       
        string successFunction =
            $"{modalFunction}close_waiting();{OnSuccess}";
        
        #region Reinitialize Components

        if (ReinitializeComponents)
            successFunction += $"reinitializeTemplateComponents('{targetModalBody}');";

        #endregion

        #region Config Confirmable

        if (Confirmable && !string.IsNullOrWhiteSpace(ConfirmableTitle) &&
            !string.IsNullOrWhiteSpace(ConfirmableMessage))
        {
            output.Attributes.SetAttribute("onclick",
                $"showConfirmableAlert(event, '{ConfirmableTitle}', '{ConfirmableMessage}')");
        }

        #endregion
        
        #region Config Ajax Upload

        if (AjaxUpload)
        {
            output.Attributes.SetAttribute("data-ajax-upload", "true");
            output.Attributes.SetAttribute("enctype", "multipart/form-data");
        }

        #endregion

        string successFunctions =
            $"{modalFunction}close_waiting();reinitializeTemplateComponents('{targetModalBody}');";
        output.Attributes.SetAttribute("data-ajax-success", successFunctions + OnSuccess);
        output.Attributes.SetAttribute("data-ajax-failure", "onAjaxFailure(xhr, status, error);" + OnFailure);
        output.Attributes.SetAttribute("data-ajax-begin", "open_waiting();" + CloneModalFunction(ModalType , ModalIndex) + OnBegin);
        if (Url is not null)
            output.Attributes.SetAttribute("data-ajax-url", Url);
    }

    private static string CloneModalFunction(ModalType type, int index)
    {
        return type switch
        {
            ModalType.Large => $"cloneModal('lg' , '{index}');",
            ModalType.Medium => $"cloneModal('md' , '{index}');",
            ModalType.Small => $"cloneModal('sm' , '{index}');",
            ModalType.Left => $"cloneModal('left' , '{index}');",
            ModalType.Right => $"cloneModal('right' , '{index}');",
            _ => string.Empty
        };
    }
    
    private static string GetModalBodyId(ModalType type, int index)
    {
        string targetModalBody = type switch
        {
            ModalType.Large => "#modal-center-lg-body",
            ModalType.Medium => "#modal-center-md-body",
            ModalType.Small => "#modal-center-sm-body",
            ModalType.Left => "#modal-left-body",   
            ModalType.Right => "#modal-right-body",
            ModalType.None => string.Empty,
            _ => string.Empty
        };
        
        return (index == 1) ? targetModalBody : targetModalBody + "-" + index;
    }
}

public enum AjaxMethod
{
    Get,
    Post
}

public enum ModalType
{
    None,
    Small,
    Medium,
    Large,
    Left,
    Right
}

public enum AjaxMode
{
    Replace,
    Before,
    After,
}