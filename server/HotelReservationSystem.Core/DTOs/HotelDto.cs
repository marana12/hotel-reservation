using System.Collections.Generic;

namespace HotelReservationSystem.Core.DTOs;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Description { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public int StarRating { get; set; }
    public string? ImageUrl { get; set; }
}

public class HotelDetailDto : HotelDto
{
    public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();
}

public class HotelListDto
{
    public IEnumerable<HotelDto> Hotels { get; set; } = new List<HotelDto>();
}