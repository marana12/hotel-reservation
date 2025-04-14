namespace HotelReservationSystem.Core.Exceptions;

public class ReservationNotFoundException : Exception
{
    public ReservationNotFoundException(int id)
        : base($"Reservation with ID {id} not found")
    {
    }
}

public class InvalidReservationDatesException : Exception
{
    public InvalidReservationDatesException(string message)
        : base(message)
    {
    }
}
