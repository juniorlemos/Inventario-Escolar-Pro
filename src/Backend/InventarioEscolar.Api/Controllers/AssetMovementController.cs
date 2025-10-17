using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    [Authorize]
    public class AssetMovementController(IMediator mediator) : InventarioApiBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseAssetMovementJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0,
           [FromQuery] bool? isCanceled = null,
           [FromQuery] string? searchTerm = null)
        {
            var result = await mediator.Send(new GetAllAssetMovementsQuery(page, pageSize, searchTerm = string.Empty, isCanceled));

            var response = result.Adapt<ResponsePagedJson<ResponseAssetMovementJson>>();

            if (response.Items.Count != 0)
                return Ok(response);

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseAssetMovementJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
               [FromRoute] long id)
        {

            var result = await mediator.Send(new GetByIdAssetMovementQuery(id));

            var response = result.Adapt<ResponseAssetMovementJson>();
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
         [FromRoute] long id,
         [FromQuery] string? cancelReason)
        {
            await mediator.Send(new UpdateAssetMovementCommand(id, cancelReason = string.Empty));

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
         [FromBody] RequestRegisterAssetMovementJson request)
        {
            var assetMovementDto = request.Adapt<AssetMovementDto>();

             await mediator.Send(new RegisterAssetMovementCommand(assetMovementDto));

            return Created();
        }
    }
}
