using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Domain.Enums;
using InventarioEscolar.Exceptions;
using Shouldly;

namespace Validators.Test.Asset.Register
{
    public class RegisterAssetValidatorTest
    {
        [Fact]
        public void Validate_RegisterAssetValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenNameIsEmpty_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.Name = string.Empty;

            var result = validator.Validate(request.AssetDto);
            
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenNameTooShort_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.Name = "ab";

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.INVALID_NUMBER_NAME);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenNameTooLong_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.Name = new string('a', 101);

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.INVALID_NUMBER_NAME);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenDescriptionTooLong_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.Description = new string('d', 201);

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.MAXIMUM_DESCRIPTION_NUMBER);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenPatrimonyCodeNegative_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.PatrimonyCode = -10;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NEGATIVE_NUMBER);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenAcquisitionValueNegative_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.AcquisitionValue = -500;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NEGATIVE_NUMBER);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenConservationStateInvalid_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.ConservationState = (ConservationState)999;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.CONSERVATION_STATE_NOT_SUPPORTED_);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenSerieNumberTooLong_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.SerieNumber = new string('s', 31);

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.MAXIMUM_SERIE_NUMBER);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenCategoryIdIsEmpty_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.CategoryId = 0;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.INVALID_CATEGORY);
        }

        [Fact]
        public void Validate_RegisterAssetValidator_WhenRoomLocationIdIsNegative_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetValidator();
            var request = RegisterAssetCommandBuilder.Build();
            request.AssetDto.RoomLocationId = -1;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.INVALID_ROOM_LOCALIZATION);
        }
    }
}