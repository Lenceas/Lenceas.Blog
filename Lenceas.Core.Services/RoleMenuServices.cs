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

        /// <summary>
        /// 根据角色ID集合获取角色菜单列表
        /// </summary>
        /// <param name="RoleIDs">角色ID集合</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<RoleMenu>> GetRoleMenuByRoleIDs(List<int> RoleIDs)
        {
            return await BaseDal.GetList(_ => RoleIDs.Contains(_.RoleID));
        }
    }
}