namespace Academy.Domain.ViewModels.Common;

/// <summary>
/// Filters The Selected Option 
/// </summary>
/// <typeparam name="TValue">represent,s the type of id for the option value</typeparam>
public class FilterSelect2OptionsViewModel<TValue> : BasePaging<Select2OptionViewModel<TValue>>
{
    public string? Parameter { get; set; }
}

/// <summary>
/// Filters The Selected Option 
/// </summary>
/// <typeparam name="TValue">represent,s the type of id for the option value</typeparam>
/// <typeparam name="TAdditionalItem">represent,s the type of additional item you want to pass into the model</typeparam>

public class FilterSelect2OptionsViewModel<TValue, TAdditionalItem> : BasePaging<Select2OptionViewModel<TValue>>
{
    public TAdditionalItem? AdditionalItem { get; set; }
    
    public string? Parameter { get; set; }
}