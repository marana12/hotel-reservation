using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Entities;

namespace HotelReservationSystem.Core.Interfaces;

public interface IReservationService
{
    Task<ReservationDto> GetReservationAsync(int id);
    Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto);
    Task<ReservationDto> UpdateReservationAsync(int id, UpdateReservationDto dto);
    Task DeleteReservationAsync(int id);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
}

public interface IRoomService
{
    Task<RoomDto> GetRoomAsync(int id);
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<IEnumerable<RoomDto>> GetRoomsByHotelAsync(int hotelId);
    Task<RoomDto> CreateRoomAsync(CreateRoomDto dto);
    Task<RoomDto> UpdateRoomAsync(int id, UpdateRoomDto dto);
    Task DeleteRoomAsync(int id);
    Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate, int? hotelId = null);
}

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
}