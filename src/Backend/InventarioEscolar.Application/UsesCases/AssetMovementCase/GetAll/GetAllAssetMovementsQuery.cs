using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public record GetAllAssetMovementsQuery(int Page, int PageSize, bool? IsCanceled = null) : IRequest<PagedResult<AssetMovementDto>>;
}
