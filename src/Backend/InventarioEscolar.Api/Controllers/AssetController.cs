using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Application.UsesCases.AssetCase.GetById;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Application.UsesCases.AssetCase.Update;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.ValueObjects;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class AssetController : InventarioApiBaseController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAssets(
             [FromServices] IGetAllAssetUseCase useCase,
             [FromQuery] int page = InventarioEscolarRuleConstants.PAGE,
             [FromQuery] int pageSize = InventarioEscolarRuleConstants.PAGESIZE)
        {
            var response = await useCase.Execute(page, pageSize);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAssets(
             [FromServices] IGetByIdAssetUseCase useCase,
             [FromRoute] long id)
        {
            //var response = await useCase.Execute(id);

            //return Ok(response);
            return Ok();

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
        [FromServices] IUpdateAssetUseCase useCase,
        [FromBody] RequestUpdateAssetJson request)
        {
            await useCase.Execute(request);

            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterAssetJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
        [FromServices] IRegisterAssetUseCase useCase,
        [FromBody] RequestRegisterAssetJson request)
        {
            var assetDto = request.Adapt<AssetDto>();

            var result = await useCase.Execute(assetDto);

            var response = result.Adapt<ResponseRegisterAssetJson>();

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
