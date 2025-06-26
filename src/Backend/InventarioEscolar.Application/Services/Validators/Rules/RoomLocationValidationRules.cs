using FluentValidation;
using InventarioEscolar.Communication.Dtos.Interfaces;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.Services.Validators.Rules
{
    public static class RoomLocationValidationRules
    {
        public static void Apply<T>(AbstractValidator<T> validator) where T : IRoomLocationBaseDto
        {
            validator.RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MinimumLength(2).WithMessage(ResourceMessagesException.ROOMLOCATION_NAME_TOOSHORT)
                .MaximumLength(200).WithMessage(ResourceMessagesException.ROOMLOCATION_NAME_TOOLONG);

            validator.RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage(ResourceMessagesException.ROOMLOCATION_DESCRIPTION_TOOLONG);

            validator.RuleFor(x => x.Building)
                .MaximumLength(50).WithMessage(ResourceMessagesException.ROOMLOCATION_BUILDING_TOOLONG);
        }
    }
}