using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Register
{
    public class RegisterCategoryValidator : AbstractValidator<CategoryDto>
    {
        public RegisterCategoryValidator() 
        {
            CategoryValidationRules.Apply(this);
        }
    }
}