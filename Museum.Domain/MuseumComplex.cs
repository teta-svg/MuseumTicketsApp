using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Museum.Domain;

[Table("MuseumComplex")]
public partial class MuseumComplex
{
    [Key]
    [Column("MuseumComplexID")]
    public int MuseumComplexId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [InverseProperty("MuseumComplex")]
    public virtual ICollection<Museum> Museums { get; set; } = new List<Museum>();
}
