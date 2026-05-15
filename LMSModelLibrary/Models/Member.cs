using System;
using System.Collections.Generic;

namespace LMSModelLibrary.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string MemberName { get; set; } = null!;

    public string MemberEmail { get; set; } = null!;

    public string MemberPhone { get; set; } = null!;

    public int MembershipTypeId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    public virtual ICollection<FinePayment> FinePayments { get; set; } = new List<FinePayment>();

    public virtual MembershipType MembershipType { get; set; } = null!;
}
