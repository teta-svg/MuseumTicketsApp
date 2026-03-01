using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Museum.Domain;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAFC6F99E8E");
        builder.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
        builder.HasOne(d => d.User).WithMany(p => p.Orders)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Order_User");
    }
}
