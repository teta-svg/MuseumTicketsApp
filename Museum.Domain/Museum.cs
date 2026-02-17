using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17.Models;

[Table("Museum")]
public partial class Museum
{
    [Key]
    [Column("MuseumID")]
    public int MuseumId { get; set; }

    [Column("MuseumComplexID")]
    public int MuseumComplexId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(50)]
    public string Street { get; set; } = null!;

    [StringLength(5)]
    public string House { get; set; } = null!;

    [ForeignKey("MuseumComplexId")]
    [InverseProperty("Museums")]
    public virtual MuseumComplex MuseumComplex { get; set; } = null!;

    [InverseProperty("Museum")]
    public virtual ICollection<MuseumExhibition> MuseumExhibitions { get; set; } = new List<MuseumExhibition>();

    [InverseProperty("Museum")]
    public virtual ICollection<MuseumSchedule> MuseumSchedules { get; set; } = new List<MuseumSchedule>();
}
