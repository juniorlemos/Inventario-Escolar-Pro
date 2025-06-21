using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetAll;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetById;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
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
        [ProducesResponseType(typeof(ResponseRoomLocationJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterRoomLocationUseCase useCase,
            [FromBody] RequestRegisterRoomLocationJson request)
        {
            var roomLocationDto = request.Adapt<RoomLocationDto>();

            var result = await useCase.Execute(roomLocationDto);

            var response = result.Adapt<ResponseRoomLocationJson>();

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRoomLocationJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdRoomLocation(
              [FromServices] IGetByIdRoomLocationUseCase useCase,
              [FromRoute] long id)
        {
            var roomLocation = await useCase.Execute(id);

            var response = roomLocation.Adapt<ResponseRoomLocationJson>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<RoomLocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromServices] IGetAllRoomLocationUseCase useCase,
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0)
        {
            var result = await useCase.Execute(page, pageSize);

            var response = result.Adapt<ResponsePagedJson<RoomLocationDto>>();

            if (response.Items.Count != 0)
                return Ok(response);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
        [FromServices] IUpdateRoomLocationUseCase useCase,
        [FromRoute] int id,
        [FromBody] RequestUpdateRoomLocationJson request)
        {
            var roomLocationDto = request.Adapt<RoomLocationDto>();

            await useCase.Execute(id, roomLocationDto);

            return NoContent();
        }
    }
}