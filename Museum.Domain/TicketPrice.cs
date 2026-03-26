using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Museum.Domain;

[Table("TicketPrice")]
public partial class TicketPrice
{
    [Key]
    [Column("TicketPriceID")]
    public int TicketPriceId { get; set; }

    [Column("TicketID")]
    public int TicketId { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    [ForeignKey("TicketId")]
    [InverseProperty("TicketPrices")]
    public virtual Ticket Ticket { get; set; } = null!;
}
