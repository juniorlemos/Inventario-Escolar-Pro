using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public class UpdateCategoryUseCase(
       IUnitOfWork unitOfWork,
       IValidator<UploadCategoryDto> validator,
       ICategoryReadOnlyRepository categoryReadOnlyRepository,
       ICategoryUpdateOnlyRepository categoryUpdateOnlyRepository) : IUpdateCategoryUseCase
    {
        public async Task Execute(long id, UploadCategoryDto categoryDto)
        {
            await Validate(categoryDto);

            var category = await categoryReadOnlyRepository.GetById(id);

            if (category is null)
                throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

            categoryDto.Adapt(category);

            categoryUpdateOnlyRepository.Update(category);
            await unitOfWork.Commit();

        }
        private async Task Validate(UploadCategoryDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}

