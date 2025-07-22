using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase
{
    public record GenerateAssetConservationStateReportQuery() : IRequest<byte[]>;
}
