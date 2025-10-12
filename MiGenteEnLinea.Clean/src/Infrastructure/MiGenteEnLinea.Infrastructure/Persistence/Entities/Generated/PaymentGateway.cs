using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("PaymentGateway")]
public partial class PaymentGateway
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("test")]
    public bool? Test { get; set; }

    [Column("productionURL")]
    [StringLength(150)]
    [Unicode(false)]
    public string? ProductionUrl { get; set; }

    [Column("testURL")]
    [StringLength(150)]
    [Unicode(false)]
    public string? TestUrl { get; set; }

    [Column("merchantID")]
    [StringLength(20)]
    [Unicode(false)]
    public string? MerchantId { get; set; }

    [Column("terminalID")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TerminalId { get; set; }
}
