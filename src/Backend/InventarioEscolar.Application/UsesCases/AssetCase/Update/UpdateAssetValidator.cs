using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public class UpdateAssetValidator : AbstractValidator<AssetDto>
    {
        public UpdateAssetValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(ResourceMessagesException.ASSET_ID_INVALID);

            Include(new RegisterAssetValidator());
        }
    }
}
