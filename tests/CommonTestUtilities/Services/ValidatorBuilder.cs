using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace CommonTestUtilities.Services
{
    public class ValidatorBuilder<T>
    {
        private readonly IValidator<T> _validator;

        public ValidatorBuilder()
        {
            _validator = Substitute.For<IValidator<T>>();
        }

        public ValidatorBuilder<T> WithValidResult()
        {
            _validator.ValidateAsync(Arg.Any<T>(), Arg.Any<CancellationToken>())
                      .Returns(new ValidationResult());

            return this;
        }

        public ValidatorBuilder<T> WithInvalidResult(params string[] errorMessages)
        {
            var failures = errorMessages
                .Select(msg => new ValidationFailure("", msg))
                .ToList();

            var validationResult = new ValidationResult(failures);

            _validator.ValidateAsync(Arg.Any<T>(), Arg.Any<CancellationToken>())
                      .Returns(validationResult);

            return this;
        }

        public IValidator<T> Build() => _validator;
    }
}