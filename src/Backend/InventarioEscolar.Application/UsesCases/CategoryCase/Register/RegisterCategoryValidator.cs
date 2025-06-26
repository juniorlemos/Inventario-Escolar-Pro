using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

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
