using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public class UpdateRoomLocationValidator : AbstractValidator<UpdateRoomLocationDto>
    {
        public UpdateRoomLocationValidator()
        {
            RoomLocationValidationRules.Apply(this);
        }
    }
}