using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Register
{
    public class RegisterCategoryValidator : AbstractValidator<CategoryDto>
    {
        public RegisterCategoryValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MinimumLength(2).WithMessage(ResourceMessagesException.CATEGORY_NAME_TOOSHORT)
                .MaximumLength(100).WithMessage(ResourceMessagesException.CATEGORY_NAME_TOOLONG);

            RuleFor(x => x.Description)
               .MaximumLength(200).WithMessage(ResourceMessagesException.CATEGORY_DESCRIPTION_TOOLONG);

        }
    }
}
