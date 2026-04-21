using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Museum.Domain.Entities;

[Table("ScheduleExceptions")]
public partial class ScheduleException
{
    [Key]
    [Column("ScheduleExceptionsID")]
    public int ScheduleExceptionsId { get; set; }

    [Column("MuseumScheduleID")]
    public int MuseumScheduleId { get; set; }

    public DateOnly ExceptionDate { get; set; }

    public bool IsOpen { get; set; }

    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }

    [ForeignKey("MuseumScheduleId")]
    [InverseProperty("ScheduleExceptions")]
    public virtual MuseumSchedule MuseumSchedule { get; set; } = null!;
}