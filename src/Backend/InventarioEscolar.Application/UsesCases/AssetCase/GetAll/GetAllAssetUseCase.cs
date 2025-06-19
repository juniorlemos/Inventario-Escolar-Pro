
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Repositories.Assets;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetAll
{
    public class GetAllAssetUseCase : IGetAllAssetUseCase
    {
        private readonly IAssetReadOnlyRepository _readOnlyRepository;

        public GetAllAssetUseCase(IAssetReadOnlyRepository readOnlyRepository
                                  )
        {
            _readOnlyRepository = readOnlyRepository;
        }
        public async Task<PagedResult<AssetDto>> Execute(int page, int pageSize)
        {
            var pagedResultAssets = await _readOnlyRepository.GetAllAssets( page,pageSize);

            var itemsDto = new List<AssetDto> { };

            return new PagedResult<AssetDto>(itemsDto, pagedResultAssets.TotalCount, page, pageSize);
        }
    }
}
