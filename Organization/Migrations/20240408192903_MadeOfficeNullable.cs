using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Organization.Migrations
{
    /// <inheritdoc />
    public partial class MadeOfficeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parish",
                columns: table => new
                {
                    ParishId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parish", x => x.ParishId);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    OfficeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.OfficeId);
                    table.ForeignKey(
                        name: "FK_Office_Parish_ParishId",
                        column: x => x.ParishId,
                        principalTable: "Parish",
                        principalColumn: "ParishId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Office_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Office",
                        principalColumn: "OfficeId");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTeam",
                columns: table => new
                {
                    EmployeesEmployeeId = table.Column<int>(type: "int", nullable: false),
                    TeamsTeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTeam", x => new { x.EmployeesEmployeeId, x.TeamsTeamId });
                    table.ForeignKey(
                        name: "FK_EmployeeTeam_Employee_EmployeesEmployeeId",
                        column: x => x.EmployeesEmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTeam_Team_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parish",
                columns: new[] { "ParishId", "Name", "ZipCode" },
                values: new object[,]
                {
                    { 1, "Vermillion", 70510 },
                    { 2, "Laffyette", 70508 },
                    { 3, "East Baton Rouge", 70808 },
                    { 4, "Ascension", 70346 }
                });

            migrationBuilder.InsertData(
                table: "Team",
                columns: new[] { "TeamId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "For cool people only.", "Cool Team" },
                    { 2, "For lame people >:)", "Lame Team" },
                    { 3, "A team just for Hans", "Just Hans" }
                });

            migrationBuilder.InsertData(
                table: "Office",
                columns: new[] { "OfficeId", "Name", "ParishId" },
                values: new object[,]
                {
                    { 1, "Diamond Nails", 1 },
                    { 2, "Atlas", 2 },
                    { 3, "CGI", 4 },
                    { 4, "OTS/PMO", 3 }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "Age", "FirstName", "LastName", "OfficeId" },
                values: new object[,]
                {
                    { 1, 20, "Hoa", "Nguyen", 4 },
                    { 2, 21, "Alex", "Brodsky", 4 },
                    { 3, 47, "Some", "Guy", 1 },
                    { 4, 33, "Hans", "Neuenhaus", 4 }
                });

            migrationBuilder.InsertData(
                table: "EmployeeTeam",
                columns: new[] { "EmployeesEmployeeId", "TeamsTeamId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 1 },
                    { 4, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_OfficeId",
                table: "Employee",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTeam_TeamsTeamId",
                table: "EmployeeTeam",
                column: "TeamsTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Office_ParishId",
                table: "Office",
                column: "ParishId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTeam");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "Parish");
        }
    }
}
