using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public class DuplicateEntityException : InventarioEscolarException
    {
        public DuplicateEntityException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
