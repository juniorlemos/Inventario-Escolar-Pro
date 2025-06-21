using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetAll;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetById;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetAll;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class CategoryController : InventarioApiBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
             [FromServices] IRegisterCategoryUseCase useCase,
             [FromBody] RequestRegisterCategoryJson request)
        {
            var categoryDto = request.Adapt<CategoryDto>();

            var result = await useCase.Execute(categoryDto);

            var response = result.Adapt<ResponseCategoryJson>();

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdCategory(
                   [FromServices] IGetByIdCategoryUseCase useCase,
                   [FromRoute] long id)
        {
            var category = await useCase.Execute(id);

            var response = category.Adapt<ResponseCategoryJson>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
          [FromServices] IGetAllCategoryUseCase useCase,
          [FromQuery] int page = 0,
          [FromQuery] int pageSize = 0)
        {
            var result = await useCase.Execute(page, pageSize);

            var response = result.Adapt<ResponsePagedJson<CategoryDto>>();

            if (response.Items.Count != 0)
                return Ok(response);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
         [FromServices] IUpdateCategoryUseCase useCase,
         [FromRoute] int id,
         [FromBody] RequestUpdateCategoryJson request)
        {
            var categoryDto = request.Adapt<CategoryDto>();

            await useCase.Execute(id, categoryDto);

            return NoContent();
        }
    }
}