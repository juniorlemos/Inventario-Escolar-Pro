using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

        public GetCategoryByIdQueryHandler(ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryReadOnlyRepository.GetById(request.CategoryId);

            if (category is null)
                throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND); 

            return category.Adapt<CategoryDto>();
        }
    }
}
