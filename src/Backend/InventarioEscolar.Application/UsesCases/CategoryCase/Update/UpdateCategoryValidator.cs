using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public class UpdateCategoryValidator : AbstractValidator<CategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ResourceMessagesException.CATEGORY_ID_INVALID);

            Include(new RegisterCategoryValidator());
        }
    }
}

