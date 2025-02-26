using InventarioEscolar.Communication.Enum;

namespace InventarioEscolar.Communication.Response
{
    public class ResponseAssetJson <T> 
    {
       public T Data {  get; set; }
        
        public ResponseAssetJson (T data) 
        {
            Data = data;
        }
    }
}
