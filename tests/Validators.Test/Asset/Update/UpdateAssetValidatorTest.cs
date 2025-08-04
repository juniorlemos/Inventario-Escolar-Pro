using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Communication.Enum;
using InventarioEscolar.Exceptions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validators.Test.Asset.Update
{
    public class UpdateAssetValidatorTest
    {
        [Fact]
        public void Validate_UpdateAssetValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenNameIsEmpty_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();
            request.AssetDto.Name = string.Empty;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenNameTooShort_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();
            request.AssetDto.Name = "ab";

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.INVALID_NUMBER_NAME);
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenNameTooLong_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();
            request.AssetDto.Name = new string('a', 101);

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.INVALID_NUMBER_NAME);
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenDescriptionTooLong_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();
            request.AssetDto.Description = new string('d', 201);

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.MAXIMUM_DESCRIPTION_NUMBER);
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenPatrimonyCodeIsNegative_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();
            request.AssetDto.PatrimonyCode = -10;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NEGATIVE_NUMBER);
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenAcquisitionValueIsNegative_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();
            request.AssetDto.AcquisitionValue = -500;

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NEGATIVE_NUMBER);
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenConservationStateIsInvalid_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();

            request.AssetDto.ConservationState = (ConservationState)999; 

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.CONSERVATION_STATE_NOT_SUPPORTED_);
        }

        [Fact]
        public void Validate_UpdateAssetValidator_WhenSerieNumberIsTooLong_ShouldHaveValidationError()
        {
            var validator = new UpdateAssetValidator();
            var request = UpdateAssetCommandBuilder.Build();
            request.AssetDto.SerieNumber = new string('s', 31);

            var result = validator.Validate(request.AssetDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.MAXIMUM_SERIE_NUMBER);
        }
    }
}
