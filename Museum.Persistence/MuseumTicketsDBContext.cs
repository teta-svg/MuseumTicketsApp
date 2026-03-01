using Microsoft.EntityFrameworkCore;
using Museum.Persistence.Configurations;
using Museum.Domain;
using MuseumEntity = Museum.Domain.Museum;

namespace Museum.Persistence;

public partial class MuseumTicketsDBContext : DbContext
{
    public MuseumTicketsDBContext() { }

    public MuseumTicketsDBContext(DbContextOptions<MuseumTicketsDBContext> options)
        : base(options) { }

    public virtual DbSet<Exhibition> Exhibitions { get; set; }
    public virtual DbSet<MuseumEntity> Museums { get; set; }
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
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MuseumTicketsDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MuseumTicketsDBContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
