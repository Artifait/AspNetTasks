using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetTasks.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreFunc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Accounts_AccountId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_FilmSessions_FilmSessionId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Seat_SeatId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_FilmSessionId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SeatId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Reservations",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ReservationSeat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeatId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationSeat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationSeat_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationSeat_Seat_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationSeat_ReservationId",
                table: "ReservationSeat",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationSeat_SeatId",
                table: "ReservationSeat",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Accounts_AccountId",
                table: "Reservations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Accounts_AccountId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "ReservationSeat");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_FilmSessionId",
                table: "Reservations",
                column: "FilmSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SeatId",
                table: "Reservations",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Accounts_AccountId",
                table: "Reservations",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_FilmSessions_FilmSessionId",
                table: "Reservations",
                column: "FilmSessionId",
                principalTable: "FilmSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Seat_SeatId",
                table: "Reservations",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
