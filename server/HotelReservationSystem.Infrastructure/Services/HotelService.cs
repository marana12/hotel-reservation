using AutoMapper;
using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Entities;
using HotelReservationSystem.Core.Interfaces;
using HotelReservationSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservationSystem.Infrastructure.Services;

public class HotelService : IHotelService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public HotelService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<HotelListDto> GetAllHotelsAsync()
    {
        var hotels = await _context.Hotels.ToListAsync();
        return new HotelListDto
        {
            Hotels = _mapper.Map<IEnumerable<HotelDto>>(hotels)
        };
    }

    public async Task<HotelDetailDto> GetHotelByIdAsync(int id)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            return null;
        }

        return _mapper.Map<HotelDetailDto>(hotel);
    }

    public async Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId)
    {
        var hotel = await _context.Hotels
            .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == hotelId);

        if (hotel == null)
        {
            return null;
        }

        var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(hotel.Rooms);

        // Asegurar que todos los RoomDto tengan el nombre del hotel
        foreach (var roomDto in roomDtos)
        {
            roomDto.HotelId = hotelId;
            roomDto.HotelName = hotel.Name;
        }

        return roomDtos;
    }

    public async Task<HotelDto> CreateHotelAsync(HotelDto hotelDto)
    {
        var hotel = _mapper.Map<Hotel>(hotelDto);
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        return _mapper.Map<HotelDto>(hotel);
    }

    public async Task<HotelDto> UpdateHotelAsync(HotelDto hotelDto)
    {
        var hotel = await _context.Hotels.FindAsync(hotelDto.Id);

        if (hotel == null)
        {
            return null;
        }

        _mapper.Map(hotelDto, hotel);
        _context.Hotels.Update(hotel);
        await _context.SaveChangesAsync();

        return _mapper.Map<HotelDto>(hotel);
    }

    public async Task<bool> DeleteHotelAsync(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel == null)
        {
            return false;
        }

        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();

        return true;
    }
}