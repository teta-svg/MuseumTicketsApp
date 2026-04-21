using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Museum.Domain.Entities;

[Table("MuseumSchedule")]
public partial class MuseumSchedule
{
    [Key]
    [Column("MuseumScheduleID")]
    public int MuseumScheduleId { get; set; }

    [Column("MuseumID")]
    public int MuseumId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public TimeOnly OpenTime { get; set; }

    public TimeOnly CloseTime { get; set; }

    [ForeignKey("MuseumId")]
    [InverseProperty("MuseumSchedules")]
    public virtual Museum Museum { get; set; } = null!;

    [InverseProperty("MuseumSchedule")]
    public virtual ICollection<ScheduleDay> ScheduleDays { get; set; } = new List<ScheduleDay>();

    [InverseProperty("MuseumSchedule")]
    public virtual ICollection<ScheduleException> ScheduleExceptions { get; set; } = new List<ScheduleException>();
}
