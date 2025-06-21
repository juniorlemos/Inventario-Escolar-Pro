using InventarioEscolar.Application.UsesCases.SchoolCase.GetAll;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetById;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class SchoolController : InventarioApiBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseSchoolJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
          [FromServices] IRegisterSchoolUseCase useCase,
          [FromBody] RequestRegisterSchoolJson request)
        {
            var schoolDto = request.Adapt<SchoolDto>();

            var result = await useCase.Execute(schoolDto);

            var response = result.Adapt<ResponseSchoolJson>();

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseSchoolJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdSchool(
              [FromServices] IGetByIdSchoolUseCase useCase,
              [FromRoute] long id)
        {
            var school = await useCase.Execute(id);

            var response = school.Adapt<ResponseSchoolJson>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<SchoolDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromServices] IGetAllSchoolUseCase useCase,
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0)
        {
            var result = await useCase.Execute(page, pageSize);

            var response = result.Adapt<ResponsePagedJson<SchoolDto>>();

            if (response.Items.Count != 0)
                return Ok(response);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
          [FromServices] IUpdateSchoolUseCase useCase,
          [FromRoute] int id,
          [FromBody] RequestUpdateSchoolJson request)
        {
            var schoolDto = request.Adapt<SchoolDto>();

            await useCase.Execute(id, schoolDto);

            return NoContent();
        }
    }
}
