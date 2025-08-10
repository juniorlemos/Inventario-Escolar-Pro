using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public class DuplicateEntityException(string message) : InventarioEscolarException(message)
    {
        public override IList<string> GetErrorMessages() => [Message];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}