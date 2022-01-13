using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lenceas.Core.Services
{
    public class MenuServices : BaseServices<Menu>, IMenuServices
    {
        public MenuServices(IBaseRepository<Menu> baseDal) : base(baseDal)
        {
        }

        /// <summary>
        /// 获取菜单结构树
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MenuTreeWebModel>> GetMenuTree(AuthModel userRole)
        {
            var allMenus = await BaseDal.GetList();
            return this.RecursionMenus(allMenus);
        }

        /// <summary>
        /// 递归菜单层级
        /// </summary>
        /// <param name="allMenus">所有菜单</param>
        /// <param name="menuPID">父菜单ID</param>
        /// <returns></returns>
        private IEnumerable<MenuTreeWebModel> RecursionMenus(IEnumerable<Menu> allMenus, int menuPID = 0)
        {
            return allMenus.Where(_ => _.MenuPID == menuPID)
                .Select(p => new MenuTreeWebModel
                {
                    Id = p.Id,
                    CDate = p.CDate,
                    MDate = p.MDate,
                    Sort = p.Sort,
                    Remark = p.Remark,
                    MenuPID = p.MenuPID,
                    MenuName = p.MenuName,
                    MenuUrl = p.MenuUrl,
                    MenuIcon = p.MenuIcon,
                    SubMenuList = this.RecursionMenus(allMenus, p.Id),
                }).OrderBy(_ => _.Sort).ToList();
        }
    }
}
