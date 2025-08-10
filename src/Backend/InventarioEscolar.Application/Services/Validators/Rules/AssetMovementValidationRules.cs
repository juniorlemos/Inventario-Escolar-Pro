using FluentValidation;
using InventarioEscolar.Communication.Dtos.Interfaces;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.Services.Validators.Rules
{
    public static class AssetMovementValidationRules
    {
        public static void Apply<T>(AbstractValidator<T> validator) where T : IAssetMovementBaseDto
        {
            validator.RuleFor(m => m.AssetId)
                .GreaterThan(0).WithMessage(ResourceMessagesException.ASSETMOVEMENT_ASSET_ID_INVALID);
            validator.RuleFor(m => m.FromRoomId)
                .GreaterThan(0).WithMessage(ResourceMessagesException.ASSETMOVEMENT_FROM_ROOM_ID_INVALID);
            validator.RuleFor(m => m.ToRoomId)
                .GreaterThan(0).WithMessage(ResourceMessagesException.ASSETMOVEMENT_TO_ROOM_ID_INVALID);
            validator.RuleFor(m => m.MovedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceMessagesException.ASSETMOVEMENT_MOVED_AT_INVALID);
            validator.RuleFor(m => m.Responsible)
                .MaximumLength(100).WithMessage(ResourceMessagesException.ASSETMOVEMENT_RESPONSIBLE_NAME_TOO_LONG);
            validator.RuleFor(m => m.CancelReason)
                .MaximumLength(100).WithMessage(ResourceMessagesException.ASSETMOVEMENT_RESPONSIBLE_NAME_TOO_LONG);
        }
     }
}