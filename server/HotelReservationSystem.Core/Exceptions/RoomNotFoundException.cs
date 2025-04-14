using System;

namespace HotelReservationSystem.Core.Exceptions
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException() : base("The requested room was not found.")
        {
        }

        public RoomNotFoundException(int roomId)
            : base($"Room with ID {roomId} was not found.")
        {
        }

        public RoomNotFoundException(string message) : base(message)
        {
        }

        public RoomNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}