using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Delete
{
   public class DeleteAssetCommandHandler(
       IAssetDeleteOnlyRepository assetDeleteOnlyRepository,
       IAssetReadOnlyRepository assetReadOnlyRepository,
       IUnitOfWork unitOfWork,
       ICurrentUserService currentUser) : IRequestHandler<DeleteAssetCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await assetReadOnlyRepository.GetById(request.AssetId)
                ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);
           
            if (asset.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            await assetDeleteOnlyRepository.Delete(asset.Id);
            await unitOfWork.Commit();
            return Unit.Value;
        }
    }
}