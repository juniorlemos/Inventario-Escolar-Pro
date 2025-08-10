using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public class BusinessException(string message) : InventarioEscolarException(message)
    {
        public override IList<string> GetErrorMessages() => [Message];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}