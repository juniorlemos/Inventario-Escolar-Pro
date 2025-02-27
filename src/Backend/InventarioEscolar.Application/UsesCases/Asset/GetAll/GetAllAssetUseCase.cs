
using AutoMapper;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Pagination;
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
        public async Task<PagedResult<AssetDto>> Execute(int page, int pageSize)
        {
            var pagedResultAssets = await _readOnlyRepository.GetAllAssets( page,pageSize);

            var itemsDto = _mapper.Map<List<AssetDto>>(pagedResultAssets.Items);

            return new PagedResult<AssetDto>(itemsDto, pagedResultAssets.TotalCount, page, pageSize);
        }
    }
}
