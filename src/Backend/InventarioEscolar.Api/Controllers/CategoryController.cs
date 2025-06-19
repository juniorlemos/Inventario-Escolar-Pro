using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class CategoryController : InventarioApiBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterCategoryJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
             [FromServices] IRegisterCategoryUseCase useCase,
             [FromBody] RequestRegisterCategoryJson request)
        {
            var categoryDto = request.Adapt<CategoryDto>();

            var result = await useCase.Execute(categoryDto);

            var response = result.Adapt<ResponseRegisterCategoryJson>();

            return Created(string.Empty, response);
        }
    }
}