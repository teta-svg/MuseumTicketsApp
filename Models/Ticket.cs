using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17.Models;

[Table("Ticket")]
[Index("ExhibitionId", "Type", Name = "UQ_Ticket_Exhibition_Type", IsUnique = true)]
public partial class Ticket
{
    [Key]
    [Column("TicketID")]
    public int TicketId { get; set; }

    [Column("ExhibitionID")]
    public int ExhibitionId { get; set; }

    [StringLength(50)]
    public string Type { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public int AvailableQuantity { get; set; }

    [ForeignKey("ExhibitionId")]
    [InverseProperty("Tickets")]
    public virtual Exhibition Exhibition { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketPrice> TicketPrices { get; set; } = new List<TicketPrice>();
}
