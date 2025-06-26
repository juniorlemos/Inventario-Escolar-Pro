using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class AssetController : InventarioApiBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseAssetJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
          [FromServices] IGetAllAssetUseCase useCase,
          [FromQuery] int page = 0,
          [FromQuery] int pageSize = 0)
        {
            var result = await useCase.Execute(page, pageSize);

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
               [FromServices] IGetByIdAssetUseCase useCase,
               [FromRoute] long id)
        {
            var asset = await useCase.Execute(id);

            var response = asset.Adapt<ResponseAssetJson>();
            return Ok(response);
        }


        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
        [FromServices] IUpdateAssetUseCase useCase,
        [FromRoute] long id,
        [FromBody] RequestUpdateAssetJson request)
        {
            var assetDto = request.Adapt<UpdateAssetDto>();

            await useCase.Execute(id, assetDto);

            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(typeof(ResponseAssetJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
        [FromServices] IRegisterAssetUseCase useCase,
        [FromBody] RequestRegisterAssetJson request)
        {
            var assetDto = request.Adapt<AssetDto>();

            var result = await useCase.Execute(assetDto);

            var response = result.Adapt<ResponseAssetJson>();

            return Created(string.Empty, response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
        [FromServices] IDeleteAssetUseCase useCase,
        [FromRoute] long id)
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
