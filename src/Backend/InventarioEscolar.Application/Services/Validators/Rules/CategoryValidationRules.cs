using FluentValidation;
using InventarioEscolar.Communication.Dtos.Interfaces;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.Services.Validators.Rules
{
    public static class CategoryValidationRules
    {
        public static void Apply<T>(AbstractValidator<T> validator) where T : ICategoryBaseDto
        {
            validator.RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MinimumLength(2).WithMessage(ResourceMessagesException.CATEGORY_NAME_TOOSHORT)
                .MaximumLength(100).WithMessage(ResourceMessagesException.CATEGORY_NAME_TOOLONG);

            validator.RuleFor(x => x.Description)
               .MaximumLength(200).WithMessage(ResourceMessagesException.CATEGORY_DESCRIPTION_TOOLONG);
        }
    }
}