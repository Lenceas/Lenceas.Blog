using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 菜单表
    /// </summary>
    public class Menu : BaseEntity
    {
        #region 构造函数
        public Menu()
        {

        }

        public Menu(int menuPID, string menuName, string menuUrl, string menuIcon)
        {
            MenuPID = menuPID;
            MenuName = menuName;
            MenuUrl = menuUrl;
            MenuIcon = string.IsNullOrEmpty(menuIcon) ? "el-icon-s-home" : menuIcon;
        }
        #endregion

        /// <summary>
        /// 父菜单ID
        /// </summary>
        [Description("父菜单ID")]
        [Required]
        public int MenuPID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        [Description("菜单名称")]
        [Required]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单路由
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        [Description("菜单路由")]
        [Required]
        public string MenuUrl { get; set; }

        /// <summary>
        /// 菜单Icon
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        [Description("菜单Icon")]
        public string MenuIcon { get; set; }
    }
}
