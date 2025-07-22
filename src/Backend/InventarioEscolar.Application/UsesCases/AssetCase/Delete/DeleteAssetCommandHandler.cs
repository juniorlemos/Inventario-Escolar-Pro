using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Delete
{
   public class DeleteAssetCommandHandler : IRequestHandler<DeleteAssetCommand, Unit>
    {
        private readonly IAssetDeleteOnlyRepository _assetDeleteOnlyRepository;
        private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteAssetCommandHandler(
            IAssetDeleteOnlyRepository assetDeleteOnlyRepository,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _assetDeleteOnlyRepository = assetDeleteOnlyRepository;
            _assetReadOnlyRepository = assetReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetReadOnlyRepository.GetById(request.AssetId);

            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            if (asset.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            await _assetDeleteOnlyRepository.Delete(asset.Id);
            await _unitOfWork.Commit();
            return Unit.Value;
        }
    }
}