using CommonTestUtilities.Dtos;
using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Exceptions;
using Shouldly;

namespace Validators.Test.RoomLocation.Update
{
    public class UpdateRoomLocationValidatorTest
    {
        [Fact]
        public void Validate_UpdateRoomLocationValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new UpdateRoomLocationValidator();
            var request = UpdateRoomLocationCommandBuilder.Build();

            var result = validator.Validate(request.RoomLocationDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_UpdateRoomLocationValidator_WhenNameIsEmpty_ShouldReturnValidationError()
        {
            var validator = new UpdateRoomLocationValidator();
            var dto = UpdateRoomLocationDtoBuilder.Build();
            dto.Name = string.Empty;

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_UpdateRoomLocationValidator_WhenNameIsTooShort_ShouldReturnValidationError()
        {
            var validator = new UpdateRoomLocationValidator();
            var dto = UpdateRoomLocationDtoBuilder.Build();
            dto.Name = "A";

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_NAME_TOOSHORT);
        }

        [Fact]
        public void Validate_UpdateRoomLocationValidator_WhenNameIsTooLong_ShouldReturnValidationError()
        {
            var validator = new UpdateRoomLocationValidator();
            var dto = UpdateRoomLocationDtoBuilder.Build();
            dto.Name = new string('N', 201);

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_NAME_TOOLONG);
        }

        [Fact]
        public void Validate_UpdateRoomLocationValidator_WhenDescriptionIsTooLong_ShouldReturnValidationError()
        {
            var validator = new UpdateRoomLocationValidator();
            var dto = UpdateRoomLocationDtoBuilder.Build();
            dto.Description = new string('D', 101);

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_DESCRIPTION_TOOLONG);
        }

        [Fact]
        public void Validate_UpdateRoomLocationValidator_WhenBuildingIsTooLong_ShouldReturnValidationError()
        {
            var validator = new UpdateRoomLocationValidator();
            var dto = UpdateRoomLocationDtoBuilder.Build();
            dto.Building = new string('B', 51);

            var result = validator.Validate(dto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_BUILDING_TOOLONG);
        }
    }
}