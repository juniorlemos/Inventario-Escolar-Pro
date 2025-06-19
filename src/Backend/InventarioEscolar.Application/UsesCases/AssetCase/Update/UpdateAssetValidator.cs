using FluentValidation;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public class UpdateAssetValidator : AbstractValidator<RequestUpdateAssetJson>
    {
        public UpdateAssetValidator()
        {
            RuleFor(asset => asset.Id)
                .GreaterThan(0).WithMessage(ResourceMessagesException.ASSET_NOT_FOUND);

            RuleFor(asset => asset.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .Length(3, 100).WithMessage(ResourceMessagesException.INVALID_NUMBER_NAME)
                .When(asset => asset.Name != null);

            RuleFor(asset => asset.Description)
                .MaximumLength(200).WithMessage(ResourceMessagesException.MAXIMUM_DESCRIPTION_NUMBER)
                .When(asset => asset.Description != null);

            RuleFor(asset => asset.PatrimonyCode)
                .GreaterThan(0).WithMessage(ResourceMessagesException.NEGATIVE_NUMBER)
                .When(asset => asset.PatrimonyCode.HasValue);

            RuleFor(asset => asset.AcquisitionValue)
                .GreaterThan(0).WithMessage(ResourceMessagesException.NEGATIVE_NUMBER)
                .When(asset => asset.AcquisitionValue.HasValue);

            RuleFor(asset => asset.ConservationState)
                .IsInEnum().WithMessage(ResourceMessagesException.CONSERVATION_STATE_NOT_SUPPORTED_);

            RuleFor(asset => asset.SerieNumber)
                .MaximumLength(30).WithMessage(ResourceMessagesException.MAXIMUM_SERIE_NUMBER)
                .When(asset => asset.SerieNumber != null);

            RuleFor(asset => asset.CategoryId)
                .NotEmpty().WithMessage(ResourceMessagesException.INVALID_CATEGORY);

            RuleFor(asset => asset.RoomLocationId)
                .GreaterThan(0).WithMessage(ResourceMessagesException.INVALID_ROOM_LOCALIZATION)
                .When(asset => asset.RoomLocationId.HasValue);

        }
    }
}