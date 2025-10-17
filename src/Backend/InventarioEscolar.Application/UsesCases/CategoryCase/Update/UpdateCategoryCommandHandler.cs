using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public class UpdateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<UpdateCategoryDto> validator,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ICategoryUpdateOnlyRepository categoryUpdateOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.CategoryDto);

            var category = await categoryReadOnlyRepository.GetById(request.Id);

            if (category is null)
                throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

            if (category.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);

            category.Name = request.CategoryDto.Name;
            category.Description = request.CategoryDto.Description;


            categoryUpdateOnlyRepository.Update(category);
            await unitOfWork.Commit();

            return Unit.Value;
        }
    }
}