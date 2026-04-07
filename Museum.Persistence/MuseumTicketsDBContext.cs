using Microsoft.EntityFrameworkCore;
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
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=MuseumTicketsDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MuseumTicketsDBContext).Assembly);

        modelBuilder.Entity<User>().HasData(
    new User // Администратор системы
    {
        UserId = 10001,
        LastName = "Админов",
        FirstName = "Петр",
        MiddleName = "Петрович",
        Email = "sysadmin@museum.ru",
        Password = "$2a$11$0NR8Ir7eg9MNV9VFQsudROSeSoRodME7UsMzNNfmLla/e/gLzQyuK", // AdminPassword123!
        Phone = "+70000000001",
        Role = "Администратор системы"
    },
    new User // Администратор музея
    {
        UserId = 10002,
        LastName = "Музеев",
        FirstName = "Иван",
        MiddleName = "Иванович",
        Email = "museumadmin@museum.ru",
        Password = "$2a$11$0NR8Ir7eg9MNV9VFQsudROSeSoRodME7UsMzNNfmLla/e/gLzQyuK", // AdminPassword123!
        Phone = "+70000000002",
        Role = "Администратор музея"
    }
);
    }
}
