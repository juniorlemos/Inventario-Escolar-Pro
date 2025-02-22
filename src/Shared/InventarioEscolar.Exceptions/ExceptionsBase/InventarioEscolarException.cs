using System.Net;

namespace InventarioEscolar.Exceptions.ExceptionsBase
{
    public abstract class InventarioEscolarException :SystemException
    {
            protected InventarioEscolarException(string message) : base(message) { }

            public abstract IList<string> GetErrorMessages();
            public abstract HttpStatusCode GetStatusCode();
    }
}
