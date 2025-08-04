using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Exceptions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validators.Test.RoomLocation.Register
{
    public class RegisterRoomLocationValidatorTest
    {
        [Fact]
        public void Validate_RegisterRoomLocationValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new RegisterRoomLocationValidator();
            var request = RegisterRoomLocationCommandBuilder.Build();

            var result = validator.Validate(request.RoomLocationDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_RegisterRoomLocationValidator_WhenNameIsEmpty_ShouldReturnValidationError()
        {
            var validator = new RegisterRoomLocationValidator();
            var request = RegisterRoomLocationCommandBuilder.Build();
            request.RoomLocationDto.Name = string.Empty;

            var result = validator.Validate(request.RoomLocationDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_RegisterRoomLocationValidator_WhenNameIsTooShort_ShouldReturnValidationError()
        {
            var validator = new RegisterRoomLocationValidator();
            var request = RegisterRoomLocationCommandBuilder.Build();
            request.RoomLocationDto.Name = "A";

            var result = validator.Validate(request.RoomLocationDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_NAME_TOOSHORT);
        }

        [Fact]
        public void Validate_RegisterRoomLocationValidator_WhenNameIsTooLong_ShouldReturnValidationError()
        {
            var validator = new RegisterRoomLocationValidator();
            var request = RegisterRoomLocationCommandBuilder.Build();
            request.RoomLocationDto.Name = new string('N', 201);

            var result = validator.Validate(request.RoomLocationDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_NAME_TOOLONG);
        }

        [Fact]
        public void Validate_RegisterRoomLocationValidator_WhenDescriptionIsTooLong_ShouldReturnValidationError()
        {
            var validator = new RegisterRoomLocationValidator();
            var request = RegisterRoomLocationCommandBuilder.Build();
            request.RoomLocationDto.Description = new string('D', 101);

            var result = validator.Validate(request.RoomLocationDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_DESCRIPTION_TOOLONG);
        }

        [Fact]
        public void Validate_RegisterRoomLocationValidator_WhenBuildingIsTooLong_ShouldReturnValidationError()
        {
            var validator = new RegisterRoomLocationValidator();
            var request = RegisterRoomLocationCommandBuilder.Build();
            request.RoomLocationDto.Building = new string('B', 51);

            var result = validator.Validate(request.RoomLocationDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.ROOMLOCATION_BUILDING_TOOLONG);
        }

    }
}
