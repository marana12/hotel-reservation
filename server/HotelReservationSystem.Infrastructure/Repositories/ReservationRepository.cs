using HotelReservationSystem.Core.Entities;
using HotelReservationSystem.Core.Exceptions;
using HotelReservationSystem.Core.Interfaces;
using HotelReservationSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelReservationSystem.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation> GetByIdAsync(int id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.Guest)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation == null)
        {
            throw new ReservationNotFoundException(id);
        }

        return reservation;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.Guest)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> FindAsync(Expression<Func<Reservation, bool>> predicate)
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.Guest)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task AddAsync(Reservation entity)
    {
        await _context.Reservations.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Reservation entity)
    {
        _context.Reservations.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Reservation entity)
    {
        _context.Reservations.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<Reservation, bool>> predicate)
    {
        return await _context.Reservations.AnyAsync(predicate);
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        return !await _context.Reservations
            .AnyAsync(r => r.RoomId == roomId &&
                          r.Status != ReservationStatus.Cancelled &&
                          ((checkInDate >= r.CheckInDate && checkInDate < r.CheckOutDate) ||
                           (checkOutDate > r.CheckInDate && checkOutDate <= r.CheckOutDate) ||
                           (checkInDate <= r.CheckInDate && checkOutDate >= r.CheckOutDate)));
    }
}