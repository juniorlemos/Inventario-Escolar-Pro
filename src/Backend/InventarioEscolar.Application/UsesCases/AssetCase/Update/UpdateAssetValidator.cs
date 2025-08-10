using FluentValidation;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Communication.Dtos;

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