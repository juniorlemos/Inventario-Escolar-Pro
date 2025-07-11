using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Register
{
    public class RegisterAssetMovementValidator : AbstractValidator<AssetMovementDto>
    {
        public RegisterAssetMovementValidator()
        {
            AssetMovementValidationRules.Apply(this);
        }
    }
}
