using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;

namespace Lenceas.Core.Services
{
    public class RoleServices : BaseServices<Role>, IRoleServices
    {
        public RoleServices(IBaseRepository<Role> baseDal) : base(baseDal)
        {
        }
    }
}