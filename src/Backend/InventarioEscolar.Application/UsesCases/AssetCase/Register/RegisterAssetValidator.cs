using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Validators.Rules;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Register
{
    public class RegisterAssetValidator : AbstractValidator<AssetDto>
    {
        public RegisterAssetValidator()
        {
            AssetValidationRules.Apply(this);

            RuleFor(a => a.CategoryId)
               .NotEmpty().WithMessage(ResourceMessagesException.INVALID_CATEGORY);

            RuleFor(a => a.RoomLocationId)
                .GreaterThan(0).WithMessage(ResourceMessagesException.INVALID_ROOM_LOCALIZATION)
                .When(a => a.RoomLocationId.HasValue);
        }
    }
}
