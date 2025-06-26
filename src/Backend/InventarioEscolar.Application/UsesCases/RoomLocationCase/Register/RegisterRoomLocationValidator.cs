using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Register
{
    public class RegisterRoomLocationValidator : AbstractValidator<RoomLocationDto>
    {
        public RegisterRoomLocationValidator()
        {
            RoomLocationValidationRules.Apply(this);
        }
    }
}