namespace AspNetTasks.DataAccess.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public int FilmSessionId { get; set; }
        public DateTime ReservedAt { get; set; }
        public ICollection<ReservationSeat> Seats { get; set; } = new List<ReservationSeat>();
    }

    public class ReservationSeat
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
