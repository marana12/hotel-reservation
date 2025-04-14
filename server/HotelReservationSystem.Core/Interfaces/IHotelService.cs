using HotelReservationSystem.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservationSystem.Core.Interfaces;

public interface IHotelService
{
    Task<HotelListDto> GetAllHotelsAsync();
    Task<HotelDetailDto> GetHotelByIdAsync(int id);
    Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId);
    Task<HotelDto> CreateHotelAsync(HotelDto hotelDto);
    Task<HotelDto> UpdateHotelAsync(HotelDto hotelDto);
    Task<bool> DeleteHotelAsync(int id);
}