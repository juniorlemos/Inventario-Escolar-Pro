using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public class GetByIdAssetUseCase : IGetByIdAssetUseCase
    {
        private readonly IAssetReadOnlyRepository _readOnlyRepository;
        

        public GetByIdAssetUseCase(IAssetReadOnlyRepository readOnlyRepository
                                 )
        {
            _readOnlyRepository = readOnlyRepository;
           
        }

        public async Task Execute(long id)
        {
            var asset = await _readOnlyRepository.GetById(id) ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);
           

            return ;
        }
    }
}
