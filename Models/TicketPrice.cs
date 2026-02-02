using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp17.Models;

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

    [ForeignKey("TicketId")]
    [InverseProperty("TicketPrices")]
    public virtual Ticket Ticket { get; set; } = null!;
}
