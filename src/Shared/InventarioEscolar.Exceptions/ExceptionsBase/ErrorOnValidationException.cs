using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException(IList<string> errorMessages) : InventarioEscolarException(string.Join(" | ", errorMessages))
    {
        public override IList<string> GetErrorMessages() => errorMessages;
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}