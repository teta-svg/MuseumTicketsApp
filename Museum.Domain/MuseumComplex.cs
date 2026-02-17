using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17.Models;

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
