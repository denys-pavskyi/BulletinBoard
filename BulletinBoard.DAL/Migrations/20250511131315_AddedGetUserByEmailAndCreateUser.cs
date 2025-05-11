using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedGetUserByEmailAndCreateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE PROCEDURE GetUserByEmail
                    @Email NVARCHAR(60)
                AS
                BEGIN
                    SELECT * FROM Users WHERE Email = @Email
                END
                """
            );

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE CreateUser
                    @Id UNIQUEIDENTIFIER,
                    @Username NVARCHAR(30),
                    @Email NVARCHAR(60),
                    @PasswordHash NVARCHAR(255),
                    @Provider NVARCHAR(20),
                    @CreatedAt DATETIME
                AS
                BEGIN
                    INSERT INTO Users (Id, Username, Email, PasswordHash, Provider, CreatedAt)
                    VALUES (@Id, @Username, @Email, @PasswordHash, @Provider, @CreatedAt)
                END
                """
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetUserByEmail");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateUser");
        }
    }
}
