using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGetPostsByUserAndFilteredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var getAllByUserSql = @"
                CREATE PROCEDURE GetAllPostsByUserId
                    @UserId UNIQUEIDENTIFIER
                AS
                BEGIN
                    SELECT *
                    FROM Posts
                    WHERE UserId = @UserId
                END";


            var getAllByFilterSql = @"
                CREATE PROCEDURE GetAllPostsByFilter
                    @SubcategoryIds NVARCHAR(MAX),
                    @IsActive BIT
                AS
                BEGIN
                    DECLARE @SubcategoryTable TABLE (Id INT);
                    INSERT INTO @SubcategoryTable (Id)
                    SELECT CAST([value] AS INT)
                    FROM STRING_SPLIT(@SubcategoryIds, ',');

                    SELECT *
                    FROM Posts
                    WHERE SubcategoryId IN (SELECT Id FROM @SubcategoryTable)
                      AND IsActive = @IsActive
                END";

            migrationBuilder.Sql(getAllByUserSql);
            migrationBuilder.Sql(getAllByFilterSql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllPostsByUserId");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllPostsByFilter");
        }
    }
}
