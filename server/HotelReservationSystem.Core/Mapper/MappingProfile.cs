using AutoMapper;
using HotelReservationSystem.Core.DTOs;
using HotelReservationSystem.Core.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Room mappings
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel != null ? src.Hotel.Name : string.Empty));

        // Reservation mappings
        CreateMap<Reservation, ReservationDto>();
        CreateMap<CreateReservationDto, Reservation>();

        // Hotel mappings
        CreateMap<Hotel, HotelDto>();
        CreateMap<Hotel, HotelDetailDto>();
        CreateMap<HotelDto, Hotel>();
    }
}
