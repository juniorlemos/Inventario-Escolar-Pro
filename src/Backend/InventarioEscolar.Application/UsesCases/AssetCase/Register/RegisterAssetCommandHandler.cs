using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Register
{
    public class RegisterAssetCommandHandler(
        IAssetWriteOnlyRepository assetWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<AssetDto> validator,
        IAssetReadOnlyRepository assetReadOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<RegisterAssetCommand, CreatedAssetResponse>
    {
        public async Task<CreatedAssetResponse> Handle(RegisterAssetCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.AssetDto);

            if (!currentUser.IsAuthenticated)
                           throw new BusinessException(ResourceMessagesException.USER_NOT_AUTHENTICATED);

            var assetAlreadyExists = await assetReadOnlyRepository.ExistPatrimonyCode(request.AssetDto.PatrimonyCode, currentUser.SchoolId);

            if (assetAlreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.PATRIMONY_CODE_ALREADY_EXISTS_);

            var asset = AssetMapper.ToEntity(request.AssetDto);
            asset.SchoolId = currentUser.SchoolId;

            await assetWriteOnlyRepository.Insert(asset);
            await unitOfWork.Commit();

            return new CreatedAssetResponse
            {
                Name = asset.Name,
                PatrimonyCode = asset.PatrimonyCode
            };
        }
    }
}