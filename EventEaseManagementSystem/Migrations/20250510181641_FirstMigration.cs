using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEaseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });
            /*

            migrationBuilder.CreateTable(
                name: "Venue",
                columns: table => new
                {
                    VenueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Location = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Venue__3C57E5D2B348A4AD", x => x.VenueID);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    EventDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    VenueID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Event__7944C87006A11589", x => x.EventID);
                    table.ForeignKey(
                        name: "Event_Venue",
                        column: x => x.VenueID,
                        principalTable: "Venue",
                        principalColumn: "VenueID");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueID = table.Column<int>(type: "int", nullable: false),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__73951ACD896D0D81", x => x.BookingID);
                    table.ForeignKey(
                        name: "Booking_Event",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID");
                    table.ForeignKey(
                        name: "Booking_Venue",
                        column: x => x.VenueID,
                        principalTable: "Venue",
                        principalColumn: "VenueID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_EventID",
                table: "Booking",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "UniqueBooking",
                table: "Booking",
                columns: new[] { "VenueID", "EventID", "BookingDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_VenueID",
                table: "Event",
                column: "VenueID");

            migrationBuilder.CreateIndex(
                name: "UQ__Venue__A40F8D12023FA950",
                table: "Venue",
                column: "VenueName",
                unique: true);
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropTable(
                name: "Booking");
            */
            migrationBuilder.DropTable(
                name: "Customers");
            /*
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Venue");
            */
        }
    }
}
