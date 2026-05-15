using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSDALLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEnumsToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cast each column to text only if it is not already text
            migrationBuilder.Sql("ALTER TABLE book_copy ALTER COLUMN copy_status TYPE text USING copy_status::text");
            migrationBuilder.Sql("ALTER TABLE borrowing ALTER COLUMN borrowing_status TYPE text USING borrowing_status::text");
            migrationBuilder.Sql("ALTER TABLE fine ALTER COLUMN fine_paid_status TYPE text USING fine_paid_status::text");
            migrationBuilder.Sql("ALTER TABLE membership_type ALTER COLUMN membership_type_name TYPE text USING membership_type_name::text");
            // (PostgreSQL silently no-ops ALTER COLUMN TYPE text on a column already of type text)

            // Drop the now-unused PostgreSQL enum types (CASCADE removes any dependent views/functions)
            migrationBuilder.Sql("DROP TYPE IF EXISTS copy_status_enum CASCADE");
            migrationBuilder.Sql("DROP TYPE IF EXISTS borrowing_status_enum CASCADE");
            migrationBuilder.Sql("DROP TYPE IF EXISTS fine_status_enum CASCADE");
            migrationBuilder.Sql("DROP TYPE IF EXISTS membership_type_enum CASCADE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "membership_type_name",
                table: "membership_type",
                newName: "MembershipTypeName");

            migrationBuilder.RenameColumn(
                name: "fine_paid_status",
                table: "fine",
                newName: "FinePaidStatus");

            migrationBuilder.RenameColumn(
                name: "borrowing_status",
                table: "borrowing",
                newName: "BorrowingStatus");

            migrationBuilder.RenameColumn(
                name: "copy_status",
                table: "book_copy",
                newName: "CopyStatus");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:borrowing_status_enum", "active,returned")
                .Annotation("Npgsql:Enum:copy_status_enum", "available,borrowed,unavailable,damaged")
                .Annotation("Npgsql:Enum:fine_status_enum", "unpaid,paid,waived")
                .Annotation("Npgsql:Enum:membership_type_enum", "basic,premium,student");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipTypeName",
                table: "membership_type",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "FinePaidStatus",
                table: "fine",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "BorrowingStatus",
                table: "borrowing",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CopyStatus",
                table: "book_copy",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
