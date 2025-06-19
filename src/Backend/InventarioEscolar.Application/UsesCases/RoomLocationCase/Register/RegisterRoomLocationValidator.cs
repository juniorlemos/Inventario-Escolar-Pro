using FluentValidation;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Register
{
    public class RegisterRoomLocationValidator : AbstractValidator<RoomLocationDto>
    {
        public RegisterRoomLocationValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
             .MinimumLength(2).WithMessage(ResourceMessagesException.ROOMLOCATION_NAME_TOOSHORT)
             .MaximumLength(200).WithMessage(ResourceMessagesException.ROOMLOCATION_NAME_TOOLONG);
             

            RuleFor(x => x.Description)
                .MaximumLength(100).WithMessage(ResourceMessagesException.ROOMLOCATION_DESCRIPTION_TOOLONG);

            RuleFor(x => x.Building)
                .MaximumLength(50).WithMessage(ResourceMessagesException.ROOMLOCATION_BUILDING_TOOLONG);
        }
    }
}
