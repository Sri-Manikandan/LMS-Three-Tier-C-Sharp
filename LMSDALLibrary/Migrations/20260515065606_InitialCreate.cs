using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LMSDALLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:borrowing_status_enum", "active,returned")
                .Annotation("Npgsql:Enum:copy_status_enum", "available,borrowed,unavailable,damaged")
                .Annotation("Npgsql:Enum:fine_status_enum", "unpaid,paid,waived")
                .Annotation("Npgsql:Enum:membership_type_enum", "basic,premium,student");

            migrationBuilder.CreateTable(
                name: "book_category",
                columns: table => new
                {
                    book_category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    book_category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("book_category_pkey", x => x.book_category_id);
                });

            migrationBuilder.CreateTable(
                name: "membership_type",
                columns: table => new
                {
                    membership_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    max_active_borrowings = table.Column<int>(type: "integer", nullable: false),
                    max_borrow_days = table.Column<int>(type: "integer", nullable: false),
                    MembershipTypeName = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("membership_type_pkey", x => x.membership_type_id);
                });

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    book_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    book_title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    book_author = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    book_category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("book_pkey", x => x.book_id);
                    table.ForeignKey(
                        name: "fk_book_category",
                        column: x => x.book_category_id,
                        principalTable: "book_category",
                        principalColumn: "book_category_id");
                });

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    member_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    member_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    member_email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    member_phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    membership_type_id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("member_pkey", x => x.member_id);
                    table.ForeignKey(
                        name: "fk_member_membership_type",
                        column: x => x.membership_type_id,
                        principalTable: "membership_type",
                        principalColumn: "membership_type_id");
                });

            migrationBuilder.CreateTable(
                name: "book_copy",
                columns: table => new
                {
                    book_copy_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    book_id = table.Column<int>(type: "integer", nullable: false),
                    CopyStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("book_copy_pkey", x => x.book_copy_id);
                    table.ForeignKey(
                        name: "fk_book_copy_book",
                        column: x => x.book_id,
                        principalTable: "book",
                        principalColumn: "book_id");
                });

            migrationBuilder.CreateTable(
                name: "borrowing",
                columns: table => new
                {
                    borrowing_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    member_id = table.Column<int>(type: "integer", nullable: false),
                    book_copy_id = table.Column<int>(type: "integer", nullable: false),
                    borrow_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    return_date = table.Column<DateOnly>(type: "date", nullable: true),
                    BorrowingStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("borrowing_pkey", x => x.borrowing_id);
                    table.ForeignKey(
                        name: "fk_borrowing_book_copy",
                        column: x => x.book_copy_id,
                        principalTable: "book_copy",
                        principalColumn: "book_copy_id");
                    table.ForeignKey(
                        name: "fk_borrowing_member",
                        column: x => x.member_id,
                        principalTable: "member",
                        principalColumn: "member_id");
                });

            migrationBuilder.CreateTable(
                name: "fine",
                columns: table => new
                {
                    fine_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    borrowing_id = table.Column<int>(type: "integer", nullable: false),
                    fine_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    FinePaidStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("fine_pkey", x => x.fine_id);
                    table.ForeignKey(
                        name: "fk_fine_borrowing",
                        column: x => x.borrowing_id,
                        principalTable: "borrowing",
                        principalColumn: "borrowing_id");
                });

            migrationBuilder.CreateTable(
                name: "fine_payment",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fine_id = table.Column<int>(type: "integer", nullable: false),
                    member_id = table.Column<int>(type: "integer", nullable: false),
                    fine_paid_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    paid_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("fine_payment_pkey", x => x.payment_id);
                    table.ForeignKey(
                        name: "fk_fine_payment_fine",
                        column: x => x.fine_id,
                        principalTable: "fine",
                        principalColumn: "fine_id");
                    table.ForeignKey(
                        name: "fk_fine_payment_member",
                        column: x => x.member_id,
                        principalTable: "member",
                        principalColumn: "member_id");
                });

            migrationBuilder.CreateIndex(
                name: "idx_book_author",
                table: "book",
                column: "book_author");

            migrationBuilder.CreateIndex(
                name: "idx_book_category",
                table: "book",
                column: "book_category_id");

            migrationBuilder.CreateIndex(
                name: "idx_book_title",
                table: "book",
                column: "book_title");

            migrationBuilder.CreateIndex(
                name: "book_category_book_category_name_key",
                table: "book_category",
                column: "book_category_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_book_copy_book",
                table: "book_copy",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "idx_borrowing_member",
                table: "borrowing",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_borrowing_book_copy_id",
                table: "borrowing",
                column: "book_copy_id");

            migrationBuilder.CreateIndex(
                name: "fine_borrowing_id_key",
                table: "fine",
                column: "borrowing_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_fine_payment_member",
                table: "fine_payment",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_fine_payment_fine_id",
                table: "fine_payment",
                column: "fine_id");

            migrationBuilder.CreateIndex(
                name: "idx_member_email",
                table: "member",
                column: "member_email");

            migrationBuilder.CreateIndex(
                name: "idx_member_phone",
                table: "member",
                column: "member_phone");

            migrationBuilder.CreateIndex(
                name: "IX_member_membership_type_id",
                table: "member",
                column: "membership_type_id");

            migrationBuilder.CreateIndex(
                name: "member_member_email_key",
                table: "member",
                column: "member_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "member_member_phone_key",
                table: "member",
                column: "member_phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fine_payment");

            migrationBuilder.DropTable(
                name: "fine");

            migrationBuilder.DropTable(
                name: "borrowing");

            migrationBuilder.DropTable(
                name: "book_copy");

            migrationBuilder.DropTable(
                name: "member");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "membership_type");

            migrationBuilder.DropTable(
                name: "book_category");
        }
    }
}
