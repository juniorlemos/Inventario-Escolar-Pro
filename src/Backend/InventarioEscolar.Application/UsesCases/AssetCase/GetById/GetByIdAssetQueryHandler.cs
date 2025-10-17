using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public class GetByIdAssetQueryHandler(IAssetReadOnlyRepository assetReadOnlyRepository) : IRequestHandler<GetByIdAssetQuery, AssetDto>
    {
        public async Task<AssetDto> Handle(GetByIdAssetQuery request, CancellationToken cancellationToken)
        {
            var asset = await assetReadOnlyRepository.GetById(request.AssetId);

            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            return AssetMapper.ToDto(asset);
        }
    }
}
