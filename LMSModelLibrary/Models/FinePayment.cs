using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class FinePayment
{
    public int PaymentId { get; set; }

    public int FineId { get; set; }

    public int MemberId { get; set; }

    public decimal FinePaidAmount { get; set; }

    public DateTime PaidDate { get; set; }

    public virtual Fine Fine { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
