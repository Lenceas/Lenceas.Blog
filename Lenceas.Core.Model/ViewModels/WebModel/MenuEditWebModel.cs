namespace Lenceas.Core.Model
{
    /// <summary>
    /// 菜单 EditWebModel
    /// </summary>
    public class MenuEditWebModel : BaseEditWebModel
    {
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public int MenuPID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单路由
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 菜单Icon
        /// </summary>
        public string MenuIcon { get; set; }
    }
}
