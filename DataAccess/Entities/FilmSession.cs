using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetTasks.DataAccess.Entities
{
    public class FilmSession
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public Film? Film { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<Seat> Seats { get; set; } = [];

        // Новый список для координат ряда
        public List<RowPosition> RowPositions { get; set; } = [];

        public bool CanReserveSeat(int rowNumber, int seatNumber)
        {
            var seat = Seats.FirstOrDefault(s => s.RowNumber == rowNumber && s.SeatNumber == seatNumber);
            return seat != null && !seat.IsReserved;
        }
    }

    public class RowPosition
    {
        public int Id { get; set; }  // Уникальный идентификатор
        public int FilmSessionId { get; set; }  // ID сеанса

        public int RowNumber { get; set; }  // Номер ряда
        public float X { get; set; }  // Координата X
        public float Y { get; set; }  // Координата Y

        public FilmSession FilmSession { get; set; }  // Связь с сеансом
    }
}
