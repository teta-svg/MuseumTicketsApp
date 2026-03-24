using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Museum.Domain;

namespace Museum.Persistence.Configurations
{
    public class MuseumComplexConfiguration : IEntityTypeConfiguration<MuseumComplex>
    {
        public void Configure(EntityTypeBuilder<MuseumComplex> builder)
        {
            builder.HasKey(e => e.MuseumComplexId);
            builder.Property(e => e.MuseumComplexId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
        }
    }

    public class MuseumConfiguration : IEntityTypeConfiguration<Museum.Domain.Museum>
    {
        public void Configure(EntityTypeBuilder<Museum.Domain.Museum> builder)
        {
            builder.HasKey(e => e.MuseumId);
            builder.Property(e => e.MuseumId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
            builder.Property(e => e.City).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Street).HasMaxLength(50).IsRequired();
            builder.Property(e => e.House).HasMaxLength(5).IsRequired();

            builder.HasOne(e => e.MuseumComplex)
                   .WithMany(c => c.Museums)
                   .HasForeignKey(e => e.MuseumComplexId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MuseumScheduleConfiguration : IEntityTypeConfiguration<MuseumSchedule>
    {
        public void Configure(EntityTypeBuilder<MuseumSchedule> builder)
        {
            builder.HasKey(e => e.MuseumScheduleId);
            builder.Property(e => e.MuseumScheduleId)
                   .UseIdentityColumn(10001, 1);

            builder.HasOne(e => e.Museum)
                   .WithMany(m => m.MuseumSchedules)
                   .HasForeignKey(e => e.MuseumId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class ScheduleDayConfiguration : IEntityTypeConfiguration<ScheduleDay>
    {
        public void Configure(EntityTypeBuilder<ScheduleDay> builder)
        {
            builder.HasKey(e => e.ScheduleDaysId);
            builder.Property(e => e.ScheduleDaysId)
                   .UseIdentityColumn(10001, 1);

            builder.HasOne(e => e.MuseumSchedule)
                   .WithMany(s => s.ScheduleDays)
                   .HasForeignKey(e => e.MuseumScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasCheckConstraint("CHK_ScheduleDays_WeekDay", "[WeekDay] BETWEEN 1 AND 7");
        }
    }
    public class ScheduleExceptionConfiguration : IEntityTypeConfiguration<ScheduleException>
    {
        public void Configure(EntityTypeBuilder<ScheduleException> builder)
        {
            builder.HasKey(e => e.ScheduleExceptionsId);
            builder.Property(e => e.ScheduleExceptionsId)
                   .UseIdentityColumn(10001, 1);

            builder.HasOne(e => e.MuseumSchedule)
                   .WithMany(s => s.ScheduleExceptions)
                   .HasForeignKey(e => e.MuseumScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class ExhibitionConfiguration : IEntityTypeConfiguration<Exhibition>
    {
        public void Configure(EntityTypeBuilder<Exhibition> builder)
        {
            builder.HasKey(e => e.ExhibitionId);
            builder.Property(e => e.ExhibitionId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Photo).HasMaxLength(255);
        }
    }
    public class MuseumExhibitionConfiguration : IEntityTypeConfiguration<MuseumExhibition>
    {
        public void Configure(EntityTypeBuilder<MuseumExhibition> builder)
        {
            builder.HasKey(e => e.MuseumExhibitionId);
            builder.Property(e => e.MuseumExhibitionId)
                   .UseIdentityColumn(10001, 1);

            builder.HasOne(e => e.Museum)
                   .WithMany(m => m.MuseumExhibitions)
                   .HasForeignKey(e => e.MuseumId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Exhibition)
                   .WithMany(x => x.MuseumExhibitions)
                   .HasForeignKey(e => e.ExhibitionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(e => e.TicketId);
            builder.Property(e => e.TicketId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Type).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Status).HasMaxLength(20).IsRequired();
            builder.HasCheckConstraint("CHK_Ticket_Status", "[Status] IN (N'Доступен', N'Продан', N'Отменён')");
            builder.Property(e => e.AvailableQuantity).IsRequired();
            builder.HasCheckConstraint("CHK_Ticket_Quantity", "[AvailableQuantity] >= 0");

            builder.HasOne(e => e.Exhibition)
                   .WithMany(x => x.Tickets)
                   .HasForeignKey(e => e.ExhibitionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class TicketPriceConfiguration : IEntityTypeConfiguration<TicketPrice>
    {
        public void Configure(EntityTypeBuilder<TicketPrice> builder)
        {
            builder.HasKey(e => e.TicketPriceId);
            builder.Property(e => e.TicketPriceId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Price).HasColumnType("money").IsRequired();
            builder.HasCheckConstraint("CHK_TicketPrice_Price", "[Price] >= 0");

            builder.HasOne(e => e.Ticket)
                   .WithMany(t => t.TicketPrices)
                   .HasForeignKey(e => e.TicketId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.MiddleName).HasMaxLength(50);
            builder.Property(e => e.Email).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Phone).HasMaxLength(20);
            builder.Property(e => e.Password).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Role).HasMaxLength(50).IsRequired();
            builder.HasCheckConstraint("CHK_User_Role", "[Role] IN (N'Гость', N'Посетитель', N'Администратор музея', N'Администратор системы')");
        }
    }
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.OrderId);
            builder.Property(e => e.OrderId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Status).HasMaxLength(50).IsRequired();
            builder.HasCheckConstraint("CHK_Order_Status", "[Status] IN (N'В ожидании', N'Оплачен', N'Отменён')");

            builder.HasOne(e => e.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(e => e.OrderItemId);
            builder.Property(e => e.OrderItemId)
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Quantity).IsRequired();
            builder.HasCheckConstraint("CHK_OrderItem_Quantity", "[Quantity] > 0");

            builder.HasOne(e => e.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(e => e.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Ticket)
                   .WithMany(t => t.OrderItems)
                   .HasForeignKey(e => e.TicketId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(e => e.PaymentId);
            builder.Property(e => e.PaymentId)      
                   .UseIdentityColumn(10001, 1);

            builder.Property(e => e.Amount).HasColumnType("money").IsRequired();
            builder.HasCheckConstraint("CHK_Payment_Amount", "[Amount] >= 0");

            builder.HasOne(e => e.Order)
                   .WithMany(o => o.Payments)
                   .HasForeignKey(e => e.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}