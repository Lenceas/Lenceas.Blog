using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;

namespace Lenceas.Core.Services
{
    public class RoleMenuServices : BaseServices<RoleMenu>, IRoleMenuServices
    {
        public RoleMenuServices(IBaseRepository<RoleMenu> baseDal) : base(baseDal)
        {
        }
    }
}
