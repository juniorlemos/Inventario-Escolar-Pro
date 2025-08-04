using CommonTestUtilities.Request;
using FluentValidation;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
using InventarioEscolar.Exceptions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validators.Test.School.Update
{
    public class UpdateSchoolValidatorTest
    {
        [Fact]
        public void Validate_UpdateSchoolValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new UpdateSchoolValidator();
            var request = UpdateSchoolCommandBuilder.Build();

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_UpdateSchoolValidator_WhenNameIsEmpty_ShouldHaveValidationError()
        {
            var validator = new UpdateSchoolValidator();
            var dto = UpdateSchoolCommandBuilder.Build();
            dto.SchoolDto.Name = string.Empty;

            var result = validator.Validate(dto.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "Name" && x.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_UpdateSchoolValidator_WhenNameIsTooShort_ShouldHaveValidationError()
        {
            var validator = new UpdateSchoolValidator();
            var dto = UpdateSchoolCommandBuilder.Build();
            dto.SchoolDto.Name = "A";

            var result = validator.Validate(dto.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "Name" && x.ErrorMessage == ResourceMessagesException.SCHOOL_NAME_TOOSHORT);
        }

        [Fact]
        public void Validate_UpdateSchoolValidator_WhenNameIsTooLong_ShouldHaveValidationError()
        {
            var validator = new UpdateSchoolValidator();
            var dto = UpdateSchoolCommandBuilder.Build();
            dto.SchoolDto.Name = new string('A', 101);

            var result = validator.Validate(dto.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "Name" && x.ErrorMessage == ResourceMessagesException.SCHOOL_NAME_TOOLONG);
        }

        [Fact]
        public void Validate_UpdateSchoolValidator_WhenInepIsTooLong_ShouldHaveValidationError()
        {
            var validator = new UpdateSchoolValidator();
            var dto = UpdateSchoolCommandBuilder.Build();
            dto.SchoolDto.Inep = new string('1', 21);

            var result = validator.Validate(dto.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "Inep" && x.ErrorMessage == ResourceMessagesException.SCHOOL_INEP_TOOLONG);
        }

        [Fact]
        public void Validate_UpdateSchoolValidator_WhenAddressIsTooLong_ShouldHaveValidationError()
        {
            var validator = new UpdateSchoolValidator();
            var dto = UpdateSchoolCommandBuilder.Build();
            dto.SchoolDto.Address = new string('R', 101);

            var result = validator.Validate(dto.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "Address" && x.ErrorMessage == ResourceMessagesException.SCHOOL_ADDRESS_TOOLONG);
        }

        [Fact]
        public void Validate_UpdateSchoolValidator_WhenCityIsTooLong_ShouldHaveValidationError()
        {
            var validator = new UpdateSchoolValidator();
            var dto = UpdateSchoolCommandBuilder.Build();
            dto.SchoolDto.City = new string('C', 31);

            var result = validator.Validate(dto.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.PropertyName == "City" && x.ErrorMessage == ResourceMessagesException.SCHOOL_CITY_TOOLONG);
        }
    }
}
