using System;

namespace HotelReservationSystem.Core.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int RoomId { get; set; }
    public string GuestId { get; set; }
    public string GuestName { get; set; }
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
}

public class CreateReservationDto
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int RoomId { get; set; }
    public string GuestId { get; set; }
}

public class UpdateReservationDto
{
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public int? RoomId { get; set; }
    public string Status { get; set; }
}