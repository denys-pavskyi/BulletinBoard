using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedAuthProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE PROCEDURE RegisterUser
                    @Id UNIQUEIDENTIFIER,
                    @Username NVARCHAR(30),
                    @Email NVARCHAR(60),
                    @Password NVARCHAR(40)
                AS
                BEGIN
                    INSERT INTO Users (Id, Username, Email, Password)
                    VALUES (@Id, @Username, @Email, @Password)
                END
                """);

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE AuthorizeUser
                    @Username NVARCHAR(30),
                    @Password NVARCHAR(40)
                AS
                BEGIN
                    SELECT * FROM Users
                    WHERE Username = @Username AND Password = @Password
                END
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS RegisterUser");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AuthorizeUser");
        }
    }
}
