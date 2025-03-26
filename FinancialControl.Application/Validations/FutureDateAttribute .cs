using System.ComponentModel.DataAnnotations;

namespace FinancialControl.Application.Validations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date && date > DateTime.Now.AddDays(1)) // Permite até 1 dia no futuro
            {
                return new ValidationResult("A data da transação não pode ser no futuro distante.");
            }
            return ValidationResult.Success;
        }
    }
}