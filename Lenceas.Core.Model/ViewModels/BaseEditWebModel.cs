namespace Lenceas.Core.Model
{
    /// <summary>
    /// 基础 EditWebModel
    /// </summary>
    public class BaseEditWebModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime MDate { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}