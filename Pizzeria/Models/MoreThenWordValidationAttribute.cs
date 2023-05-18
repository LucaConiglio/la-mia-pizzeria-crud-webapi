using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class MoreThenWordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string fieldValue = (string)value;

            if (string.IsNullOrEmpty(fieldValue))
            {
                return new ValidationResult("Il campo é nullo, scrivi qualcosa!");
            }

            string[] parole = fieldValue.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            int conteggioParole = parole.Length;
            if (conteggioParole < 5)
            {
                return new ValidationResult("Il campo deve contere almeno 5 parole, attualmente hai scritto n: " + conteggioParole + " parole.");
            }
            return ValidationResult.Success;

        }
    }
}
