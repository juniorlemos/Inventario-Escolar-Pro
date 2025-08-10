using CommonTestUtilities.Services;
using FluentValidation;

namespace CommonTestUtilities.Helpers
{
    public static class ValidatorTestHelper
    {
        public static IValidator<T> CreateValidator<T>(bool isValid, string errorMessage = null!)
        {
            return isValid
                ? new ValidatorBuilder<T>().WithValidResult().Build()
                : new ValidatorBuilder<T>().WithInvalidResult(errorMessage).Build();
        }
    }
}