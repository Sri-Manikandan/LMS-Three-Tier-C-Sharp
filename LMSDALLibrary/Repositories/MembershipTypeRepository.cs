using LMSModelLibrary.Models;
using LMSDALLibrary.Interfaces;

namespace LMSDALLibrary.Repositories
{
    public class MembershipTypeRepository : AbstractRepository<MembershipType>, IMembershipTypeRepository
    {
        public MembershipType? GetByTypeName(MembershipTypeEnum typeName)
        {
            return context.MembershipTypes
                .FirstOrDefault(m => m.MembershipTypeName == typeName);
        }
    }
}
