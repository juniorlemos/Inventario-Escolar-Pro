using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
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
        [ProducesResponseType(typeof(ResponseRegisterSchoolJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
          [FromServices] IRegisterSchoolUseCase useCase,
          [FromBody] RequestRegisterSchoolJson request)
        {
            var schoolDto = request.Adapt<SchoolDto>();

            var result = await useCase.Execute(schoolDto);

            var response = result.Adapt<ResponseRegisterSchoolJson>();

            return Created(string.Empty, response);
        }
    }
}
