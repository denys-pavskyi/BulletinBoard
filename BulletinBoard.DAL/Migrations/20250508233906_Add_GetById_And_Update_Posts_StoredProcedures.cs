using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_GetById_And_Update_Posts_StoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetPostById
                    @Id UNIQUEIDENTIFIER
                AS
                BEGIN
                    SELECT [Id],
                           [Title],
                           [Description],
                           [IsActive],
                           [CreatedDate],
                           [SubcategoryId],
                           [UserId]
                    FROM [dbo].[Posts]
                    WHERE [Id] = @Id
                END
                ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE UpdatePost
                    @Id UNIQUEIDENTIFIER,
                    @Title NVARCHAR(150),
                    @Description NVARCHAR(1000),
                    @IsActive BIT,
                    @SubcategoryId INT
                AS
                BEGIN
                    UPDATE [dbo].[Posts]
                    SET 
                        [Title] = @Title,
                        [Description] = @Description,
                        [IsActive] = @IsActive,
                        [SubcategoryId] = @SubcategoryId
                    WHERE [Id] = @Id
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetPostById");
            migrationBuilder.Sql("DROP PROCEDURE UpdatePost");
        }
    }
}
