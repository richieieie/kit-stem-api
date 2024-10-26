using System.ComponentModel.DataAnnotations;

public class NotDefaultDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTimeOffset date && date == DateTimeOffset.MinValue)
        {
            return new ValidationResult(ErrorMessage ?? "The date must not be the default value.");
        }
        return ValidationResult.Success;
    }
}