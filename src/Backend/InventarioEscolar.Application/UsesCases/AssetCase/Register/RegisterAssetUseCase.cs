using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;


namespace InventarioEscolar.Application.UsesCases.AssetCase.Register
{
    public class RegisterAssetUseCase(
        IAssetWriteOnlyRepository assetWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<AssetDto> validator,
        IAssetReadOnlyRepository assetReadOnlyRepository) : IRegisterAssetUseCase
    {
        public async Task<AssetDto> Execute(AssetDto assetDto)
        {
            await Validate(assetDto);

            var assetAlreadyExists = await assetReadOnlyRepository.ExistPatrimonyCode(assetDto.PatrimonyCode);

            if (assetAlreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.PATRIMONY_CODE_ALREADY_EXISTS_);

            var asset = assetDto.Adapt<Asset>();

            await assetWriteOnlyRepository.Insert(asset);
            await unitOfWork.Commit();

            return new AssetDto
            {
                Name = assetDto.Name
            };
        }

        private async Task Validate(AssetDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
