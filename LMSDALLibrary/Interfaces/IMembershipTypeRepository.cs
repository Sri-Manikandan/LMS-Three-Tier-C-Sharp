using LMSModelLibrary.Models;

namespace LMSDALLibrary.Interfaces
{
    public interface IMembershipTypeRepository : IRepository<MembershipType>
    {
        MembershipType? GetByTypeName(MembershipTypeEnum typeName);
    }
}
