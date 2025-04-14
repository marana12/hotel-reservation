using System;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Core.Entities;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public int RoomId { get; set; }
    public Room Room { get; set; }

    [Required]
    public string GuestId { get; set; }
    public ApplicationUser Guest { get; set; }

    [Required]
    public ReservationStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}