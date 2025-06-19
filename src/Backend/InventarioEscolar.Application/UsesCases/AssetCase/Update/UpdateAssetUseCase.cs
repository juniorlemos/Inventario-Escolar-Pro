using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public class UpdateAssetUseCase : IUpdateAssetUseCase
    {
        private readonly IAssetUpdateOnlyRepository _repositoryUpdateOnly;
        private readonly IAssetReadOnlyRepository _repositoryReadOnly;
        private readonly IUnitOfWork _unitOfWork;
       

        public UpdateAssetUseCase(
               IAssetUpdateOnlyRepository repositoryUpdateOnly,
               IUnitOfWork unitOfWork,
             
               IAssetReadOnlyRepository repositoryReadOnly)
        {
            _repositoryUpdateOnly = repositoryUpdateOnly;
            _unitOfWork = unitOfWork;
           
            _repositoryReadOnly = repositoryReadOnly;
        }

        public async Task Execute(RequestUpdateAssetJson request)
        {
            var asset = await _repositoryReadOnly.GetById(request.Id);
            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            await Validate(request);

            var newAsset = request;

            //_repositoryUpdateOnly.Update(newAsset);

            await _unitOfWork.Commit();
        }

        private static async Task Validate(RequestUpdateAssetJson request)
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
