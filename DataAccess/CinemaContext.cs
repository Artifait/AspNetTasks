using Microsoft.EntityFrameworkCore;
using AspNetTasks.DataAccess.Entities;

namespace AspNetTasks.DataAccess
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        {
        }

        public DbSet<Film> Films { get; set; }             
        public DbSet<FilmSession> FilmSessions { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<RowPosition> RowPositions { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationSeat> ReservationSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Film>()
                .HasMany(f => f.Sessions)
                .WithOne(fs => fs.Film)
                .HasForeignKey(fs => fs.FilmId);

            modelBuilder.Entity<FilmSession>()
                .HasMany(fs => fs.Seats)
                .WithOne(s => s.FilmSession)
                .HasForeignKey(s => s.FilmSessionId);

            modelBuilder.Entity<FilmSession>()
                .HasMany(fs => fs.RowPositions)
                .WithOne()
                .HasForeignKey(rp => rp.FilmSessionId); 

            modelBuilder.Entity<ReservationSeat>()
                .HasOne(rs => rs.Reservation)
                .WithMany(r => r.Seats)
                .HasForeignKey(rs => rs.ReservationId);

            modelBuilder.Entity<ReservationSeat>()
                .HasOne(rs => rs.Seat)
                .WithMany()
                .HasForeignKey(rs => rs.SeatId);
        }
    }
}
