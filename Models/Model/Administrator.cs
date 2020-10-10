using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Model
{
    /// <summary>
    /// 管理员类
    /// </summary>
    public class Administrator : BaseModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPwd { get; set; }
    }
}
