using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17.Models;

[Table("Exhibition")]
public partial class Exhibition
{
    [Key]
    [Column("ExhibitionID")]
    public int ExhibitionId { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Photo { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [InverseProperty("Exhibition")]
    public virtual ICollection<MuseumExhibition> MuseumExhibitions { get; set; } = new List<MuseumExhibition>();

    [InverseProperty("Exhibition")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
