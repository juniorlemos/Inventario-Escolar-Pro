using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Register
{
    public class RegisterCategoryUseCase(
       IUnitOfWork unitOfWork,
       IValidator<CategoryDto> validator,
       ICategoryReadOnlyRepository categoryReadOnlyRepository,
       ICategoryWriteOnlyRepository categoryWriteOnlyRepository) : IRegisterCategoryUseCase
    {
        public async Task<CategoryDto> Execute(CategoryDto categoryDto)
        {
            await Validate(categoryDto);

            var categoryAlreadyExists = await categoryReadOnlyRepository.ExistCategoryName(categoryDto.Name);

            if (categoryAlreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.CATEGORY_NAME_ALREADY_EXISTS);

            var category = categoryDto.Adapt<Category>();

            await categoryWriteOnlyRepository.Insert(category);
            await unitOfWork.Commit();

            return category.Adapt<CategoryDto>();

        }
        private async Task Validate(CategoryDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}

