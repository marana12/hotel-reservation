using System;

namespace HotelReservationSystem.Core.Exceptions
{
    public class RoomNotAvailableException : Exception
    {
        public RoomNotAvailableException() : base("The room is not available for the selected dates.")
        {
        }

        public RoomNotAvailableException(int roomId, DateTime checkIn, DateTime checkOut)
            : base($"Room with ID {roomId} is not available from {checkIn:MM/dd/yyyy} to {checkOut:MM/dd/yyyy}.")
        {
        }

        public RoomNotAvailableException(string message) : base(message)
        {
        }

        public RoomNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}