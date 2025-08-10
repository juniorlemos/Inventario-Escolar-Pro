using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using InventarioEscolar.Exceptions;
using Shouldly;

namespace Validators.Test.AssetMovement.Register
{
    public class RegisterAssetMovementValidatorTest
    {
        [Fact]
        public void Validate_RegisterAssetMovementValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new RegisterAssetMovementValidator();
            var request = RegisterAssetMovementCommandBuilder.Build();

            var result = validator.Validate(request.AssetMovementDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_RegisterAssetMovementValidator_WhenAssetIdIsInvalid_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetMovementValidator();
            var dto = RegisterAssetMovementCommandBuilder.Build().AssetMovementDto;
            dto.AssetId = 0;

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "AssetId" && x.ErrorMessage == ResourceMessagesException.ASSETMOVEMENT_ASSET_ID_INVALID);
        }

        [Fact]
        public void Validate_RegisterAssetMovementValidator_WhenFromRoomIdIsInvalid_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetMovementValidator();
            var dto = RegisterAssetMovementCommandBuilder.Build().AssetMovementDto;
            dto.FromRoomId = 0;

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "FromRoomId" && x.ErrorMessage == ResourceMessagesException.ASSETMOVEMENT_FROM_ROOM_ID_INVALID);
        }

        [Fact]
        public void Validate_RegisterAssetMovementValidator_WhenToRoomIdIsInvalid_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetMovementValidator();
            var dto = RegisterAssetMovementCommandBuilder.Build().AssetMovementDto;
            dto.ToRoomId = 0;

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "ToRoomId" && x.ErrorMessage == ResourceMessagesException.ASSETMOVEMENT_TO_ROOM_ID_INVALID);
        }

        [Fact]
        public void Validate_RegisterAssetMovementValidator_WhenMovedAtIsInTheFuture_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetMovementValidator();
            var dto = RegisterAssetMovementCommandBuilder.Build().AssetMovementDto;
            dto.MovedAt = DateTime.UtcNow.AddMinutes(5); 

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "MovedAt" && x.ErrorMessage == ResourceMessagesException.ASSETMOVEMENT_MOVED_AT_INVALID);
        }

        [Fact]
        public void Validate_RegisterAssetMovementValidator_WhenResponsibleIsTooLong_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetMovementValidator();
            var dto = RegisterAssetMovementCommandBuilder.Build().AssetMovementDto;
            dto.Responsible = new string('A', 101);

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "Responsible" && x.ErrorMessage == ResourceMessagesException.ASSETMOVEMENT_RESPONSIBLE_NAME_TOO_LONG);
        }

        [Fact]
        public void Validate_RegisterAssetMovementValidator_WhenCancelReasonIsTooLong_ShouldHaveValidationError()
        {
            var validator = new RegisterAssetMovementValidator();
            var dto = RegisterAssetMovementCommandBuilder.Build().AssetMovementDto;
            dto.CancelReason = new string('B', 101);

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "CancelReason" && x.ErrorMessage == ResourceMessagesException.ASSETMOVEMENT_RESPONSIBLE_NAME_TOO_LONG);
        }
    }
}