namespace AspNetTasks.DataAccess.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int FilmSessionId { get; set; }
        public int RowNumber { get; set; }    // Номер ряда
        public int SeatNumber { get; set; }   // Порядковый номер места в ряду
        public bool IsReserved { get; set; }  // Статус бронирования

        public FilmSession FilmSession { get; set; }
    }
}
