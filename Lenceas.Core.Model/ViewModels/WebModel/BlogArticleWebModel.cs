namespace Lenceas.Core.Model
{
    /// <summary>
    /// 博客文章 WebModel
    /// </summary>
    public class BlogArticleWebModel : BaseWebModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentNum { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsHot { get; set; }

        /// <summary>
        /// 是否首页
        /// </summary>
        public bool IsHome { get; set; }
    }
}
