using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Application.Extensions;
using Academy.Application.Mapper.TicketMappings;
using Academy.Application.Mapper.TicketMessageMappings;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.Ticket;
using Academy.Domain.Models.Ticket;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Ticket.Admin;
using Academy.Domain.ViewModels.Ticket.Client;
using Microsoft.EntityFrameworkCore;

namespace Academy.Application.Services.Implementation;

public class TicketService(
    ITicketRepository ticketRepository,
    ITicketMessageRepository ticketMessageRepository) : ITicketService
{
    #region Admin
    
    #region FilterAsync

    public async Task<Result<AdminFilterTicketsViewModel>> FilterAsync(AdminFilterTicketsViewModel filter)
    {
        filter ??= new();

        var conditions = Filter.GenerateConditions<Ticket>();
        var orderConditions = Filter.GenerateOrder<Ticket>(x => x.CreatedDate, FilterOrderBy.Descending);

        #region Filters

        if (!filter.Title.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(x => EF.Functions.Like(x.Title, $"%{filter.Title}%"));

        if (filter.UserId > 0)
            conditions.Add(x => x.UserId == filter.UserId);
        
        if (!filter.FromDate.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.CreatedDate >= filter.FromDate!.ToMiladiDateTime());

        if (!filter.ToDate.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.CreatedDate <= filter.ToDate!.ToMiladiDateTime());
        

        switch (filter.TicketStatus)
        {
            case FilterByTicketStatus.Closed:
                conditions.Add(x => x.TicketStatus == TicketStatus.Closed);
                break;

            case FilterByTicketStatus.InProgress:
                conditions.Add(x => x.TicketStatus == TicketStatus.InProgress);
                break;

            case FilterByTicketStatus.SupporterAnswered:
                conditions.Add(x => x.TicketStatus == TicketStatus.SupporterAnswered);
                break;

            case FilterByTicketStatus.PendingForUserAnswer:
                conditions.Add(x => x.TicketStatus == TicketStatus.PendingForUserAnswer);
                break;
        }        

        switch (filter.TicketPriority)
        {
            case FilterByTicketPriority.Low:
                conditions.Add(x => x.TicketPriority == TicketPriority.Low);
                break;
            
            case FilterByTicketPriority.Medium:
                conditions.Add(x => x.TicketPriority == TicketPriority.Medium);
                break;
            
            case FilterByTicketPriority.High:
                conditions.Add(x => x.TicketPriority == TicketPriority.High);
                break;

        }        

        switch (filter.TicketSection)
        {
            case FilterByTicketSection.Supporter:
                conditions.Add(x => x.TicketSection == TicketSection.Supporter);
                break;
        }
        

        switch (filter.DeleteStatus)
        {
            case DeleteStatus.Deleted:
                conditions.Add(x => x.IsDeleted);
                break;

            case DeleteStatus.NotDeleted:
                conditions.Add(x => !x.IsDeleted);
                break;
        }

        #endregion
        
        string[] includes =
        [
            nameof(Ticket.User)
        ];

        await ticketRepository.FilterAsync(filter, conditions, x => x.MapToAdminTicketViewModel(), orderConditions, includes: includes);
        return filter;
    }
    

    #endregion

    #region CreateAsync

    public async Task<Result<int>> CreateAsync(AdminCreateTicketViewModel model)
    {
        #region Validations

        if (model.TicketMessage.SenderId < 1)
            return Result.Failure<int>(ErrorMessages.BadRequestError);

        #endregion

        var ticket = model.MapToTicket();

        await ticketRepository.InsertAsync(ticket);
        await ticketRepository.SaveChangesAsync();

        #region Add Ticket Message

        var ticketMessage = model.TicketMessage.MapTicketMessage();

        ticketMessage.TicketId = ticket.Id;

        if (model.TicketMessage.Attachment is not null)
        {
            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.TicketMessage.Attachment.FileName);
            var result = await model.TicketMessage.Attachment.AddFilesToServer(fileName, FilePaths.TicketMessageAttachmentFile);
            if (result.IsFailure)
                return Result.Failure<int>(result.Message);

            ticketMessage.Attachment = result.Value;
        }

        await ticketMessageRepository.InsertAsync(ticketMessage);
        await ticketMessageRepository.SaveChangesAsync();

        #endregion

        return Result.Success(ticket.Id, SuccessMessages.TicketInsertSuccessfullyDone);
    }
    

    #endregion

    #region FillModelForUpdateAsync

    public async Task<Result<AdminUpdateTicketViewModel>> FillModelForUpdateAsync(int id)
    {
        if (id < 1)
            return Result.Failure<AdminUpdateTicketViewModel>(ErrorMessages.BadRequestError);

        var ticket = await ticketRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (ticket is null)
            return Result.Failure<AdminUpdateTicketViewModel>(ErrorMessages.NotFoundError);

        return ticket.MapToAdminUpdateTicketViewModel();
    }

    #endregion
    
    #region UpdateAsync

    public async Task<Result> UpdateAsync(AdminUpdateTicketViewModel model)
    {
        if (model.Id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        var ticket = await ticketRepository.FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted);

        if (ticket is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        ticket.MapToTicket(model);

        ticketRepository.Update(ticket);
        await ticketRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }

    #endregion

    #region DeleteOrRecoverAsync

    public async Task<Result> DeleteOrRecoverAsync(int id)
    {
        if (id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        var ticket = await ticketRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (ticket is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        var result = ticketRepository.SoftDeleteOrRecover(ticket);
        await ticketRepository.SaveChangesAsync();

        return Result.Success(result ? SuccessMessages.DeleteSuccess : SuccessMessages.RecoverSuccess);
    }

    #endregion

    #region GetByIdAsync

    public async Task<Result<AdminTicketViewModel>> GetByIdAsync(int id)
    {
        if (id < 1)
            return Result.Failure<AdminTicketViewModel>(ErrorMessages.BadRequestError);


        var ticket = await ticketRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, includes:
        [
            nameof(Ticket.TicketMessages),
            $"{nameof(Ticket.TicketMessages)}.{nameof(TicketMessage.Sender)}",
            nameof(Ticket.User)
        ]);

        if (ticket is null)
            return Result.Failure<AdminTicketViewModel>(ErrorMessages.NotFoundError);

        #region Mark as Read

        //Mark ticket as read
        if (!ticket.ReadBySupporter)
        {
            ticket.ReadBySupporter = true;
            ticketRepository.Update(ticket);
            await ticketRepository.SaveChangesAsync();   
        }

        //Mark ticket messages as read
        await ticketMessageRepository.ExecuteUpdateAsync(
            x => x.TicketId == ticket.Id,
            x => x.SetProperty(c => c.ReadBySupporter, true));

        #endregion

        return ticket.MapToAdminTicketViewModel();
    }

    #endregion

    #region ToggleCloseTicketAsync

    public async Task<Result> ToggleCloseTicketAsync(int id, int userId)
    {
        if (id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        var ticket = await ticketRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, includes: nameof(Ticket.TicketMessages));

        if (ticket is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        if (ticket.TicketStatus is not TicketStatus.Closed)
        {
            ticket.TicketStatus = TicketStatus.Closed;
        }
        else
        {
            var lastMessage = ticket.TicketMessages.Last(x => x.TicketId == id);
            var ticketMessageCounts = ticket.TicketMessages.Count;
            ticket.TicketStatus = lastMessage.SenderId == userId ? (ticketMessageCounts > 1 ? TicketStatus.SupporterAnswered : TicketStatus.PendingForUserAnswer) : TicketStatus.InProgress;
        }

        ticketRepository.Update(ticket);
        await ticketRepository.SaveChangesAsync();

        return Result.Success();
    }

    #endregion

    #region CloseOldTicketsAsync

    public async Task CloseOldTicketsAsync()
    {
        var oneMonthAgo = DateTime.Now.AddMonths(-1).Date;
        await ticketRepository.ExecuteUpdateAsync(
            x => x.TicketStatus == TicketStatus.SupporterAnswered &&
                 x.ReadByUser &&
                 x.TicketMessages.Any() &&
                 x.TicketMessages
                     .OrderByDescending(c => c.CreatedDate)
                     .FirstOrDefault()!.CreatedDate.Date < oneMonthAgo,
            x => x.SetProperty(c => c.TicketStatus, TicketStatus.Closed)
        );
    }

    


    #endregion
    
    #endregion
    
    #region Client

    #region FilterAsync

    public async Task<Result<ClientFilterTicketsViewModel>> FilterAsync(ClientFilterTicketsViewModel filter)
    {
        filter ??= new();

        var conditions = Filter.GenerateConditions<Ticket>();
        var orderConditions = Filter.GenerateOrder<Ticket>(x => x.CreatedDate, FilterOrderBy.Descending);

        #region Filters

        conditions.Add(x => x.UserId == filter.UserId);
        conditions.Add(x => !x.IsDeleted);

        #endregion

        await ticketRepository.FilterAsync(filter, conditions, x => x.MapToClientTicketViewModel(), orderConditions, includes: [nameof(Ticket.User)]);
        return filter;
    }

    #endregion
    
    #region CreateAsync

    public async Task<Result<int>> CreateAsync(ClientCreateTicketViewModel model)
    {
        #region Validations

        if (model.Message.SenderId < 1)
            return Result.Failure<int>(ErrorMessages.BadRequestError);

        #endregion

        var ticket = model.MapToTicket();

        await ticketRepository.InsertAsync(ticket);
        await ticketRepository.SaveChangesAsync();

        #region Add Ticket Message

        var ticketMessage = model.Message.MapTicketMessage();

        ticketMessage.TicketId = ticket.Id;

        if (model.Message.Attachment is not null)
        {
            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(model.Message.Attachment.FileName);
            var result = await model.Message.Attachment.AddFilesToServer(fileName, FilePaths.TicketMessageAttachmentFile);
            if (result.IsFailure)
                return Result.Failure<int>(result.Message);

            ticketMessage.Attachment = result.Value;
        }

        await ticketMessageRepository.InsertAsync(ticketMessage);
        await ticketMessageRepository.SaveChangesAsync();

        #endregion

        return Result.Success(ticket.Id, SuccessMessages.TicketInsertSuccessfullyDone);
    }

    #endregion

    #region GetByIdForUserPanelAsync

    public async Task<Result<ClientTicketViewModel>> GetByIdForUserPanelAsync(int id)
    {
        if (id < 1)
            return Result.Failure<ClientTicketViewModel>(ErrorMessages.BadRequestError);


        var ticket = await ticketRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, includes:
        [
            nameof(Ticket.TicketMessages),
            $"{nameof(Ticket.TicketMessages)}.{nameof(TicketMessage.Sender)}",
            nameof(Ticket.User)
        ]);

        if (ticket is null)
            return Result.Failure<ClientTicketViewModel>(ErrorMessages.NotFoundError);

        #region Mark as Read

        //Mark ticket as read
        if (!ticket.ReadByUser)
        {
            ticket.ReadByUser = true;
            ticketRepository.Update(ticket);
            await ticketRepository.SaveChangesAsync();   
        }

        //Mark ticket messages as read
        await ticketMessageRepository.ExecuteUpdateAsync(
            x => x.TicketId == ticket.Id,
            x => x.SetProperty(c => c.ReadByUser, true));

        #endregion
        
        return ticket.MapToClientTicketViewModel();
    }

    #endregion

    #region GetUnreadTicketsCountAsync

    public async Task<int> GetUnreadTicketsCountAsync(int userId)
    {
        return await ticketRepository.CountAsync(x => x.UserId == userId && !x.ReadByUser && !x.IsDeleted);
    }

    #endregion


    #endregion
}