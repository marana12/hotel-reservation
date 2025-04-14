using HotelReservationSystem.Core.Entities;
using HotelReservationSystem.Core.Interfaces;
using HotelReservationSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Repositories;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate)
    {
        try
        {
            var availableRooms = await _dbSet
                .Where(r => r.IsAvailable)
                .ToListAsync();

            var roomsWithReservations = await _context.Reservations
                .Where(r =>
                    (checkInDate < r.CheckOutDate && checkOutDate > r.CheckInDate) &&
                    r.Status != ReservationStatus.Cancelled)
                .Select(r => r.RoomId)
                .Distinct()
                .ToListAsync();

            return availableRooms.Where(r => !roomsWithReservations.Contains(r.Id)).ToList();
        }
        catch (Exception ex)
        {
            // Log the error
            throw new ApplicationException("Error retrieving available rooms", ex);
        }
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        try
        {
            var room = await _dbSet.FindAsync(roomId);
            if (room == null || !room.IsAvailable)
                return false;

            var hasReservation = await _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == roomId &&
                    checkInDate < r.CheckOutDate &&
                    checkOutDate > r.CheckInDate &&
                    r.Status != ReservationStatus.Cancelled);

            return !hasReservation;
        }
        catch (Exception ex)
        {
            // Log the error
            throw new ApplicationException($"Error checking room availability for ID: {roomId}", ex);
        }
    }
}