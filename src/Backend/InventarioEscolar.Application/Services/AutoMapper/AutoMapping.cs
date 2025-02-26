using AutoMapper;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            RequestToDomain();
            DomainToReponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterAssetJson, Asset>();
            CreateMap<RequestUpdateAssetJson, Asset>();
        }

        private void DomainToReponse()
        {
            CreateMap<Asset,AssetDto>();
        }
    }
}
