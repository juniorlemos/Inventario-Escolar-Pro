using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class RoomLocationController : InventarioApiBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterRoomLocationJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterRoomLocationUseCase useCase,
            [FromBody] RequestRegisterRoomLocationJson request)
        {
            var roomLocationDto = request.Adapt<RoomLocationDto>();

            var result = await useCase.Execute(roomLocationDto);

            var response = result.Adapt<ResponseRegisterRoomLocationJson>();

            return Created(string.Empty, response);
        }
    }
}
