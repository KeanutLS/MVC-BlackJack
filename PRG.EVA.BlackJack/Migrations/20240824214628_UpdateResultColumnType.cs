using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRG.EVA.BlackJack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResultColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Gamelogs",
                table: "Gamelogs");

            migrationBuilder.RenameTable(
                name: "Gamelogs",
                newName: "GameLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "GameLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameLogs",
                table: "GameLogs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameLogs",
                table: "GameLogs");

            migrationBuilder.RenameTable(
                name: "GameLogs",
                newName: "Gamelogs");

            migrationBuilder.AlterColumn<int>(
                name: "Result",
                table: "Gamelogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gamelogs",
                table: "Gamelogs",
                column: "Id");
        }
    }
}
