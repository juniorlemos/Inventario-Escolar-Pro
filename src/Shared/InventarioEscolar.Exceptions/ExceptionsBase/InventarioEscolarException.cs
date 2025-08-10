using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public abstract class InventarioEscolarException(string message) : SystemException(message)
    {
        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}