using InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
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
    public class RoomLocationController(IMediator mediator) : InventarioApiBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRoomLocationJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RequestRegisterRoomLocationJson request)
        {
            var roomLocationDto = request.Adapt<RoomLocationDto>();

            var result = await mediator.Send(new RegisterRoomLocationCommand(roomLocationDto));

            var response = result.Adapt<ResponseRoomLocationJson>();

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRoomLocationJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdRoomLocation(
              [FromRoute] long id)
        {
            var roomLocation = await mediator.Send(new GetRoomLocationByIdQuery(id));

            var response = roomLocation.Adapt<ResponseRoomLocationJson>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseRoomLocationJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0,
           [FromQuery] string? searchTerm = null)
        {
            var result = await mediator.Send(new GetAllRoomLocationsQuery(page, pageSize, searchTerm));

            var response = result.Adapt<ResponsePagedJson<ResponseRoomLocationJson>>();

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
        [FromBody] RequestUpdateRoomLocationJson request)
        {
            var roomLocationDto = request.Adapt<UpdateRoomLocationDto>();

            await mediator.Send(new UpdateRoomLocationCommand(id, roomLocationDto));

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
        [FromRoute] long id)
        {
            await mediator.Send(new DeleteRoomLocationCommand(id));

            return NoContent();
        }
    }
}