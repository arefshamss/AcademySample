using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Attributes;

public class MinRangeAttribute : ValidationAttribute
{
    private readonly double _minValue;

    public MinRangeAttribute(double minValue)
    {
        _minValue = minValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));


        if (double.TryParse(value.ToString(), out double doubleValue))
            if (doubleValue >= _minValue)
                return ValidationResult.Success;
            else
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));



        return new ValidationResult("مقدار وارد شده صحیح نمی باشد");
    }

    public override string FormatErrorMessage(string name)
     => string.Format(ErrorMessageString, name, _minValue);

}