using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 博客文章表
    /// </summary>
    public class BlogArticle : BaseEntity
    {
        #region 构造函数
        public BlogArticle()
        {
            Title = string.Empty;
            SubTitle = string.Empty;
            Intro = string.Empty;
            Thumbnail = string.Empty;
            Content = string.Empty;
            Category = string.Empty;
            Author = string.Empty;
            TagIDs = string.Empty;
            Views = 0;
            CommentNum = 0;
            IsHot = false;
            IsHome = false;
        }
        #endregion

        /// <summary>
        /// 标题
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [Description("标题")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        [Description("副标题")]
        public string? SubTitle { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        [Description("简介")]
        [Required]
        public string? Intro { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        [Description("缩略图")]
        public string? Thumbnail { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column(TypeName = "varchar(2000)")]
        [Description("内容")]
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [Description("类别")]
        public string? Category { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [Description("作者")]
        public string? Author { get; set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        [Description("标签集合")]
        public string? TagIDs { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        [Description("访问量")]
        public int Views { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        [Description("评论数")]
        public int CommentNum { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [Description("是否推荐")]
        public bool IsHot { get; set; }

        /// <summary>
        /// 是否首页
        /// </summary>
        [Description("是否首页")]
        public bool IsHome { get; set; }
    }
}