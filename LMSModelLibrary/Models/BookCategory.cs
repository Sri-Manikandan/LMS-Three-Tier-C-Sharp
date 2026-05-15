using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class BookCategory
{
    public int BookCategoryId { get; set; }

    public string BookCategoryName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
