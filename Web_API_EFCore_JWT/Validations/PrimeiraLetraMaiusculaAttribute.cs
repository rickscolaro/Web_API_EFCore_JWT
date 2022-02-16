
using System.ComponentModel.DataAnnotations;

namespace APICatalogo_Fundamentos.Validations {

    // Validation cria uma DataAnnotations personalizada

    // Quando for usar uma classe como atribute no final é obrigado a colocar o sufixo Attribute
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute {

        protected override ValidationResult? IsValid(object? value,
            ValidationContext validationContext) {

            if (value == null || string.IsNullOrEmpty(value.ToString())) {

                return ValidationResult.Success;
            }

            var primeiraLetra = value.ToString()[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper()) {

                return new ValidationResult("A primeira letra do nome tem que ser maiúscula");
            }
            return ValidationResult.Success;

        }
    }
}