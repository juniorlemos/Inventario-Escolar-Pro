using InventarioEscolar.Application.UsesCases.SchoolCase.Delete;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetAll;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetById;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class SchoolController : InventarioApiBaseController
    {
        private readonly IMediator _mediator;

        public SchoolController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ResponseSchoolJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
          [FromBody] RequestRegisterSchoolJson request)
        {
            var schoolDto = request.Adapt<SchoolDto>();

            var result = await _mediator.Send(new RegisterSchoolCommand(schoolDto));

            var response = result.Adapt<ResponseSchoolJson>();

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseSchoolJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdSchool(
              [FromRoute] long id)
        {
            var school = await _mediator.Send(new GetByIdSchoolQuery(id));

            var response = school.Adapt<ResponseSchoolJson>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseSchoolJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0)
        {
            var result = await _mediator.Send(new GetAllSchoolQuery(page, pageSize));

            var response = result.Adapt<ResponsePagedJson<ResponseSchoolJson>>();

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
          [FromBody] RequestUpdateSchoolJson request)
        {
            var updateSchoolDto = request.Adapt<UpdateSchoolDto>();

            await _mediator.Send(new UpdateSchoolCommand(id, updateSchoolDto));

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
        [FromRoute] long id)
        {
            await _mediator.Send(new DeleteSchoolCommand(id));

            return NoContent();
        }

    }
}
