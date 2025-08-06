namespace Academy.Domain.ViewModels.Common;

public class SelectViewModel<TKey>
{
    public TKey Id { get; set; }
    
    public string DisplayValue { get; set; }
}