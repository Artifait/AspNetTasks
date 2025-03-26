using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetTasks.Migrations
{
    /// <inheritdoc />
    public partial class AddRowPositionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Accounts_AccountId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeat_Reservations_ReservationId",
                table: "ReservationSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeat_Seat_SeatId",
                table: "ReservationSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_FilmSessions_FilmSessionId",
                table: "Seat");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_AccountId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seat",
                table: "Seat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationSeat",
                table: "ReservationSeat");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Reservations");

            migrationBuilder.RenameTable(
                name: "Seat",
                newName: "Seats");

            migrationBuilder.RenameTable(
                name: "ReservationSeat",
                newName: "ReservationSeats");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_FilmSessionId",
                table: "Seats",
                newName: "IX_Seats_FilmSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationSeat_SeatId",
                table: "ReservationSeats",
                newName: "IX_ReservationSeats_SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationSeat_ReservationId",
                table: "ReservationSeats",
                newName: "IX_ReservationSeats_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationSeats",
                table: "ReservationSeats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RowPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FilmSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<float>(type: "REAL", nullable: false),
                    Y = table.Column<float>(type: "REAL", nullable: false),
                    FilmSessionId1 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RowPositions_FilmSessions_FilmSessionId",
                        column: x => x.FilmSessionId,
                        principalTable: "FilmSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RowPositions_FilmSessions_FilmSessionId1",
                        column: x => x.FilmSessionId1,
                        principalTable: "FilmSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RowPositions_FilmSessionId",
                table: "RowPositions",
                column: "FilmSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_RowPositions_FilmSessionId1",
                table: "RowPositions",
                column: "FilmSessionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeats_Reservations_ReservationId",
                table: "ReservationSeats",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeats_Seats_SeatId",
                table: "ReservationSeats",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_FilmSessions_FilmSessionId",
                table: "Seats",
                column: "FilmSessionId",
                principalTable: "FilmSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeats_Reservations_ReservationId",
                table: "ReservationSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSeats_Seats_SeatId",
                table: "ReservationSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_FilmSessions_FilmSessionId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "RowPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationSeats",
                table: "ReservationSeats");

            migrationBuilder.RenameTable(
                name: "Seats",
                newName: "Seat");

            migrationBuilder.RenameTable(
                name: "ReservationSeats",
                newName: "ReservationSeat");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_FilmSessionId",
                table: "Seat",
                newName: "IX_Seat_FilmSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationSeats_SeatId",
                table: "ReservationSeat",
                newName: "IX_ReservationSeat_SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_ReservationSeats_ReservationId",
                table: "ReservationSeat",
                newName: "IX_ReservationSeat_ReservationId");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Reservations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seat",
                table: "Seat",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationSeat",
                table: "ReservationSeat",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AccountId",
                table: "Reservations",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Accounts_AccountId",
                table: "Reservations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeat_Reservations_ReservationId",
                table: "ReservationSeat",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSeat_Seat_SeatId",
                table: "ReservationSeat",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_FilmSessions_FilmSessionId",
                table: "Seat",
                column: "FilmSessionId",
                principalTable: "FilmSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
