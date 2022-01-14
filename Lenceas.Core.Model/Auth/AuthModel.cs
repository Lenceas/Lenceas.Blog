using System;
using System.Collections.Generic;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 用户身份缓存对象
    /// </summary>
    [Serializable]
    public class AuthModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户角色ID集合
        /// </summary>
        public List<int> RoleIDs { get; set; }
    }
}
