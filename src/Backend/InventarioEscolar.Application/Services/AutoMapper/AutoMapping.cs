using AutoMapper;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            RequestToDomain();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterAssetJson, Asset>();
        }
    }
}
