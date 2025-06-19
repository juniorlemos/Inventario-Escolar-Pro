
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Exceptions.ExceptionsBase;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Domain.Repositories.Assets;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Delete
{
    public class DeleteAssetUseCase : IDeleteAssetUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssetWriteOnlyRepository _repositoryWrite;
        private readonly IAssetReadOnlyRepository _repositoryRead;


        public DeleteAssetUseCase(
                         IUnitOfWork unitOfWork,
                         IAssetWriteOnlyRepository repositoryWrite,
                         IAssetReadOnlyRepository repositoryRead)
        { 
            _unitOfWork = unitOfWork;
            _repositoryWrite = repositoryWrite;
            _repositoryRead = repositoryRead;
        }
        public async Task Execute(long assetId)
        {
            var asset = await _repositoryRead.GetById(assetId);

            if (asset == null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            await _repositoryWrite.Delete(assetId);

            await _unitOfWork.Commit();
        }
    }
}
