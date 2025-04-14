using HotelReservationSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed Admin User
            if (await userManager.FindByEmailAsync("admin@hotel.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@hotel.com",
                    Email = "admin@hotel.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Hotels
            if (!await context.Hotels.AnyAsync())
            {
                var hotels = new List<Hotel>
                {
                    new Hotel
                    {
                        Name = "Grand Hotel",
                        Address = "123 Main Street",
                        Description = "A luxurious hotel in the heart of the city",
                        City = "New York",
                        Country = "USA",
                        StarRating = 5,
                        ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?q=80&w=1000&auto=format&fit=crop"
                    },
                    new Hotel
                    {
                        Name = "Seaside Resort",
                        Address = "456 Beach Avenue",
                        Description = "Beautiful beachfront property with ocean views",
                        City = "Miami",
                        Country = "USA",
                        StarRating = 4,
                        ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?q=80&w=1000&auto=format&fit=crop"
                    },
                    new Hotel
                    {
                        Name = "Mountain Lodge",
                        Address = "789 Pine Road",
                        Description = "Cozy mountain retreat with stunning views",
                        City = "Denver",
                        Country = "USA",
                        StarRating = 3,
                        ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?q=80&w=1000&auto=format&fit=crop"
                    }
                };

                await context.Hotels.AddRangeAsync(hotels);
                await context.SaveChangesAsync();

                // Seed Rooms for each hotel
                if (!await context.Rooms.AnyAsync())
                {
                    // Get the hotels we just created
                    var savedHotels = await context.Hotels.ToListAsync();

                    foreach (var hotel in savedHotels)
                    {
                        var rooms = new List<Room>();

                        // Add different room types for each hotel
                        rooms.Add(new Room
                        {
                            RoomNumber = $"{hotel.Id}01",
                            Type = RoomType.Single,
                            PricePerNight = 100.00m + (hotel.StarRating * 20),
                            IsAvailable = true,
                            Description = "Single room with a comfortable bed and basic amenities",
                            Capacity = 1,
                            HotelId = hotel.Id
                        });

                        rooms.Add(new Room
                        {
                            RoomNumber = $"{hotel.Id}02",
                            Type = RoomType.Double,
                            PricePerNight = 150.00m + (hotel.StarRating * 30),
                            IsAvailable = true,
                            Description = "Double room with two beds and a view",
                            Capacity = 2,
                            HotelId = hotel.Id
                        });

                        rooms.Add(new Room
                        {
                            RoomNumber = $"{hotel.Id}03",
                            Type = RoomType.Suite,
                            PricePerNight = 250.00m + (hotel.StarRating * 50),
                            IsAvailable = true,
                            Description = "Luxury suite with a king-size bed and living area",
                            Capacity = 2,
                            HotelId = hotel.Id
                        });

                        rooms.Add(new Room
                        {
                            RoomNumber = $"{hotel.Id}04",
                            Type = RoomType.Deluxe,
                            PricePerNight = 300.00m + (hotel.StarRating * 60),
                            IsAvailable = true,
                            Description = "Deluxe room with premium amenities and the best views",
                            Capacity = 4,
                            HotelId = hotel.Id
                        });

                        await context.Rooms.AddRangeAsync(rooms);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}