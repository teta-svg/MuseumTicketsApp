using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Museum.Domain;

namespace Museum.Persistence.Configurations;

public class ExhibitionConfiguration : IEntityTypeConfiguration<Exhibition>
{
    public void Configure(EntityTypeBuilder<Exhibition> builder)
    {
        builder.HasKey(e => e.ExhibitionId).HasName("PK__Exhibiti__32CDCC7E11D9447A");
    }
}

public class MuseumComplexConfiguration : IEntityTypeConfiguration<MuseumComplex>
{
    public void Configure(EntityTypeBuilder<MuseumComplex> builder)
    {
        builder.HasKey(e => e.MuseumComplexId).HasName("PK__MuseumCo__A3CAC08F0770D3AD");
    }
}

public class MuseumScheduleConfiguration : IEntityTypeConfiguration<MuseumSchedule>
{
    public void Configure(EntityTypeBuilder<MuseumSchedule> builder)
    {
        builder.HasKey(e => e.MuseumScheduleId).HasName("PK__MuseumSc__73D1CB814906902D");

        builder.HasOne(d => d.Museum).WithMany(p => p.MuseumSchedules)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_MuseumSchedule_Museum");
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06A1628BAA09");

        builder.HasOne(d => d.Order).WithMany(p => p.OrderItems)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_OrderItem_Order");

        builder.HasOne(d => d.Ticket).WithMany(p => p.OrderItems)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_OrderItem_Ticket");
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A586B1F2D8A");
        builder.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

        builder.HasOne(d => d.Order).WithMany(p => p.Payments)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Payment_Order");
    }
}

public class ScheduleDayConfiguration : IEntityTypeConfiguration<ScheduleDay>
{
    public void Configure(EntityTypeBuilder<ScheduleDay> builder)
    {
        builder.HasKey(e => e.ScheduleDaysId).HasName("PK__Schedule__3931B0C8E716E7FB");

        builder.HasOne(d => d.MuseumSchedule).WithMany(p => p.ScheduleDays)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ScheduleDays_MuseumSchedule");
    }
}

public class ScheduleExceptionConfiguration : IEntityTypeConfiguration<ScheduleException>
{
    public void Configure(EntityTypeBuilder<ScheduleException> builder)
    {
        builder.HasKey(e => e.ScheduleExceptionsId).HasName("PK__Schedule__F7A1D11AFE31A9F7");

        builder.HasOne(d => d.MuseumSchedule).WithMany(p => p.ScheduleExceptions)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ScheduleExceptions_MuseumSchedule");
    }
}

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC627E81D9F5E");

        builder.HasOne(d => d.Exhibition).WithMany(p => p.Tickets)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Ticket_Exhibition");
    }
}

public class TicketPriceConfiguration : IEntityTypeConfiguration<TicketPrice>
{
    public void Configure(EntityTypeBuilder<TicketPrice> builder)
    {
        builder.HasKey(e => e.TicketPriceId).HasName("PK__TicketPr__BE7DED9CB222B746");

        builder.HasOne(d => d.Ticket).WithMany(p => p.TicketPrices)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TicketPrice_Ticket");
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.UserId).HasName("PK__User__1788CCAC6180EAAA");
    }
}
