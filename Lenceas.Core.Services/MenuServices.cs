using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;

namespace Lenceas.Core.Services
{
    public class MenuServices : BaseServices<Menu>, IMenuServices
    {
        private IBaseRepository<RoleMenu> _baseRoleMenuDal;

        public MenuServices(IBaseRepository<Menu> baseDal, IBaseRepository<RoleMenu> baseRoleMenuDal) : base(baseDal)
        {
            _baseRoleMenuDal = baseRoleMenuDal;
        }

        /// <summary>
        /// 获取有权限的菜单结构树
        /// </summary>
        /// <param name="userRole">用户身份缓存对象</param>
        /// <returns></returns>
        public async Task<IEnumerable<MenuTreeWebModel>> GetMenuTree(AuthModel userRole)
        {
            var result = new List<MenuTreeWebModel>();

            var roleMenus = await new RoleMenuServices(_baseRoleMenuDal).GetRoleMenuByRoleIDs(userRole.RoleIDs);
            if (roleMenus.Any())
            {
                var allMenus = await BaseDal.GetByIds(roleMenus.Select(_ => _.MenuID).Distinct().ToList());
                if (allMenus.Any())
                {
                    result = this.RecursionMenus(allMenus);
                }
            }

            return result;
        }

        /// <summary>
        /// 递归菜单层级
        /// </summary>
        /// <param name="allMenus">所有菜单</param>
        /// <param name="menuPID">父菜单ID</param>
        /// <returns></returns>
        private List<MenuTreeWebModel> RecursionMenus(IEnumerable<Menu> allMenus, int menuPID = 0)
        {
            var result = new List<MenuTreeWebModel>();
            if (allMenus.Any())
            {
                result = allMenus.Where(_ => _.MenuPID == menuPID)
                .Select(p => new MenuTreeWebModel
                {
                    Id = p.Id,
                    CDate = p.CDate,
                    MDate = p.MDate,
                    Sort = p.Sort,
                    Remark = p.Remark ?? string.Empty,
                    MenuPID = p.MenuPID,
                    MenuName = p.MenuName,
                    MenuUrl = p.MenuUrl,
                    MenuIcon = p.MenuIcon,
                    SubMenuList = this.RecursionMenus(allMenus, p.Id),
                }).OrderBy(_ => _.Sort).ToList();
            }
            return result;
        }
    }
}