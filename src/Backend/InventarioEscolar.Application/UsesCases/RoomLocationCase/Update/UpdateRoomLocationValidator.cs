using FluentValidation;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public class UpdateRoomLocationValidator : AbstractValidator<RoomLocationDto>
    {
        public UpdateRoomLocationValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ResourceMessagesException.ROOMLOCATION_ID_INVALID);

            Include(new RegisterRoomLocationValidator());
        }
    }
}