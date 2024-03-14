using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESERVATION.Migrations
{
    public partial class reservtion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DateViewModel",
                columns: table => new
                {
                    res_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    coursem_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OptionViewModel",
                columns: table => new
                {
                    res_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    coursem_id = table.Column<int>(type: "int", nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    option_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "T_COURSE",
                columns: table => new
                {
                    courceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    limitMinNum = table.Column<int>(type: "int", nullable: false),
                    limitMaxNum = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    alertMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_COURSE", x => x.courceId);
                });

            migrationBuilder.CreateTable(
                name: "T_COURSEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alertMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_COURSEM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_OPTION",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courceId = table.Column<int>(type: "int", nullable: false),
                    optionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    alertMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OPTION", x => x.OptionId);
                });

            migrationBuilder.CreateTable(
                name: "T_RESERVATION",
                columns: table => new
                {
                    reservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    coursem_id = table.Column<int>(type: "int", nullable: false),
                    cource_id = table.Column<int>(type: "int", nullable: false),
                    option_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phonenumber = table.Column<int>(type: "int", nullable: false),
                    mail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentIntentid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    calendarid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    update = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RESERVATION", x => x.reservationId);
                });

            migrationBuilder.CreateTable(
                name: "T_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_USER", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DateViewModel");

            migrationBuilder.DropTable(
                name: "OptionViewModel");

            migrationBuilder.DropTable(
                name: "T_COURSE");

            migrationBuilder.DropTable(
                name: "T_COURSEM");

            migrationBuilder.DropTable(
                name: "T_OPTION");

            migrationBuilder.DropTable(
                name: "T_RESERVATION");

            migrationBuilder.DropTable(
                name: "T_USER");
        }
    }
}
