using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Register
{
    public class RegisterAssetCommandHandler : IRequestHandler<RegisterAssetCommand, AssetDto>
    {
        private readonly IAssetWriteOnlyRepository _assetWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AssetDto> _validator;
        private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;
        private readonly ICurrentUserService _currentUser;

        public RegisterAssetCommandHandler(
            IAssetWriteOnlyRepository assetWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IValidator<AssetDto> validator,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            ICurrentUserService currentUser)
        {
            _assetWriteOnlyRepository = assetWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _assetReadOnlyRepository = assetReadOnlyRepository;
            _currentUser = currentUser;
        }

        public async Task<AssetDto> Handle(RegisterAssetCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowIfInvalid(request.AssetDto);

            if (!_currentUser.IsAuthenticated)
                           throw new BusinessException(ResourceMessagesException.USER_NOT_AUTHENTICATED);

            var assetAlreadyExists = await _assetReadOnlyRepository.ExistPatrimonyCode(request.AssetDto.PatrimonyCode, _currentUser.SchoolId);

            if (assetAlreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.PATRIMONY_CODE_ALREADY_EXISTS_);

            var asset = request.AssetDto.Adapt<Asset>();

            await _assetWriteOnlyRepository.Insert(asset);
            await _unitOfWork.Commit();

            return asset.Adapt<AssetDto>();
        }

    }
}

