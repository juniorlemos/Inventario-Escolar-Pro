using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Update
{
    public record UpdateAssetMovementCommand(long Id, string CancelReason) : IRequest<Unit>;
}
