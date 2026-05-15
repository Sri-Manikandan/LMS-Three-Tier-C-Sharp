using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class MembershipType
{
    public int MembershipTypeId { get; set; }

    public int MaxActiveBorrowings { get; set; }

    public int MaxBorrowDays { get; set; }

    public MembershipTypeEnum MembershipTypeName { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
