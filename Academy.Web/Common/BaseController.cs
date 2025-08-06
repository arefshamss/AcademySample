using Academy.Web.Helpers.FileResults;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Common;

public class BaseController : Controller
{
    protected static string SweetAlertSuccessMessage = MessageKey.SweetAlertSuccessMessage;
    protected static string SweetAlertErrorMessage = MessageKey.SweetAlertErrorMessage;
    protected static string SweetAlertInfoMessage = MessageKey.SweetAlertInfoMessage;
    protected static string SweetAlertWarningMessage = MessageKey.SweetAlertWarningMessage;

    protected static string ToasterSuccessMessage = MessageKey.ToasterSuccessMessage;
    protected static string ToasterErrorMessage = MessageKey.ToasterErrorMessage;
    protected static string ToasterInfoMessage = MessageKey.ToasterInfoMessage;
    protected static string ToasterWarningMessage = MessageKey.ToasterWarningMessage;

    #region Notifications

    #region Sweet Alert

    public void ShowSweetAlertSuccessMessage() =>
        TempData[SweetAlertSuccessMessage] = "عملیات با موفقیت انجام شد";

    public void ShowSweetAlertSuccessMessage(string message) =>
        TempData[SweetAlertSuccessMessage] = message;

    public void ShowSweetAlertErrorMessage() =>
        TempData[SweetAlertErrorMessage] = "عملیات با شکست مواجه شد";

    public void ShowSweetAlertErrorMessage(string errorMessage) =>
        TempData[SweetAlertErrorMessage] = errorMessage;

    public void ShowSweetAlertInfoMessage(string message) =>
        TempData[SweetAlertInfoMessage] = message;

    public void ShowWarningMessage(string message) =>
        TempData[SweetAlertWarningMessage] = message;

    #endregion

    #region Toaster

    public void ShowToasterSuccessMessage() =>
        TempData[ToasterSuccessMessage] = "عملیات با موفقیت انجام شد";

    public void ShowToasterSuccessMessage(string message) =>
        TempData[ToasterSuccessMessage] = message;

    public void ShowToasterErrorMessage() =>
        TempData[ToasterErrorMessage] = "عملیات با شکست مواجه شد";

    public void ShowToasterErrorMessage(string errorMessage) =>
        TempData[ToasterErrorMessage] = errorMessage;

    public void ShowToasterInfoMessage(string message) =>
        TempData[ToasterInfoMessage] = message;

    public void ShowToasterWarningMessage(string message) =>
        TempData[ToasterWarningMessage] = message;

    #endregion

    #endregion

    #region Actions

    protected IActionResult RedirectToRefererUrl()
    {
        var referrerUrl = HttpContext.Request.Headers["Referer"].ToString();
        if (string.IsNullOrEmpty(referrerUrl) || !Url.IsLocalUrl(referrerUrl))
            return RedirectToAction("Index", "Home", new { area = "" });    

        return Redirect(referrerUrl);
    }

    protected IActionResult RedirectToRefererUrl(string fallBackUrl)
    {
        var referrerUrl = HttpContext.Request.Headers["Referer"].ToString();
        if (string.IsNullOrEmpty(referrerUrl) || !Url.IsLocalUrl(referrerUrl))
            return Redirect(fallBackUrl);

        return Redirect(referrerUrl);
    }

    protected IActionResult RedirectToRefererUrl(string fallBackAction, string fallBackController, string fallBackArea = "")
    {
        var referrerUrl = HttpContext.Request.Headers["Referer"].ToString();
        if (string.IsNullOrEmpty(referrerUrl) || !Url.IsLocalUrl(referrerUrl))
            return RedirectToAction(fallBackAction, fallBackController, new { area = fallBackArea });

        return Redirect(referrerUrl);
    }


    protected IActionResult RedirectToRefererUrl(string fallBackAction, string fallBackController, object? routeValues = null)
    {
        var referrerUrl = HttpContext.Request.Headers["Referer"].ToString();
        if (string.IsNullOrEmpty(referrerUrl) || !Url.IsLocalUrl(referrerUrl))
            return RedirectToAction(fallBackAction, fallBackController, routeValues);
        return Redirect(referrerUrl);
    }


    protected IActionResult AjaxSubstitutionResult<TFilter>(TFilter filter, Result<TFilter> result) where TFilter : IAjaxSubsituationViewResult
    {
        if (filter.IsAjax)
        {
            filter.IsAjax = false;
            return PartialView(result.Value);
        }

        filter.IsAjax = false;
        return View(result.Value);
    }

    protected PdfFileResult Pdf(byte[] fileContents) => new (fileContents);
    
    protected ExcelFileResult Excel(byte[] fileContents) => new (fileContents);
    
    #endregion

    #region Tools

    public object? PeekTempData(string key) =>
        TempData.Peek(key);

    public void KeepTempData(string key, object value)
    {
        TempData[key] = value;
        TempData.Keep(key);
    }

    public void RemoveTempData(string key) =>
        TempData.Remove(key);

    #endregion
}