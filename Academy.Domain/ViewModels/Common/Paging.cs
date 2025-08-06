using Microsoft.EntityFrameworkCore;

namespace Academy.Domain.ViewModels.Common;

public interface IAjaxSubsituationViewResult
{
    public bool IsAjax { get; set; }
}

public class BasePaging<T> : IAjaxSubsituationViewResult
{
    public BasePaging()
    {
        Page = 1;
        TakeEntity = 10;
        HowManyShowPageAfterAndBefore = 5;
        Entities = new List<T>();
    }

    public int Page { get; set; }

    public int PageCount { get; set; }

    public int AllEntitiesCount { get; set; }

    public int StartPage { get; set; }

    public int EndPage { get; set; }

    public int TakeEntity { get; set; } = 20;

    public int SkipEntity { get; set; }

    public int HowManyShowPageAfterAndBefore { get; set; }

    public int Counter { get; set; }

    public List<T> Entities { get; set; }

    public string? FormId { get; set; }


    public PagingViewModel GetCurrentPaging()
    {
        return new PagingViewModel
        {
            TakeEntity = this.TakeEntity ,
            EndPage = this.EndPage,
            Page = this.Page,
            StartPage = this.StartPage,
            FormId = this.FormId,
        };
    }

    public string GetShownEntitiesPagesTitle()
    {
        if (AllEntitiesCount != 0)
        {
            var startItem = 1;
            var endItem = AllEntitiesCount;

            if (EndPage > 1)
            {
                startItem = (Page - 1) * TakeEntity + 1;
                endItem = Page * TakeEntity > AllEntitiesCount ? AllEntitiesCount : Page * TakeEntity;
            }

            return $"نمایش {startItem} تا {endItem} از {AllEntitiesCount}";
        }

        return $"0 آیتم";
    }
    public async Task<BasePaging<T>> Paging(IQueryable<T> queryable, CancellationToken ct = default)
    {
        TakeEntity = TakeEntity;

        var allEntitiesCount = await queryable.CountAsync(ct);

        var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)TakeEntity));

        Page = Page > pageCount ? pageCount : Page;
        if (Page <= 0) Page = 1;
        AllEntitiesCount = allEntitiesCount;
        HowManyShowPageAfterAndBefore = HowManyShowPageAfterAndBefore;
        SkipEntity = (Page - 1) * TakeEntity;
        StartPage = Page - HowManyShowPageAfterAndBefore <= 0 ? 1 : Page - HowManyShowPageAfterAndBefore;
        EndPage = Page + HowManyShowPageAfterAndBefore > pageCount
            ? pageCount
            : Page + HowManyShowPageAfterAndBefore;
        PageCount = pageCount;
        Entities = await queryable.Skip(SkipEntity).Take(TakeEntity).ToListAsync(ct);
        Counter = ((Page - 1) * TakeEntity) + 1;

        return this;
    }

    public bool IsAjax { get; set; }
}
public class PagingViewModel
{
    public int TakeEntity { get; set; }
    
    public int Page { get; set; }

    public int StartPage { get; set; }

    public int EndPage { get; set; }

    public string? FormId { get; set; }
}



public class GroupedResult<TGroupKey, TModel>
{
    public TGroupKey Key { get; set; }
    public List<TModel> Items { get; set; } = new List<TModel>();
}

public class GroupedPaging<TGroupKey, TModel> : BasePaging<GroupedResult<TGroupKey, TModel>>
{
    public async Task<GroupedPaging<TGroupKey, TModel>> Paging(
        IQueryable<GroupedResult<TGroupKey, TModel>> queryable,
        CancellationToken ct = default)
    {
        var allEntitiesCount = await queryable.CountAsync(ct);

        var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)TakeEntity));

        Page = Page > pageCount ? pageCount : Page;
        if (Page <= 0) Page = 1;
        AllEntitiesCount = allEntitiesCount;
        HowManyShowPageAfterAndBefore = HowManyShowPageAfterAndBefore;
        SkipEntity = (Page - 1) * TakeEntity;
        StartPage = Page - HowManyShowPageAfterAndBefore <= 0 ? 1 : Page - HowManyShowPageAfterAndBefore;
        EndPage = Page + HowManyShowPageAfterAndBefore > pageCount
            ? pageCount
            : Page + HowManyShowPageAfterAndBefore;
        PageCount = pageCount;
        Entities = await queryable.Skip(SkipEntity).Take(TakeEntity).ToListAsync(ct);
        Counter = ((Page - 1) * TakeEntity) + 1;

        return this;
    }
}