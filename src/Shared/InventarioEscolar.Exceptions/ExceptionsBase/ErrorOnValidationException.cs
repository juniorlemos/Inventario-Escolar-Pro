using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : InventarioEscolarException
    {
        private readonly IList<string> _errorMessages;

        public ErrorOnValidationException(IList<string> errorMessages)
            : base(string.Join(" | ", errorMessages)) 
        {
            _errorMessages = errorMessages;
        }

        public override IList<string> GetErrorMessages() => _errorMessages;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}