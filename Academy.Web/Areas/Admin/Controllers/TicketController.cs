using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.ViewModels.Ticket.Admin;
using Academy.Domain.ViewModels.TicketMessage.Admin;
using Academy.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.TicketManagement)]
public class TicketController(
    ITicketService ticketService,
    ITicketMessageService ticketMessageService) : AdminBaseController
{
    #region Ticket

    #region List

    [HttpGet]
    [UserActivityLog(Description = ActivityMessages.TicketsList), InvokePermission(PermissionName.TicketsList)]
    [AdminSearchMarker(Title = AdminSearchTitles.TicketsList)]
    public async Task<IActionResult> List(AdminFilterTicketsViewModel filter)
    {
        var result = await ticketService.FilterAsync(filter);

        return AjaxSubstitutionResult(filter, result);
    }
    
    [HttpGet]
    [UserActivityLog(Description = ActivityMessages.TicketsList), InvokePermission(PermissionName.TicketsList)]
    public async Task<IActionResult> ListPartial(AdminFilterTicketsViewModel filter)
    {
        var result = await ticketService.FilterAsync(filter);
        return PartialView("_ListPartial", result.Value);
    }

    #endregion
    
    #region Details

    [UserActivityLog(Description = ActivityMessages.TicketDetailsGet), InvokePermission(PermissionName.TicketDetails),HttpGet]

    public async Task<IActionResult> Details(int id)
    {
        var result = await ticketService.GetByIdAsync(id);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return RedirectToRefererUrl(nameof(List));
        }

        return View(result.Value);
    }

    #endregion

    #region Create

    [UserActivityLog(Description = ActivityMessages.CreateTicketGet), InvokePermission(PermissionName.CreateTicket),HttpGet]
    [AdminSearchMarker(Title = AdminSearchTitles.CreateTicket)]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.CreateTicketPost), InvokePermission(PermissionName.CreateTicket)]
    public async Task<IActionResult> Create(AdminCreateTicketViewModel model)
    {
        model.TicketMessage.SenderId = User.GetUserId();

        if (!ModelState.IsValid)
        {
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }

        var result = await ticketService.CreateAsync(model);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return View(model);
        }

        ShowToasterSuccessMessage(result.Message);
        return RedirectToAction(nameof(Details), new { id = result.Value });
    }

    #endregion

    #region Update

    [UserActivityLog(Description = ActivityMessages.UpdateTicketGet), InvokePermission(PermissionName.UpdateTicket),HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var result = await ticketService.FillModelForUpdateAsync(id);

        if (result.IsFailure)
            return BadRequest(result.Message);

        return PartialView("_UpdatePartial", result.Value);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.UpdateTicketPost), InvokePermission(PermissionName.UpdateTicket)]
    public async Task<IActionResult> Update(AdminUpdateTicketViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetModelErrorsAsString());

        var result = await ticketService.UpdateAsync(model);

        if (result.IsFailure)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    #endregion

    #region DeleteOrRecover

    [UserActivityLog(Description = ActivityMessages.DeleteOrRecoverTicket), InvokePermission(PermissionName.DeleteOrRecoverTicket),HttpGet]
    public async Task<IActionResult> DeleteOrRecover(int id)
    {
        var result = await ticketService.DeleteOrRecoverAsync(id);
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }

    #endregion

    #region Toggle Close

    [UserActivityLog(Description = ActivityMessages.ToggleCloseTicketGet), InvokePermission(PermissionName.ToggleCloseTicket),HttpGet]
    public async Task<IActionResult> ToggleClose(int id)
    {
        var result = await ticketService.ToggleCloseTicketAsync(id, User.GetUserId());

        if (result.IsFailure)
            ShowToasterErrorMessage(result.Message);
        else
            ShowToasterSuccessMessage(result.Message);

        return RedirectToAction(nameof(Details), new { id = id });
    }

    #endregion
    
    #endregion

    #region TicketMessage

    #region Create

    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.CreateTicketMessagePost), InvokePermission(PermissionName.CreateTicketMessage)]
    public async Task<IActionResult> CreateTicketMessage(AdminCreateTicketMessageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return RedirectToAction(nameof(Details), new { id = model.TicketId });
        }


        var result = await ticketMessageService.CreateAsync(model);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return RedirectToAction(nameof(Details), new { id = model.TicketId });
        }

        ShowToasterSuccessMessage(result.Message);
        return RedirectToAction(nameof(Details), new { id = model.TicketId });
    }

    #endregion

    #endregion


}