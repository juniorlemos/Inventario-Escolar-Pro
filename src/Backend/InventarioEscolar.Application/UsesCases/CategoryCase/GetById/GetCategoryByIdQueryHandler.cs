using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetById
{
    public class GetCategoryByIdQueryHandler(ICategoryReadOnlyRepository categoryReadOnlyRepository) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await categoryReadOnlyRepository.GetById(request.CategoryId)
                ?? throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);
           
            return category.Adapt<CategoryDto>();
        }
    }
}
