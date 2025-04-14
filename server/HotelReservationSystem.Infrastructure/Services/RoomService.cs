using AutoMapper;
using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Entities;
using HotelReservationSystem.Core.Exceptions;
using HotelReservationSystem.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Infrastructure.Data;

namespace HotelReservationSystem.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomService> _logger;
        private readonly ApplicationDbContext _context;

        public RoomService(IRoomRepository roomRepository, IMapper mapper, ILogger<RoomService> logger, ApplicationDbContext context)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<RoomDto> GetRoomAsync(int id)
        {
            try
            {
                var room = await _context.Rooms
                    .Include(r => r.Hotel)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (room == null)
                {
                    _logger.LogWarning("Room with ID {RoomId} not found", id);
                    throw new RoomNotFoundException(id);
                }

            return _mapper.Map<RoomDto>(room);
            }
            catch (RoomNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving room with ID {RoomId}", id);
                throw new ApplicationException($"Error retrieving room with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            try
            {
                var rooms = await _context.Rooms
                    .Include(r => r.Hotel)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<RoomDto>>(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all rooms");
                throw new ApplicationException("Error retrieving all rooms", ex);
            }
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByHotelAsync(int hotelId)
        {
            try
            {
                var rooms = await _context.Rooms
                    .Include(r => r.Hotel)
                    .Where(r => r.HotelId == hotelId)
                    .ToListAsync();
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rooms for hotel {HotelId}", hotelId);
                throw new ApplicationException($"Error retrieving rooms for hotel {hotelId}", ex);
            }
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto)
        {
            try
        {
            var room = _mapper.Map<Room>(dto);
                await _roomRepository.AddAsync(room);
            return _mapper.Map<RoomDto>(room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating room");
                throw new ApplicationException("Error creating room", ex);
            }
        }

        public async Task<RoomDto> UpdateRoomAsync(int id, UpdateRoomDto dto)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                {
                    _logger.LogWarning("Room with ID {RoomId} not found for update", id);
                    throw new RoomNotFoundException(id);
                }

            _mapper.Map(dto, room);
                await _roomRepository.UpdateAsync(room);
            return _mapper.Map<RoomDto>(room);
            }
            catch (RoomNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room with ID {RoomId}", id);
                throw new ApplicationException($"Error updating room with ID {id}", ex);
            }
        }

        public async Task DeleteRoomAsync(int id)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(id);
                if (room == null)
                {
                    _logger.LogWarning("Room with ID {RoomId} not found for deletion", id);
                    throw new RoomNotFoundException(id);
                }

                await _roomRepository.DeleteAsync(room);
            }
            catch (RoomNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting room with ID {RoomId}", id);
                throw new ApplicationException($"Error deleting room with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate, int? hotelId = null)
        {
            try
            {
                if (checkInDate >= checkOutDate)
                {
                    _logger.LogWarning("Invalid date range: CheckIn {CheckIn} must be before CheckOut {CheckOut}", checkInDate, checkOutDate);
                    throw new ArgumentException("Check-in date must be before check-out date");
                }

                var query = _context.Rooms
                    .Include(r => r.Hotel)
                    .Where(r => r.IsAvailable);

                if (hotelId.HasValue)
                {
                    query = query.Where(r => r.HotelId == hotelId.Value);
                }

                // Excluir habitaciones que ya tienen reservaciones en el rango de fechas
                var unavailableRoomIds = await _context.Reservations
                    .Where(r => r.Status != ReservationStatus.Cancelled &&
                               ((r.CheckInDate <= checkInDate && r.CheckOutDate > checkInDate) ||
                                (r.CheckInDate < checkOutDate && r.CheckOutDate >= checkOutDate) ||
                                (r.CheckInDate >= checkInDate && r.CheckOutDate <= checkOutDate)))
                    .Select(r => r.RoomId)
                    .ToListAsync();

                var availableRooms = await query
                    .Where(r => !unavailableRoomIds.Contains(r.Id))
                .ToListAsync();

                return _mapper.Map<IEnumerable<RoomDto>>(availableRooms);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available rooms for date range: {CheckIn} to {CheckOut}", checkInDate, checkOutDate);
                throw new ApplicationException($"Error retrieving available rooms for the specified date range", ex);
            }
        }
    }
}