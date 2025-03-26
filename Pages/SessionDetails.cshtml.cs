using AspNetTasks.DataAccess.Entities;
using AspNetTasks.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetTasks.Pages
{
    public class SessionDetailsModel : PageModel
    {
        private readonly CinemaContext _context;

        public SessionDetailsModel(CinemaContext context)
        {
            _context = context;
        }

        public FilmSession Session { get; set; }
        public List<RowPosition> RowPositions { get; set; }
        public int FilmId { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public List<int> SelectedSeats { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Получаем сеанс по ID
            Session = await _context.FilmSessions
                .Include(fs => fs.Seats)
                .FirstOrDefaultAsync(fs => fs.Id == id);

            if (Session == null)
            {
                return NotFound();
            }

            // Получаем координаты рядов
            RowPositions = await _context.RowPositions
                .Where(rp => rp.FilmSessionId == id)
                .ToListAsync();

            FilmId = Session.FilmId;

            return Page();
        }

        public async Task<IActionResult> OnPostReserveAsync(int sessionId)
        {
            if (SelectedSeats == null || string.IsNullOrEmpty(Phone))
            {
                return Page(); // Вернем пользователя обратно на страницу
            }

            var session = await _context.FilmSessions
                .Include(fs => fs.Seats)
                .FirstOrDefaultAsync(fs => fs.Id == sessionId);

            if (session == null)
            {
                return NotFound();
            }

            // Проверка доступности мест
            foreach (var seatId in SelectedSeats)
            {
                var seat = session.Seats.FirstOrDefault(s => s.Id == seatId);
                if (seat == null || seat.IsReserved)
                {
                    ModelState.AddModelError("", "Одно из выбранных мест уже занято.");
                    return Page();
                }
            }

            // Создание новой записи о бронировании
            var reservation = new Reservation
            {
                Phone = Phone,
                FilmSessionId = sessionId,
                ReservedAt = DateTime.Now
            };

            // Добавляем выбранные места в бронирование
            foreach (var seatId in SelectedSeats)
            {
                var seat = session.Seats.FirstOrDefault(s => s.Id == seatId);
                if (seat != null)
                {
                    reservation.Seats.Add(new ReservationSeat
                    {
                        SeatId = seatId,
                        Seat = seat
                    });

                    // Отметим место как занято
                    seat.IsReserved = true;
                }
            }

            // Сохраняем изменения
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("/FilmDetails", new { id = session.FilmId });
        }
    }
}
