using InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Register;
using InventarioEscolar.Application.UsesCases.AssetMovementCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class AssetMovementController : InventarioApiBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseAssetMovementJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromServices] IGetAllAssetMovementUseCase useCase,
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0,
           [FromQuery] bool? isCanceled = null)
        {
            var result = await useCase.Execute(page, pageSize, isCanceled);

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
         [FromServices] IUpdateAssetMovementUseCase useCase,
         [FromRoute] long id,
         [FromQuery] string cancelReason)
        {
            await useCase.Execute(id, cancelReason);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseAssetMovementJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
         [FromServices] IRegisterAssetMovementUseCase useCase,
         [FromBody] RequestRegisterAssetMovementJson request)
        {
            var assetMovementDto = request.Adapt<AssetMovementDto>();

            var result = await useCase.Execute(assetMovementDto);

            var response = result.Adapt<ResponseAssetMovementJson>();

            return Created(string.Empty, response);
        }
    }
}
