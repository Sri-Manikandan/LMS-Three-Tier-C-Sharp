using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string BookAuthor { get; set; } = null!;

    public int BookCategoryId { get; set; }

    public virtual BookCategory BookCategory { get; set; } = null!;

    public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();
}
