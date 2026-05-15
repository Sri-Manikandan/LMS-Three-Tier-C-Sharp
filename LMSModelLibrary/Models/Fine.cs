using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class Fine
{
    public int FineId { get; set; }

    public int BorrowingId { get; set; }

    public decimal FineAmount { get; set; }

    public virtual Borrowing Borrowing { get; set; } = null!;

    public FineStatusEnum FinePaidStatus { get; set; }

    public virtual ICollection<FinePayment> FinePayments { get; set; } = new List<FinePayment>();
}
