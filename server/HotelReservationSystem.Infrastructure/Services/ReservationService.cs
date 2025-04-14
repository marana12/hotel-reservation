using AutoMapper;
using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Entities;
using HotelReservationSystem.Core.Exceptions;
using HotelReservationSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace HotelReservationSystem.Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        IReservationRepository reservationRepository,
        IMapper mapper,
        ILogger<ReservationService> logger)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReservationDto> GetReservationAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        return _mapper.Map<ReservationDto>(reservation);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
    {
        var reservations = await _reservationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto)
    {
        try
        {
            if (dto.CheckInDate >= dto.CheckOutDate)
            {
                throw new InvalidReservationDatesException("Check-in date must be before check-out date");
            }

            var isRoomAvailable = await _reservationRepository.IsRoomAvailableAsync(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            if (!isRoomAvailable)
            {
                throw new RoomNotAvailableException(dto.RoomId, dto.CheckInDate, dto.CheckOutDate);
            }

            var reservation = _mapper.Map<Reservation>(dto);
            reservation.Status = ReservationStatus.Pending;
            reservation.CreatedAt = DateTime.UtcNow;

            await _reservationRepository.AddAsync(reservation);

            return await GetReservationAsync(reservation.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation");
            throw;
        }
    }

    public async Task<ReservationDto> UpdateReservationAsync(int id, UpdateReservationDto dto)
    {
        try
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (dto.CheckInDate.HasValue && dto.CheckOutDate.HasValue)
            {
                if (dto.CheckInDate.Value >= dto.CheckOutDate.Value)
                {
                    throw new InvalidReservationDatesException("Check-in date must be before check-out date");
                }

                var isRoomAvailable = await _reservationRepository.IsRoomAvailableAsync(
                    dto.RoomId ?? reservation.RoomId,
                    dto.CheckInDate.Value,
                    dto.CheckOutDate.Value);

                if (!isRoomAvailable)
                {
                    throw new RoomNotAvailableException(
                        dto.RoomId ?? reservation.RoomId,
                        dto.CheckInDate.Value,
                        dto.CheckOutDate.Value);
                }
            }

            _mapper.Map(dto, reservation);
            reservation.UpdatedAt = DateTime.UtcNow;

            await _reservationRepository.UpdateAsync(reservation);

            return await GetReservationAsync(reservation.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating reservation");
            throw;
        }
    }

    public async Task DeleteReservationAsync(int id)
    {
        try
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            await _reservationRepository.DeleteAsync(reservation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting reservation");
            throw;
        }
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        return await _reservationRepository.IsRoomAvailableAsync(roomId, checkInDate, checkOutDate);
    }
}