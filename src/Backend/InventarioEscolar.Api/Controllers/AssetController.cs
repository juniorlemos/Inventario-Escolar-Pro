using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class AssetController : InventarioApiBaseController
    {
        private readonly IMediator _mediator;

        public AssetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseAssetJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
          [FromQuery] int page = 0,
          [FromQuery] int pageSize = 0)
        {           
            var result = await _mediator.Send(new GetAllAssetQuery(page, pageSize));
            var response = result.Adapt<ResponsePagedJson<ResponseAssetJson>>();

            if (response.Items.Count != 0)
                return Ok(response);

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseAssetJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsset(
               [FromRoute] long id)
        {

            var result = await _mediator.Send(new GetByIdAssetQuery(42));

            var response = result.Adapt<ResponseAssetJson>();
            return Ok(response);
        }


        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
        [FromRoute] long id,
        [FromBody] RequestUpdateAssetJson request)
        {
            var assetDto = request.Adapt<UpdateAssetDto>();

            await _mediator.Send(new UpdateAssetCommand(id, assetDto));

            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(typeof(ResponseAssetJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
        [FromBody] RequestRegisterAssetJson request)
        {
            var assetDto = request.Adapt<AssetDto>();

            var result = await _mediator.Send(new RegisterAssetCommand(assetDto));

            var response = result.Adapt<ResponseAssetJson>();

            return Created(string.Empty, response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
        [FromRoute] long id)
        {
            await _mediator.Send( new DeleteAssetCommand(id));

            return NoContent();
        }
    }
}
