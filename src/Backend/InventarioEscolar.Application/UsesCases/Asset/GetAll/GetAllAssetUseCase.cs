
using AutoMapper;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Repositories.Asset;

namespace InventarioEscolar.Application.UsesCases.Asset.GetAll
{
    public class GetAllAssetUseCase : IGetAllAssetUseCase
    {
        private readonly IAssetReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public GetAllAssetUseCase(IAssetReadOnlyRepository readOnlyRepository,
                                  IMapper maper)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = maper;
        }
        public async Task<ResponseAssetJson<IEnumerable<AssetDto>>> Execute()
        {
            var assets = await _readOnlyRepository.GetAllAssets();
            var assetsDto =_mapper.Map<IEnumerable<AssetDto>>(assets);

            return new ResponseAssetJson<IEnumerable<AssetDto>>(assetsDto);
        }
    }
}
