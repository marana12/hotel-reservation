using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Core.Entities;

public class Room
{
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string RoomNumber { get; set; }

    [Required]
    public RoomType Type { get; set; }

    [Required]
    public decimal PricePerNight { get; set; }

    [Required]
    public int Capacity { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsAvailable { get; set; }

    [Required]
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
}

public enum RoomType
{
    Single,
    Double,
    Suite,
    Deluxe
}