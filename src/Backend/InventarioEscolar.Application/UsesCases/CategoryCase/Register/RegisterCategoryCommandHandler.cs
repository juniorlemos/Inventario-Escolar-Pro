using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Register
{
    public class RegisterCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    IValidator<CategoryDto> validator,
    ICategoryReadOnlyRepository categoryReadOnlyRepository,
    ICategoryWriteOnlyRepository categoryWriteOnlyRepository,
    ICurrentUserService currentUser) : IRequestHandler<RegisterCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(RegisterCategoryCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.CategoryDto);

            if (!currentUser.IsAuthenticated)
                throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            var exists = await categoryReadOnlyRepository
                .ExistCategoryName(request.CategoryDto.Name, currentUser.SchoolId);

            if (exists)
                throw new DuplicateEntityException(ResourceMessagesException.CATEGORY_NAME_ALREADY_EXISTS);

            var category = request.CategoryDto.Adapt<Category>();
            category.SchoolId = currentUser.SchoolId;

            await categoryWriteOnlyRepository.Insert(category);
            await unitOfWork.Commit();

            return category.Adapt<CategoryDto>();
        }
    }
}