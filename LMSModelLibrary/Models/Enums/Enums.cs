namespace LMSModelLibrary.Models
{
    public enum MembershipTypeEnum
    {
        Basic,
        Premium,
        Student
    }

    public enum CopyStatusEnum
    {
        available,
        borrowed,
        unavailable,
        damaged
    }

    public enum BorrowingStatusEnum
    {
        active,
        returned
    }

    public enum FineStatusEnum
    {
        unpaid,
        paid,
        waived
    }
}
