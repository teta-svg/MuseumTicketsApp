using System;
using System.Collections.Generic;
using ConsoleApp17.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17;

public partial class MuseumTicketsDBContext : DbContext
{
    public MuseumTicketsDBContext()
    {
    }

    public MuseumTicketsDBContext(DbContextOptions<MuseumTicketsDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exhibition> Exhibitions { get; set; }

    public virtual DbSet<Museum> Museums { get; set; }

    public virtual DbSet<MuseumComplex> MuseumComplexes { get; set; }

    public virtual DbSet<MuseumExhibition> MuseumExhibitions { get; set; }

    public virtual DbSet<MuseumSchedule> MuseumSchedules { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ScheduleDay> ScheduleDays { get; set; }

    public virtual DbSet<ScheduleException> ScheduleExceptions { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketPrice> TicketPrices { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MuseumTicketsDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exhibition>(entity =>
        {
            entity.HasKey(e => e.ExhibitionId).HasName("PK__Exhibiti__32CDCC7E11D9447A");
        });

        modelBuilder.Entity<Museum>(entity =>
        {
            entity.HasKey(e => e.MuseumId).HasName("PK__Museum__C10D28D294428B64");

            entity.HasOne(d => d.MuseumComplex).WithMany(p => p.Museums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Museum_MuseumComplex");
        });

        modelBuilder.Entity<MuseumComplex>(entity =>
        {
            entity.HasKey(e => e.MuseumComplexId).HasName("PK__MuseumCo__A3CAC08F0770D3AD");
        });

        modelBuilder.Entity<MuseumExhibition>(entity =>
        {
            entity.HasKey(e => e.MuseumExhibitionId).HasName("PK__MuseumEx__8C9BE304C8B2B2E5");

            entity.HasOne(d => d.Exhibition).WithMany(p => p.MuseumExhibitions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MuseumExhibition_Exhibition");

            entity.HasOne(d => d.Museum).WithMany(p => p.MuseumExhibitions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MuseumExhibition_Museum");
        });

        modelBuilder.Entity<MuseumSchedule>(entity =>
        {
            entity.HasKey(e => e.MuseumScheduleId).HasName("PK__MuseumSc__73D1CB814906902D");

            entity.HasOne(d => d.Museum).WithMany(p => p.MuseumSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MuseumSchedule_Museum");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAFC6F99E8E");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06A1628BAA09");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.Ticket).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItem_Ticket");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A586B1F2D8A");

            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Order");
        });

        modelBuilder.Entity<ScheduleDay>(entity =>
        {
            entity.HasKey(e => e.ScheduleDaysId).HasName("PK__Schedule__3931B0C8E716E7FB");

            entity.HasOne(d => d.MuseumSchedule).WithMany(p => p.ScheduleDays)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleDays_MuseumSchedule");
        });

        modelBuilder.Entity<ScheduleException>(entity =>
        {
            entity.HasKey(e => e.ScheduleExceptionsId).HasName("PK__Schedule__F7A1D11AFE31A9F7");

            entity.HasOne(d => d.MuseumSchedule).WithMany(p => p.ScheduleExceptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleExceptions_MuseumSchedule");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC627E81D9F5E");

            entity.HasOne(d => d.Exhibition).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Exhibition");
        });

        modelBuilder.Entity<TicketPrice>(entity =>
        {
            entity.HasKey(e => e.TicketPriceId).HasName("PK__TicketPr__BE7DED9CB222B746");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketPrices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketPrice_Ticket");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC6180EAAA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
