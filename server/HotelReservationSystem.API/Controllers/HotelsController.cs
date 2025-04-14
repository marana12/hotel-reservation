using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace HotelReservationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<ActionResult<HotelListDto>> GetAllHotels()
    {
        var result = await _hotelService.GetAllHotelsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDetailDto>> GetHotelById(int id)
    {
        var result = await _hotelService.GetHotelByIdAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id}/rooms")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetRoomsByHotelId(int id)
    {
        var result = await _hotelService.GetRoomsByHotelIdAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // Solo los administradores pueden crear, actualizar o eliminar hoteles
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<HotelDto>> CreateHotel(HotelDto hotelDto)
    {
        var result = await _hotelService.CreateHotelAsync(hotelDto);
        return CreatedAtAction(nameof(GetHotelById), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHotel(int id, HotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest();
        }

        var result = await _hotelService.UpdateHotelAsync(hotelDto);

        if (result == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var result = await _hotelService.DeleteHotelAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}