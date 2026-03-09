using Microsoft.EntityFrameworkCore;
using Museum.Domain;
using Museum.Persistence.Configurations;
using System.Text.Json;
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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MuseumTicketsDBContext).Assembly);

        modelBuilder.Entity<User>().HasData(new User //админ
        {
            UserId = 10001,
            LastName = "Админов",
            FirstName = "Петр",
            MiddleName = "Петрович",
            Email = "admin@museum.ru",
            Password = "$2a$11$0NR8Ir7eg9MNV9VFQsudROSeSoRodME7UsMzNNfmLla/e/gLzQyuK",
            Phone = "+70000000000",
            Role = "Администратор системы"
        });

        var fileName = "seed.json";

        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        if (File.Exists(jsonPath))
        {
            try
            {
                var json = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<SeedDataWrapper>(json, options);

                if (data != null)
                {
                    modelBuilder.Entity<MuseumComplex>().HasData(data.MuseumComplexes);
                    modelBuilder.Entity<MuseumEntity>().HasData(data.Museums);
                    modelBuilder.Entity<Exhibition>().HasData(data.Exhibitions);
                    modelBuilder.Entity<Ticket>().HasData(data.Tickets);
                    modelBuilder.Entity<TicketPrice>().HasData(data.TicketPrices);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки сидов: {ex.Message}");
            }
        }
    }
}
