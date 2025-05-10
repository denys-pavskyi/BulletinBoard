using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedGetAllPostsStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE GetAllPosts
            AS
            BEGIN
                SELECT [Id]
                  ,[Title]
                  ,[Description]
                  ,[IsActive]
                  ,[CreatedDate]
                  ,[SubcategoryId]
                  ,[UserId]
              FROM [BulletinBoardDB].[dbo].[Posts]
            END
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetAllPosts");
        }
    }
}
