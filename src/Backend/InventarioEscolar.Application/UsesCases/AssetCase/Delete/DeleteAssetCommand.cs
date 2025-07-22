using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Delete
{
    public record DeleteAssetCommand(long AssetId) : IRequest<Unit>;
}
