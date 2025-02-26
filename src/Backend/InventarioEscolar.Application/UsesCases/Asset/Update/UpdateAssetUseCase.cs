using AutoMapper;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Asset;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.Asset.Update
{
    public class UpdateAssetUseCase : IUpdateAssetUseCase
    {
        private readonly IAssetUpdateOnlyRepository _repositoryUpdateOnly;
        private readonly IAssetReadOnlyRepository _repositoryReadOnly;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateAssetUseCase(
               IAssetUpdateOnlyRepository repositoryUpdateOnly,
               IUnitOfWork unitOfWork,
               IMapper mapper,
               IAssetReadOnlyRepository repositoryReadOnly)
        {
            _repositoryUpdateOnly = repositoryUpdateOnly;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repositoryReadOnly = repositoryReadOnly;
        }

        public async Task Execute(RequestUpdateAssetJson request)
        {
            var asset = await _repositoryReadOnly.GetById(request.Id);
            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            await Validate(request);

            var newAsset = _mapper.Map<Domain.Entities.Asset>(request);

            _repositoryUpdateOnly.Update(asset);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateAssetJson request)
        {
            var validator = new UpdateAssetValidator();

            var result = await validator.ValidateAsync(request);
            
            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
