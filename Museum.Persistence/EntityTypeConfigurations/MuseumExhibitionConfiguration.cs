using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Museum.Domain;

public class MuseumExhibitionConfiguration : IEntityTypeConfiguration<MuseumExhibition>
{
    public void Configure(EntityTypeBuilder<MuseumExhibition> builder)
    {
        builder.HasKey(e => e.MuseumExhibitionId).HasName("PK__MuseumEx__8C9BE304C8B2B2E5");
        builder.HasOne(d => d.Exhibition).WithMany(p => p.MuseumExhibitions)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_MuseumExhibition_Exhibition");
        builder.HasOne(d => d.Museum).WithMany(p => p.MuseumExhibitions)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_MuseumExhibition_Museum");
    }
}
