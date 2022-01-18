using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;

namespace Lenceas.Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        public BlogArticleServices(IBaseRepository<BlogArticle> baseDal) : base(baseDal)
        {
        }
    }
}