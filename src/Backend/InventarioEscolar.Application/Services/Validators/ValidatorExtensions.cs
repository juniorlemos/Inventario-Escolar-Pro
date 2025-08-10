using FluentValidation;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.Services.Validators
{
    public static class ValidatorExtensions
    {
        public static async Task ValidateAndThrowIfInvalid<T>(this IValidator<T> validator, T instance)
        {
            var result = await validator.ValidateAsync(instance);
           
            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}