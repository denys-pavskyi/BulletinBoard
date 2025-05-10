using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddAndRemovePostsStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE AddPost
                    @Id UNIQUEIDENTIFIER,
                    @Title NVARCHAR(150),
                    @Description NVARCHAR(1000),
                    @IsActive BIT,
                    @CreatedDate DATETIME2,
                    @SubcategoryId INT,
                    @UserId UNIQUEIDENTIFIER
                AS
                BEGIN
                    INSERT INTO [dbo].[Posts] (
                        [Id],
                        [Title],
                        [Description],
                        [IsActive],
                        [CreatedDate],
                        [SubcategoryId],
                        [UserId])
                    VALUES (
                        @Id,
                        @Title,
                        @Description,
                        @IsActive,
                        @CreatedDate,
                        @SubcategoryId,
                        @UserId);
                END
                ");


            migrationBuilder.Sql(@"
                CREATE PROCEDURE DeletePostById
                    @Id UNIQUEIDENTIFIER
                AS
                BEGIN
                    DELETE FROM [dbo].[Posts]
                    WHERE [Id] = @Id;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE AddPost");
            migrationBuilder.Sql("DROP PROCEDURE RemovePostById");  
        }
    }
}
