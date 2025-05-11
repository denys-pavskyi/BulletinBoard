using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedRefreshTokenProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE PROCEDURE CreateRefreshToken
                    @Id UNIQUEIDENTIFIER,
                    @UserId UNIQUEIDENTIFIER,
                    @Token NVARCHAR(200),
                    @ExpiresAt DATETIME2
                AS
                BEGIN
                    INSERT INTO RefreshTokens (Id, UserId, Token, ExpiresAt)
                    VALUES (@Id, @UserId, @Token, @ExpiresAt)
                END
                """
            );

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE GetRefreshTokenByToken
                    @Token NVARCHAR(200)
                AS
                BEGIN
                    SELECT * FROM RefreshTokens
                    WHERE Token = @Token
                END
                """
            );

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE UpdateRefreshToken
                    @OldToken NVARCHAR(200),
                    @NewToken NVARCHAR(200),
                    @NewExpiresAt DATETIME2
                AS
                BEGIN
                    UPDATE RefreshTokens
                    SET Token = @NewToken,
                        ExpiresAt = @NewExpiresAt
                    WHERE Token = @OldToken
                END
                """
            );

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE DeleteRefreshToken
                    @Token NVARCHAR(200)
                AS
                BEGIN
                    DELETE FROM RefreshTokens
                    WHERE Token = @Token
                END
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateRefreshToken");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetRefreshTokenByToken");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateRefreshToken");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteRefreshToken");
        }
    }
}
