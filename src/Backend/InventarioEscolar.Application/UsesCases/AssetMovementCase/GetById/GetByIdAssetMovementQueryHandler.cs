using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetById
{
    public class GetByIdAssetMovementQueryHandler(IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository) : IRequestHandler<GetByIdAssetMovementQuery, AssetMovementDto>
    {
        public async Task<AssetMovementDto> Handle(GetByIdAssetMovementQuery request, CancellationToken cancellationToken)
        {
            var assetMovement = await assetMovementReadOnlyRepository.GetById(request.AssetMovementId);

            if (assetMovement is null)
                throw new NotFoundException(ResourceMessagesException.ASSETMOVEMENT_NOT_FOUND);

            return AssetMovementMapper.ToDto(assetMovement); 
        }
    }
}
