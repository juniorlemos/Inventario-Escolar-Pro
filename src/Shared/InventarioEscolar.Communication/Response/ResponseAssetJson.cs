using InventarioEscolar.Communication.Enum;

namespace InventarioEscolar.Communication.Response
{
    public record ResponseAssetJson <T> 
    {
       public T Data {  get; set; }
        
        public ResponseAssetJson (T data) 
        {
            Data = data;
        }
    }
}
