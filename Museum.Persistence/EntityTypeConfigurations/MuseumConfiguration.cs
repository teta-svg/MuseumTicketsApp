using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuseumEntity = Museum.Domain.Museum;

public class MuseumConfiguration : IEntityTypeConfiguration<MuseumEntity>
{
    public void Configure(EntityTypeBuilder<MuseumEntity> builder)
    {
        builder.HasKey(e => e.MuseumId).HasName("PK__Museum__C10D28D294428B64");
        builder.HasOne(d => d.MuseumComplex).WithMany(p => p.Museums)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Museum_MuseumComplex");
    }
}
