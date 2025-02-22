using FluentValidation;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.UsesCases.Asset.Register
{
    public class RegisterAssetValidator : AbstractValidator<RequestRegisterAssetJson>
    {
        public RegisterAssetValidator()
        {
            RuleFor(asset => asset.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .Length(3, 100).WithMessage(ResourceMessagesException.INVALID_NUMBER_NAME);

            RuleFor(asset => asset.Description)
                .MaximumLength(200).WithMessage(ResourceMessagesException.MAXIMUM_DESCRIPTION_NUMBER);
           
            RuleFor(asset => asset.PatrimonyCode)
                .GreaterThan(0).When(asset => asset.PatrimonyCode.HasValue)
                .WithMessage(ResourceMessagesException.NEGATIVE_NUMBER);

            RuleFor(a => a.AcquisitionValue)
              .GreaterThan(0).WithMessage(ResourceMessagesException.NEGATIVE_NUMBER)
              .When(a => a.AcquisitionValue.HasValue);

            RuleFor(a => a.ConservationState)
              .IsInEnum().WithMessage(ResourceMessagesException.CONSERVATION_STATE_NOT_SUPPORTED_);

             RuleFor(a => a.SerieNumber)
                .MaximumLength(30).WithMessage(ResourceMessagesException.MAXIMUM_SERIE_NUMBER);

            RuleFor(a => a.CategoryId)
               .NotEmpty().WithMessage(ResourceMessagesException.INVALID_CATEGORY);

            RuleFor(a => a.RoomLocationId)
                .GreaterThan(0).WithMessage(ResourceMessagesException.INVALID_ROOM_LOCALIZATION)
                .When(a => a.RoomLocationId.HasValue);
        }
    }
}
