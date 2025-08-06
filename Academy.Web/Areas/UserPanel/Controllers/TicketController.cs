using Academy.Web.Areas.UserPanel.Controllers.Common;
using Academy.Web.Extensions;
using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Domain.ViewModels.Ticket.Client;
using Academy.Domain.ViewModels.TicketMessage.Client;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.UserPanel.Controllers;

public class TicketController(
    ITicketService ticketService,
    ITicketMessageService ticketMessageService) : UserPanelBaseController
{
    #region Ticket

    #region List

    [HttpGet(RoutingExtension.UserPanel.Ticket.List)]
    public async Task<IActionResult> List(ClientFilterTicketsViewModel filter)
    {
        filter.UserId = User.GetUserId();
        var result = (await ticketService.FilterAsync(filter)).Value;
        return View(result);
    }

    #endregion

    #region Details

    [HttpGet(RoutingExtension.UserPanel.Ticket.Detail)]
    public async Task<IActionResult> Details(int id)
    {
        var result = await ticketService.GetByIdForUserPanelAsync(id);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
        }

        return View(result.Value);
    }

    #endregion

    #region Create

    [HttpGet(RoutingExtension.UserPanel.Ticket.Create)]
    public async Task<IActionResult> Create()
    {
        var model = new ClientCreateTicketViewModel
        {
            Message = new ClientCreateTicketMessageViewModel()
        };
        
        return View(model);
    }

    [HttpPost(RoutingExtension.UserPanel.Ticket.Create)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClientCreateTicketViewModel model)
    {
        model.UserId = User.GetUserId();
        
        if (model.Message == null)
            model.Message = new ClientCreateTicketMessageViewModel();
        
        model.Message.SenderId =User.GetUserId();
        
        if (!ModelState.IsValid)
            return View(model);

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

    #endregion

    #region TicketMessage
    
    #region Create

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTicketMessage(ClientCreateTicketMessageViewModel model)
    {
        model.SenderId = User.GetUserId();
        
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