
using InventarioEscolar.Domain.Repositories.Asset;

namespace InventarioEscolar.Application.UsesCases.Asset.GetAll
{
    public class GetAllAssetUseCase : IGetAllAssetUseCase
    {
        private readonly IAssetReadOnlyRepository _readOnlyRepository;

        public GetAllAssetUseCase(IAssetReadOnlyRepository readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository;
        }
        public async Task<IList<Domain.Entities.Asset>> Execute()
        {
            return await _readOnlyRepository.GetAllAssets();
        }
    }
}
