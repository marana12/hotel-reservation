using HotelReservationSystem.Core.Entities;

namespace HotelReservationSystem.Core.Interfaces;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
}