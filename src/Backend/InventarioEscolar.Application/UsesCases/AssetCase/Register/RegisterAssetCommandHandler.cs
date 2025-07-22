using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Interfaces;
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
            await Validate(request.AssetDto);

            var schoolId = _currentUser.SchoolId
                          ?? throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            var assetAlreadyExists = await _assetReadOnlyRepository.ExistPatrimonyCode(request.AssetDto.PatrimonyCode, schoolId);

            if (assetAlreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.PATRIMONY_CODE_ALREADY_EXISTS_);

            var asset = request.AssetDto.Adapt<Asset>();

            await _assetWriteOnlyRepository.Insert(asset);
            await _unitOfWork.Commit();

            return asset.Adapt<AssetDto>();
        }

        private async Task Validate(AssetDto dto)
        {
            var result = await _validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}

