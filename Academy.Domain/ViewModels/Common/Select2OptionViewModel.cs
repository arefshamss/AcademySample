namespace Academy.Domain.ViewModels.Common;


/// <summary>
/// Model for select options inside the javascript select 2 package
/// takes an Id for value of the options and a Text for the displayed name of the option 
/// </summary>
/// <typeparam name="T">represent,s the type of id for the option value</typeparam>
public class Select2OptionViewModel<T>
{
    public T Id { get; set; }
    
    public string Text { get; set; }
}

