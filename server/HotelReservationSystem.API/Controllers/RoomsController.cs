using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Exceptions;
using HotelReservationSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomsController> _logger;

        public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all rooms");
                return StatusCode(500, "Internal server error while retrieving rooms");
            }
        }

        // GET: api/Rooms/hotel/5
        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRoomsByHotel(int hotelId)
        {
            try
            {
                var rooms = await _roomService.GetRoomsByHotelAsync(hotelId);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rooms by hotel: {HotelId}", hotelId);
                return StatusCode(500, "Internal server error while retrieving rooms by hotel");
            }
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoom(int id)
        {
            try
            {
                var room = await _roomService.GetRoomAsync(id);
                if (room == null)
                {
                    return NotFound();
                }
                return Ok(room);
            }
            catch (RoomNotFoundException ex)
            {
                _logger.LogWarning(ex, "Room not found: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room: {Id}", id);
                return StatusCode(500, "Internal server error while retrieving the room");
            }
        }

        // GET: api/Rooms/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAvailableRooms(
            [FromQuery] DateTime checkInDate,
            [FromQuery] DateTime checkOutDate,
            [FromQuery] int? hotelId = null)
        {
            try
            {
                var rooms = await _roomService.GetAvailableRoomsAsync(checkInDate, checkOutDate, hotelId);
                return Ok(rooms);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid parameters for available rooms search");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for available rooms");
                return StatusCode(500, "Internal server error while searching for available rooms");
            }
        }

        // POST: api/Rooms
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomDto roomDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdRoom = await _roomService.CreateRoomAsync(roomDto);
                return CreatedAtAction(nameof(GetRoom), new { id = createdRoom.Id }, createdRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room");
                return StatusCode(500, "Internal server error while creating the room");
            }
        }

        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto roomDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedRoom = await _roomService.UpdateRoomAsync(id, roomDto);
                if (updatedRoom == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (RoomNotFoundException ex)
            {
                _logger.LogWarning(ex, "Room not found for update: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room: {Id}", id);
                return StatusCode(500, "Internal server error while updating the room");
            }
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                await _roomService.DeleteRoomAsync(id);
                return NoContent();
            }
            catch (RoomNotFoundException ex)
            {
                _logger.LogWarning(ex, "Room not found for deletion: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room: {Id}", id);
                return StatusCode(500, "Internal server error while deleting the room");
            }
        }
    }
}