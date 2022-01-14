using Lenceas.Core.Model;

namespace Lenceas.Core.IServices
{
    public interface IRoleMenuServices : IBaseServices<RoleMenu>
    {
        /// <summary>
        /// 根据角色ID集合获取角色菜单列表
        /// </summary>
        /// <param name="RoleIDs">角色ID集合</param>
        /// <returns></returns>
        Task<IEnumerable<RoleMenu>> GetRoleMenuByRoleIDs(List<int> RoleIDs);
    }
}