using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Exceptions;
using HotelReservationSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(
        IReservationService reservationService,
        ILogger<ReservationController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all reservations");
            return StatusCode(500, new { message = "An error occurred while retrieving reservations" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var reservation = await _reservationService.GetReservationAsync(id);
            return Ok(reservation);
        }
        catch (ReservationNotFoundException)
        {
            return NotFound(new { message = $"Reservation with ID {id} not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting reservation with ID {id}");
            return StatusCode(500, new { message = "An error occurred while retrieving the reservation" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
    {
        try
        {
            var reservation = await _reservationService.CreateReservationAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        catch (InvalidReservationDatesException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (RoomNotAvailableException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation");
            return StatusCode(500, new { message = "An error occurred while creating the reservation" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationDto dto)
    {
        try
        {
            var reservation = await _reservationService.UpdateReservationAsync(id, dto);
            return Ok(reservation);
        }
        catch (ReservationNotFoundException)
        {
            return NotFound(new { message = $"Reservation with ID {id} not found" });
        }
        catch (InvalidReservationDatesException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (RoomNotAvailableException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating reservation with ID {id}");
            return StatusCode(500, new { message = "An error occurred while updating the reservation" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _reservationService.DeleteReservationAsync(id);
            return NoContent();
        }
        catch (ReservationNotFoundException)
        {
            return NotFound(new { message = $"Reservation with ID {id} not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting reservation with ID {id}");
            return StatusCode(500, new { message = "An error occurred while deleting the reservation" });
        }
    }
}