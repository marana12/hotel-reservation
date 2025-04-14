namespace HotelReservationSystem.Core.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; }
    public string Type { get; set; }
    public decimal PricePerNight { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; }
}

public class CreateRoomDto
{
    public string RoomNumber { get; set; }
    public string Type { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public string Description { get; set; }
    public int HotelId { get; set; }
}

public class UpdateRoomDto
{
    public string RoomNumber { get; set; }
    public string Type { get; set; }
    public decimal? PricePerNight { get; set; }
    public int? Capacity { get; set; }
    public string Description { get; set; }
    public bool? IsAvailable { get; set; }
    public int? HotelId { get; set; }
}