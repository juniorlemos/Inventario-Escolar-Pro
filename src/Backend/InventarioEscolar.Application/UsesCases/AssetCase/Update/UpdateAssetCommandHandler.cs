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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public class UpdateAssetCommandHandler : IRequestHandler<UpdateAssetCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateAssetDto> _validator;
        private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;
        private readonly IAssetUpdateOnlyRepository _assetUpdateOnlyRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateAssetCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<UpdateAssetDto> validator,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IAssetUpdateOnlyRepository assetUpdateOnlyRepository,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _assetReadOnlyRepository = assetReadOnlyRepository;
            _assetUpdateOnlyRepository = assetUpdateOnlyRepository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowIfInvalid(request.AssetDto);

            var asset = await _assetReadOnlyRepository.GetById(request.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            if (asset.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            request.AssetDto.Adapt(asset);

            _assetUpdateOnlyRepository.Update(asset);
            await _unitOfWork.Commit();

            return Unit.Value;
        }

    }
}
