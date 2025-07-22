using InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class AssetMovementController : InventarioApiBaseController
    {
        private readonly IMediator _mediator;

        public AssetMovementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseAssetMovementJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0,
           [FromQuery] bool? isCanceled = null)
        {
            var result = await _mediator.Send(new GetAllAssetMovementsQuery(page, pageSize, isCanceled));

            var response = result.Adapt<ResponsePagedJson<ResponseAssetMovementJson>>();

            if (response.Items.Count != 0)
                return Ok(response);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
         [FromRoute] long id,
         [FromQuery] string cancelReason)
        {
            await _mediator.Send(new UpdateAssetMovementCommand(id, cancelReason));

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseAssetMovementJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
         [FromBody] RequestRegisterAssetMovementJson request)
        {
            var assetMovementDto = request.Adapt<AssetMovementDto>();

            var result = await _mediator.Send(new RegisterAssetMovementCommand(assetMovementDto));

            var response = result.Adapt<ResponseAssetMovementJson>();

            return Created(string.Empty, response);
        }
    }
}
