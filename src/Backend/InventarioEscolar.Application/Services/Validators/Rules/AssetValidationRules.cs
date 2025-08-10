using FluentValidation;
using InventarioEscolar.Communication.Dtos.Interfaces;
using InventarioEscolar.Exceptions;

namespace InventarioEscolar.Application.Services.Validators.Rules
{
    public static class AssetValidationRules
    {
        public static void Apply<T>(AbstractValidator<T> validator) where T : IAssetBaseDto
        {
            validator.RuleFor(asset => asset.Name)
               .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
               .Length(3, 100).WithMessage(ResourceMessagesException.INVALID_NUMBER_NAME);

            validator.RuleFor(asset => asset.Description)
                .MaximumLength(200).WithMessage(ResourceMessagesException.MAXIMUM_DESCRIPTION_NUMBER);

            validator.RuleFor(asset => asset.PatrimonyCode)
                .GreaterThan(0).When(asset => asset.PatrimonyCode.HasValue)
                .WithMessage(ResourceMessagesException.NEGATIVE_NUMBER);

            validator.RuleFor(a => a.AcquisitionValue)
              .GreaterThan(0).WithMessage(ResourceMessagesException.NEGATIVE_NUMBER)
              .When(a => a.AcquisitionValue.HasValue);

            validator.RuleFor(a => a.ConservationState)
              .IsInEnum().WithMessage(ResourceMessagesException.CONSERVATION_STATE_NOT_SUPPORTED_);

            validator.RuleFor(a => a.SerieNumber)
               .MaximumLength(30).WithMessage(ResourceMessagesException.MAXIMUM_SERIE_NUMBER);
        }
    }
}