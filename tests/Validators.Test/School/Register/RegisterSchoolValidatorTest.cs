using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Exceptions;
using Shouldly;

namespace Validators.Test.School.Register
{
    public class RegisterSchoolValidatorTest
    {
        [Fact]
        public void Validate_RegisterSchoolValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new RegisterSchoolValidator();
            var request = RegisterSchoolCommandBuilder.Build();

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_RegisterSchoolValidator_WhenNameIsEmpty_ShouldReturnNameEmptyError()
        {
            var validator = new RegisterSchoolValidator();
            var request = RegisterSchoolCommandBuilder.Build();
            request.SchoolDto.Name = string.Empty;

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_RegisterSchoolValidator_WhenNameIsTooShort_ShouldReturnNameTooShortError()
        {
            var validator = new RegisterSchoolValidator();
            var request = RegisterSchoolCommandBuilder.Build();
            request.SchoolDto.Name = "A";

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.SCHOOL_NAME_TOOSHORT);
        }

        [Fact]
        public void Validate_RegisterSchoolValidator_WhenNameIsTooLong_ShouldReturnNameTooLongError()
        {
            var validator = new RegisterSchoolValidator();
            var request = RegisterSchoolCommandBuilder.Build();
            request.SchoolDto.Name = new string('N', 101);

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.SCHOOL_NAME_TOOLONG);
        }

        [Fact]
        public void Validate_RegisterSchoolValidator_WhenInepIsTooLong_ShouldReturnInepTooLongError()
        {
            var validator = new RegisterSchoolValidator();
            var request = RegisterSchoolCommandBuilder.Build();
            request.SchoolDto.Inep = new string('1', 21);

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.SCHOOL_INEP_TOOLONG);
        }

        [Fact]
        public void Validate_RegisterSchoolValidator_WhenAddressIsTooLong_ShouldReturnAddressTooLongError()
        {
            var validator = new RegisterSchoolValidator();
            var request = RegisterSchoolCommandBuilder.Build();
            request.SchoolDto.Address = new string('A', 101);

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.SCHOOL_ADDRESS_TOOLONG);
        }

        [Fact]
        public void Validate_RegisterSchoolValidator_WhenCityIsTooLong_ShouldReturnCityTooLongError()
        {
            var validator = new RegisterSchoolValidator();
            var request = RegisterSchoolCommandBuilder.Build();
            request.SchoolDto.City = new string('C', 31);

            var result = validator.Validate(request.SchoolDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.ErrorMessage == ResourceMessagesException.SCHOOL_CITY_TOOLONG);
        }
    }
}