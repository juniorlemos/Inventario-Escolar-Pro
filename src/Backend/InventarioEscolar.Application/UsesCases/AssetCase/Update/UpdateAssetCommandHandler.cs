using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public class UpdateAssetCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<UpdateAssetDto> validator,
        IAssetReadOnlyRepository assetReadOnlyRepository,
        IAssetUpdateOnlyRepository assetUpdateOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<UpdateAssetCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.AssetDto);

            var asset = await assetReadOnlyRepository.GetById(request.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            if (asset.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            request.AssetDto.Adapt(asset);

            assetUpdateOnlyRepository.Update(asset);
            await unitOfWork.Commit();

            return Unit.Value;
        }
    }
}