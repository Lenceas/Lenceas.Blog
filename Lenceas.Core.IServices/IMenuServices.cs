using Lenceas.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lenceas.Core.IServices
{
    public interface IMenuServices : IBaseServices<Menu>
    {
        /// <summary>
        /// 获取有权限的菜单结构树
        /// </summary>
        /// <param name="userRole">用户身份缓存对象</param>
        /// <returns></returns>
        Task<IEnumerable<MenuTreeWebModel>> GetMenuTree(AuthModel userRole);
    }
}