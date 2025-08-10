using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            CategoryValidationRules.Apply(this);
        }
    }
}