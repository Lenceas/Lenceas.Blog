using System.ComponentModel;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 权限类型
    /// </summary>
    public enum EnumPermissionType
    {
        /// <summary>
        /// 所有
        /// </summary>
        [Description("所有")]
        所有 = 0,

        /// <summary>
        /// 增加
        /// </summary>
        [Description("增加")]
        增加 = 1,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        删除 = 2,

        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        修改 = 3,

        /// <summary>
        /// 查看
        /// </summary>
        [Description("查看")]
        查看 = 4
    }
}
