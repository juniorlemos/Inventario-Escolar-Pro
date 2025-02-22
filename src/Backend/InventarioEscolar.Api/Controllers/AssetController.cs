using InventarioEscolar.Application.UsesCases.Asset.GetAll;
using InventarioEscolar.Application.UsesCases.Asset.Register;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAssets(
             [FromServices] IGetAllAssetUseCase useCase)
        {
            var response = await useCase.Execute();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRegisterAssetJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAssets(
             [FromServices] IGetAllAssetUseCase useCase,
             [FromRoute] long id)
        {
            var response = await useCase.Execute();

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterAssetJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
        [FromServices] IRegisterAssetUseCase useCase,
        [FromBody] RequestRegisterAssetJson request)
        {
            var result = await useCase.Execute(request);

            return Created(string.Empty, result);
        }


    }
}
