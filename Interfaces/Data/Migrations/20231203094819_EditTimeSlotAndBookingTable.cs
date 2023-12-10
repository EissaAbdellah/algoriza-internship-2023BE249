using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class EditTimeSlotAndBookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentTimeSlotId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentTimeSlotId",
                table: "Bookings",
                column: "AppointmentTimeSlotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentTimeSlotId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentTimeSlotId",
                table: "Bookings",
                column: "AppointmentTimeSlotId",
                unique: true);
        }
    }
}
