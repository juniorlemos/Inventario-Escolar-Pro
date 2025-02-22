using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public class NotFoundException : InventarioEscolarException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
