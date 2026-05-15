using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class Borrowing
{
    public int BorrowingId { get; set; }

    public int MemberId { get; set; }

    public int BookCopyId { get; set; }

    public DateOnly BorrowDate { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }
    
    public BorrowingStatusEnum BorrowingStatus { get; set; }

    public virtual BookCopy BookCopy { get; set; } = null!;

    public virtual Fine? Fine { get; set; }

    public virtual Member Member { get; set; } = null!;
}
