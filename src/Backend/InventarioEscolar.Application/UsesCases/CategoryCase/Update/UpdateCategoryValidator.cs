using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public class UpdateCategoryValidator : AbstractValidator<UploadCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            CategoryValidationRules.Apply(this);
        }
    }
}

