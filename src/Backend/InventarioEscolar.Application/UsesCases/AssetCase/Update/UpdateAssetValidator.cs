using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public class UpdateAssetValidator : AbstractValidator<UpdateAssetDto>
    {
        public UpdateAssetValidator()
        {
            AssetValidationRules.Apply(this);
        }
    }
}
