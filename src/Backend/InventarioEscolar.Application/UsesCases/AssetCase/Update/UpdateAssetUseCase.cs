using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public class UpdateAssetUseCase(
       IUnitOfWork unitOfWork,
       IValidator<UpdateAssetDto> validator,
       IAssetReadOnlyRepository assetReadOnlyRepository,
       IAssetUpdateOnlyRepository assetUpdateOnlyRepository) : IUpdateAssetUseCase
    {
        public async Task Execute(long id, UpdateAssetDto assetDto)
        {
            await Validate(assetDto);

            var asset = await assetReadOnlyRepository.GetById(id);

            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            assetDto.Adapt(asset);

            assetUpdateOnlyRepository.Update(asset);
            await unitOfWork.Commit();
        }
        private async Task Validate(UpdateAssetDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
