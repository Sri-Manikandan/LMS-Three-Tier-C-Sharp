using System;
using System.Collections.Generic;
using LMSModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSDALLibrary.Contexts;

public partial class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<BookCopy> BookCopies { get; set; }

    public virtual DbSet<Borrowing> Borrowings { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<FinePayment> FinePayments { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MembershipType> MembershipTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(DBHelper.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<BorrowingStatusEnum>()
                    .HasPostgresEnum<CopyStatusEnum>()
                    .HasPostgresEnum<FineStatusEnum>()
                    .HasPostgresEnum<MembershipTypeEnum>();

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("book_pkey");

            entity.ToTable("book");

            entity.HasIndex(e => e.BookAuthor, "idx_book_author");

            entity.HasIndex(e => e.BookCategoryId, "idx_book_category");

            entity.HasIndex(e => e.BookTitle, "idx_book_title");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.BookAuthor)
                .HasMaxLength(150)
                .HasColumnName("book_author");
            entity.Property(e => e.BookCategoryId).HasColumnName("book_category_id");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(200)
                .HasColumnName("book_title");

            entity.HasOne(d => d.BookCategory).WithMany(p => p.Books)
                .HasForeignKey(d => d.BookCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_category");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity.HasKey(e => e.BookCategoryId).HasName("book_category_pkey");

            entity.ToTable("book_category");

            entity.HasIndex(e => e.BookCategoryName, "book_category_book_category_name_key").IsUnique();

            entity.Property(e => e.BookCategoryId).HasColumnName("book_category_id");
            entity.Property(e => e.BookCategoryName)
                .HasMaxLength(100)
                .HasColumnName("book_category_name");
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.HasKey(e => e.BookCopyId).HasName("book_copy_pkey");

            entity.ToTable("book_copy");

            entity.HasIndex(e => e.BookId, "idx_book_copy_book");

            entity.Property(e => e.BookCopyId).HasColumnName("book_copy_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookCopies)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_copy_book");
        });

        modelBuilder.Entity<Borrowing>(entity =>
        {
            entity.HasKey(e => e.BorrowingId).HasName("borrowing_pkey");

            entity.ToTable("borrowing");

            entity.HasIndex(e => e.MemberId, "idx_borrowing_member");

            entity.Property(e => e.BorrowingId).HasColumnName("borrowing_id");
            entity.Property(e => e.BookCopyId).HasColumnName("book_copy_id");
            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("borrow_date");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");

            entity.HasOne(d => d.BookCopy).WithMany(p => p.Borrowings)
                .HasForeignKey(d => d.BookCopyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_borrowing_book_copy");

            entity.HasOne(d => d.Member).WithMany(p => p.Borrowings)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_borrowing_member");
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(e => e.FineId).HasName("fine_pkey");

            entity.ToTable("fine");

            entity.HasIndex(e => e.BorrowingId, "fine_borrowing_id_key").IsUnique();

            entity.Property(e => e.FineId).HasColumnName("fine_id");
            entity.Property(e => e.BorrowingId).HasColumnName("borrowing_id");
            entity.Property(e => e.FineAmount)
                .HasPrecision(10, 2)
                .HasColumnName("fine_amount");

            entity.HasOne(d => d.Borrowing).WithOne(p => p.Fine)
                .HasForeignKey<Fine>(d => d.BorrowingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fine_borrowing");
        });

        modelBuilder.Entity<FinePayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("fine_payment_pkey");

            entity.ToTable("fine_payment");

            entity.HasIndex(e => e.MemberId, "idx_fine_payment_member");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.FineId).HasColumnName("fine_id");
            entity.Property(e => e.FinePaidAmount)
                .HasPrecision(10, 2)
                .HasColumnName("fine_paid_amount");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.PaidDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("paid_date");

            entity.HasOne(d => d.Fine).WithMany(p => p.FinePayments)
                .HasForeignKey(d => d.FineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fine_payment_fine");

            entity.HasOne(d => d.Member).WithMany(p => p.FinePayments)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fine_payment_member");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("member_pkey");

            entity.ToTable("member");

            entity.HasIndex(e => e.MemberEmail, "idx_member_email");

            entity.HasIndex(e => e.MemberPhone, "idx_member_phone");

            entity.HasIndex(e => e.MemberEmail, "member_member_email_key").IsUnique();

            entity.HasIndex(e => e.MemberPhone, "member_member_phone_key").IsUnique();

            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MemberEmail)
                .HasMaxLength(150)
                .HasColumnName("member_email");
            entity.Property(e => e.MemberName)
                .HasMaxLength(100)
                .HasColumnName("member_name");
            entity.Property(e => e.MemberPhone)
                .HasMaxLength(15)
                .HasColumnName("member_phone");
            entity.Property(e => e.MembershipTypeId).HasColumnName("membership_type_id");

            entity.HasOne(d => d.MembershipType).WithMany(p => p.Members)
                .HasForeignKey(d => d.MembershipTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_member_membership_type");
        });

        modelBuilder.Entity<MembershipType>(entity =>
        {
            entity.HasKey(e => e.MembershipTypeId).HasName("membership_type_pkey");

            entity.ToTable("membership_type");

            entity.Property(e => e.MembershipTypeId).HasColumnName("membership_type_id");
            entity.Property(e => e.MaxActiveBorrowings).HasColumnName("max_active_borrowings");
            entity.Property(e => e.MaxBorrowDays).HasColumnName("max_borrow_days");
        });

OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
