using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_GetById_And_Update_Announcements_StoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetAnnouncementById
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
                    FROM [dbo].[Announcements]
                    WHERE [Id] = @Id
                END
                ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE UpdateAnnouncement
                    @Id UNIQUEIDENTIFIER,
                    @Title NVARCHAR(150),
                    @Description NVARCHAR(1000),
                    @IsActive BIT,
                    @SubcategoryId INT
                AS
                BEGIN
                    UPDATE [dbo].[Announcements]
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
            migrationBuilder.Sql("DROP PROCEDURE GetAnnouncementById");
            migrationBuilder.Sql("DROP PROCEDURE UpdateAnnouncement");
        }
    }
}
