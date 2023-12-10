using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class CreateAppointmentTable_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesSlot_Appointments_AppointmentId",
                table: "TimesSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimesSlot",
                table: "TimesSlot");

            migrationBuilder.RenameTable(
                name: "TimesSlot",
                newName: "Times");

            migrationBuilder.RenameIndex(
                name: "IX_TimesSlot_AppointmentId",
                table: "Times",
                newName: "IX_Times_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Times",
                table: "Times",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Times_Appointments_AppointmentId",
                table: "Times",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Times_Appointments_AppointmentId",
                table: "Times");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Times",
                table: "Times");

            migrationBuilder.RenameTable(
                name: "Times",
                newName: "TimesSlot");

            migrationBuilder.RenameIndex(
                name: "IX_Times_AppointmentId",
                table: "TimesSlot",
                newName: "IX_TimesSlot_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimesSlot",
                table: "TimesSlot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimesSlot_Appointments_AppointmentId",
                table: "TimesSlot",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
