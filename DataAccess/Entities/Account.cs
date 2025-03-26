namespace AspNetTasks.DataAccess.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

        // Навигационное свойство для бронирований
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
