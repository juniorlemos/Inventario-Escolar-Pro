using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetAll
{
    public class GetAllCategoriesQueryHandler(ICategoryReadOnlyRepository categoryReadOnlyRepository) : IRequestHandler<GetAllCategoriesQuery, PagedResult<CategoryDto>>
    {
        public async Task<PagedResult<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var pagedCategories = await categoryReadOnlyRepository.GetAll(request.Page, request.PageSize,request.SearchTerm)
                                     ?? PagedResult<Category>.Empty(request.Page, request.PageSize, request.SearchTerm);

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