using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Exceptions;
using Shouldly;

namespace Validators.Test.Category.Register
{
    public class RegisterCategoryValidatorTest
    {
        [Fact]
        public void Validate_RegisterCategoryValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new RegisterCategoryValidator();
            var request = RegisterCategoryCommandBuilder.Build();

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_WhenNameIsEmpty_ShouldHaveValidationError()
        {
            var validator = new RegisterCategoryValidator();

            var request = RegisterCategoryCommandBuilder.Build();
            request.CategoryDto.Name = string.Empty;

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_WhenNameIsTooShort_ShouldHaveValidationError()
        {
            var validator = new RegisterCategoryValidator();

            var request = RegisterCategoryCommandBuilder.Build();
            request.CategoryDto.Name = "A"; 

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.CATEGORY_NAME_TOOSHORT);
        }

        [Fact]
        public void Validate_WhenNameIsTooLong_ShouldHaveValidationError()
        {
            var validator = new RegisterCategoryValidator();

            var request = RegisterCategoryCommandBuilder.Build();
            request.CategoryDto.Name = new string('A', 101); 

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.CATEGORY_NAME_TOOLONG);
        }

        [Fact]
        public void Validate_WhenDescriptionIsTooLong_ShouldHaveValidationError()
        {
            var validator = new RegisterCategoryValidator();

            var request = RegisterCategoryCommandBuilder.Build();
            request.CategoryDto.Description = new string('A', 201); 

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.CATEGORY_DESCRIPTION_TOOLONG);
        }
    }
}