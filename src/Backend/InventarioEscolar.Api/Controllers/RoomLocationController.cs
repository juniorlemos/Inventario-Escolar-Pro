using InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Update;
using InventarioEscolar.Application.UsesCases.SchoolCase.Delete;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class RoomLocationController : InventarioApiBaseController
    {
        private readonly IMediator _mediator;

        public RoomLocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRoomLocationJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromBody] RequestRegisterRoomLocationJson request)
        {
            var roomLocationDto = request.Adapt<RoomLocationDto>();

            var result = await _mediator.Send(new RegisterRoomLocationCommand(roomLocationDto));

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
            var roomLocation = await _mediator.Send(new GetRoomLocationByIdQuery(id));

            var response = roomLocation.Adapt<ResponseRoomLocationJson>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponsePagedJson<ResponseRoomLocationJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 0)
        {
            var result = await _mediator.Send(new GetAllRoomLocationsQuery(page, pageSize));

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

            await _mediator.Send(new UpdateRoomLocationCommand(id, roomLocationDto));

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
        [FromRoute] long id)
        {
            await _mediator.Send(new DeleteRoomLocationCommand(id));

            return NoContent();
        }
    }
}