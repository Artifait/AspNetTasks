using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetTasks.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatsAndSeatReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxReservations",
                table: "FilmSessions");

            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilmSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    IsReserved = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seat_FilmSessions_FilmSessionId",
                        column: x => x.FilmSessionId,
                        principalTable: "FilmSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SeatId",
                table: "Reservations",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_FilmSessionId",
                table: "Seat",
                column: "FilmSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Seat_SeatId",
                table: "Reservations",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Seat_SeatId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SeatId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "MaxReservations",
                table: "FilmSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
