using AspNetTasks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.DataAccess.Repositories
{
    //public class ReservationRepository
    //{
    //    private readonly CinemaContext _context;

    //    public ReservationRepository(CinemaContext context)
    //    {
    //        _context = context;
    //    }

    //    // Метод для бронирования места на сеансе
    //    public async Task<bool> ReserveSeatAsync(int accountId, int filmSessionId, int rowNumber, int seatNumber)
    //    {
    //        // Проверяем, доступно ли место для бронирования
    //        var session = await _context.FilmSessions
    //                                     .Include(fs => fs.Seats)
    //                                     .FirstOrDefaultAsync(fs => fs.Id == filmSessionId);

    //        if (session == null)
    //        {
    //            return false; // Сеанс не найден
    //        }

    //        var seat = session.Seats.FirstOrDefault(s => s.RowNumber == rowNumber && s.SeatNumber == seatNumber);

    //        if (seat == null || seat.IsReserved)
    //        {
    //            return false; // Место не существует или уже забронировано
    //        }

    //        // Создаем бронирование для места
    //        var reservation = new Reservation
    //        {
    //            AccountId = accountId,
    //            FilmSessionId = filmSessionId,
    //            SeatId = seat.Id,
    //            ReservedAt = DateTime.Now
    //        };

    //        seat.IsReserved = true; // Отмечаем место как забронированное
    //        await _context.Reservations.AddAsync(reservation);
    //        await _context.SaveChangesAsync();
    //        return true;
    //    }

    //    // Получаем все бронирования по аккаунту
    //    public async Task<IEnumerable<Reservation>> GetReservationsByAccountAsync(int accountId)
    //    {
    //        return await _context.Reservations
    //            .Where(r => r.AccountId == accountId)
    //            .Include(r => r.FilmSession)
    //            .ThenInclude(fs => fs.Film)
    //            .Include(r => r.Seat)
    //            .ToListAsync();
    //    }

    //    // Получаем все бронирования для сеанса
    //    public async Task<IEnumerable<Reservation>> GetReservationsBySessionAsync(int filmSessionId)
    //    {
    //        return await _context.Reservations
    //            .Where(r => r.FilmSessionId == filmSessionId)
    //            .Include(r => r.Seat)
    //            .ToListAsync();
    //    }

    //    // Метод для отмены бронирования
    //    public async Task<bool> CancelReservationAsync(int reservationId)
    //    {
    //        var reservation = await _context.Reservations
    //            .Include(r => r.Seat)
    //            .FirstOrDefaultAsync(r => r.Id == reservationId);

    //        if (reservation == null)
    //        {
    //            return false; // Бронирование не найдено
    //        }

    //        reservation.Seat.IsReserved = false; // Отмечаем место как свободное
    //        _context.Reservations.Remove(reservation);
    //        await _context.SaveChangesAsync();
    //        return true;
    //    }
    //}
}
