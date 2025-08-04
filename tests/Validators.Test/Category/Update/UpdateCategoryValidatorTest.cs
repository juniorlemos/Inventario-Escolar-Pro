using CommonTestUtilities.Request;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Exceptions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validators.Test.Category.Update
{
    public class UpdateCategoryValidatorTest
    {
        [Fact]
        public void Validate_UpdateCategoryValidator_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new UpdateCategoryValidator();
            var request = UpdateCategoryCommandBuilder.Build();

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeTrue();
        }

        [Fact]
        public void Validate_UpdateCategoryValidator_WhenNameIsEmpty_ShouldReturnValidationError()
        {
            var validator = new UpdateCategoryValidator();
            var request = UpdateCategoryCommandBuilder.Build();
            request.CategoryDto.Name = string.Empty;

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Validate_UpdateCategoryValidator_WhenNameIsTooShort_ShouldReturnValidationError()
        {
            var validator = new UpdateCategoryValidator();
            var request = UpdateCategoryCommandBuilder.Build();
            request.CategoryDto.Name = "A";

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.CATEGORY_NAME_TOOSHORT);
        }

        [Fact]
        public void Validate_UpdateCategoryValidator_WhenNameIsTooLong_ShouldReturnValidationError()
        {
            var validator = new UpdateCategoryValidator();
            var request = UpdateCategoryCommandBuilder.Build();
            request.CategoryDto.Name = new string('A', 101);

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.CATEGORY_NAME_TOOLONG);
        }

        [Fact]
        public void Validate_UpdateCategoryValidator_WhenDescriptionIsTooLong_ShouldReturnValidationError()
        {
            var validator = new UpdateCategoryValidator();
            var request = UpdateCategoryCommandBuilder.Build();
            request.CategoryDto.Description = new string('D', 201);

            var result = validator.Validate(request.CategoryDto);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(x => x.ErrorMessage == ResourceMessagesException.CATEGORY_DESCRIPTION_TOOLONG);
        }

    }
}
