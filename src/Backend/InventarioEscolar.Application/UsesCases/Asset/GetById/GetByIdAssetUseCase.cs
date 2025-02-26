using AutoMapper;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Repositories.Asset;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.Asset.GetById
{
    public class GetByIdAssetUseCase : IGetByIdAssetUseCase
    {
        private readonly IAssetReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;

        public GetByIdAssetUseCase(IAssetReadOnlyRepository readOnlyRepository,
                                  IMapper maper)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = maper;
        }

        public async Task<ResponseAssetJson<AssetDto>> Execute(long id)
        {
            var asset = await _readOnlyRepository.GetById(id) ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);
            var assetDto = _mapper.Map<AssetDto>(asset);

            return new ResponseAssetJson<AssetDto>(assetDto);
        }
    }
}
