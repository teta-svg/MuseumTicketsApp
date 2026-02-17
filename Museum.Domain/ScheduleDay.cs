using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17.Models;

public partial class ScheduleDay
{
    [Key]
    [Column("ScheduleDaysID")]
    public int ScheduleDaysId { get; set; }

    [Column("MuseumScheduleID")]
    public int MuseumScheduleId { get; set; }

    public int WeekDay { get; set; }

    [ForeignKey("MuseumScheduleId")]
    [InverseProperty("ScheduleDays")]
    public virtual MuseumSchedule MuseumSchedule { get; set; } = null!;
}
