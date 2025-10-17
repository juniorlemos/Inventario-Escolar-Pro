using InventarioEscolar.Application.UsesCases.CategoryCase.Delete;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetAll;
using InventarioEscolar.Application.UsesCases.CategoryCase.GetById;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Application.UsesCases.CategoryCase.Update;
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
    public class CategoryController(IMediator mediator) : InventarioApiBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
             [FromBody] RequestRegisterCategoryJson request)
        {
            var categoryDto = request.Adapt<CategoryDto>();

            var result = await mediator.Send(new RegisterCategoryCommand(categoryDto));

            var response = result.Adapt<ResponseCategoryJson>();

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
                   [FromRoute] long id)
        {
            var category = await mediator.Send(new GetCategoryByIdQuery(id));

            var response = category.Adapt<ResponseCategoryJson>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseCategoryJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
          [FromQuery] int page = 0,
          [FromQuery] int pageSize = 0,
          [FromQuery] string? searchTerm = null)
        {
            var result = await mediator.Send(new GetAllCategoriesQuery(page, pageSize, searchTerm));

            var response = result.Adapt<ResponsePagedJson<ResponseCategoryJson>>();

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
         [FromBody] RequestUpdateCategoryJson request)
        {
            var categoryDto = request.Adapt<UpdateCategoryDto>();

            await mediator.Send(new UpdateCategoryCommand(id, categoryDto));

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
        [FromRoute] long id)
        {
            await mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}