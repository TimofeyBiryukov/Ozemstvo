using System.Windows.Controls;

namespace OzemstvoWPF
{
    public class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string? stringToValidate = value as string;
            if (string.IsNullOrEmpty(stringToValidate))
            {
                return new ValidationResult(false, "This field is required");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class TemplateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string? stringToValidate = value as string;
            ValidationResult result = new ValidationResult(false, "Template must contain {{url}}");
            if (string.IsNullOrEmpty(stringToValidate))
            {
                return result;
            }
            if (!stringToValidate.Contains("{{url}}"))
            {
                return result;
            }
            return ValidationResult.ValidResult;
        }
    }
}
