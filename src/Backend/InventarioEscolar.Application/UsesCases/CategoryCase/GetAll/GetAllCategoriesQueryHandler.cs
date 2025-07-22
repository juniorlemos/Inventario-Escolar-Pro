using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetAll
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PagedResult<CategoryDto>>
    {
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

        public GetAllCategoriesQueryHandler(ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
        }

        public async Task<PagedResult<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var pagedCategories = await _categoryReadOnlyRepository.GetAll(request.Page, request.PageSize)
                                     ?? PagedResult<Category>.Empty(request.Page, request.PageSize);

            var dtoItems = pagedCategories.Items.Adapt<List<CategoryDto>>();

            return new PagedResult<CategoryDto>(
                dtoItems,
                pagedCategories.TotalCount,
                pagedCategories.Page,
                pagedCategories.PageSize
            );
        }
    }
}
