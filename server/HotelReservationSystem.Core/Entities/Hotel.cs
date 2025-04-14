using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Core.Entities;

public class Hotel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? Country { get; set; }

    public int StarRating { get; set; }

    [StringLength(200)]
    public string? ImageUrl { get; set; }

    // Relación con habitaciones
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}