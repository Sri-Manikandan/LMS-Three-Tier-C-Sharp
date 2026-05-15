using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class BookCopy
{
    public int BookCopyId { get; set; }

    public int BookId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public CopyStatusEnum CopyStatus { get; set; }

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
