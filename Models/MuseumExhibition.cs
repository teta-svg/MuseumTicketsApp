using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17.Models;

[Table("MuseumExhibition")]
public partial class MuseumExhibition
{
    [Key]
    [Column("MuseumExhibitionID")]
    public int MuseumExhibitionId { get; set; }

    [Column("MuseumID")]
    public int MuseumId { get; set; }

    [Column("ExhibitionID")]
    public int ExhibitionId { get; set; }

    [ForeignKey("ExhibitionId")]
    [InverseProperty("MuseumExhibitions")]
    public virtual Exhibition Exhibition { get; set; } = null!;

    [ForeignKey("MuseumId")]
    [InverseProperty("MuseumExhibitions")]
    public virtual Museum Museum { get; set; } = null!;
}
