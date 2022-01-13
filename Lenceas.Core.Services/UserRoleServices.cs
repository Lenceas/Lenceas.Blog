using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;

namespace Lenceas.Core.Services
{
    public class UserRoleServices : BaseServices<UserRole>, IUserRoleServices
    {
        public UserRoleServices(IBaseRepository<UserRole> baseDal) : base(baseDal)
        {
        }
    }
}
