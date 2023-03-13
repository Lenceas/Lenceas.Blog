using AutoMapper;
using Lenceas.Core.Common;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Api.Controllers
{
    /// <summary>
    /// 博客文章接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [Authorize]
    public class BlogArticleController : ControllerBase
    {
        #region 构造函数
        private readonly IBlogArticleServices _blogArticleServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _cache;
        public BlogArticleController(IBlogArticleServices blogArticleServices, IMapper mapper, IHttpContextAccessor accessor, IMemoryCache memoryCache, IDistributedCache cache)
        {
            _blogArticleServices = blogArticleServices;
            _mapper = mapper;
            _accessor = accessor;
            _memoryCache = memoryCache;
            _cache = cache;
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 获取博客文章分页
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet("GetPage")]
        [AllowAnonymous]
        public async Task<ApiResult<PageViewModel<BlogArticleWebModel>>> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            var r = new ApiResult<PageViewModel<BlogArticleWebModel>>();
            try
            {
                r.Msg = "查询成功";
                r.Data = _mapper.Map<List<BlogArticleWebModel>>(await _blogArticleServices.GetPage(pageIndex, pageSize)).AsPageViewModel(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取博客文章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BlogArticleWebModel>>> GetList()
        {
            var r = new ApiResult<List<BlogArticleWebModel>>();
            try
            {
                r.Msg = "查询成功";
                r.Data = _mapper.Map<List<BlogArticleWebModel>>(await _blogArticleServices.GetList());
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取博客文章详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ApiResult<BlogArticleWebModel>> GetById(int id)
        {
            var r = new ApiResult<BlogArticleWebModel>();
            try
            {
                var entity = await _blogArticleServices.GetById(id);
                if (entity != null)
                {
                    r.Msg = "查询成功";
                    r.Data = _mapper.Map<BlogArticleWebModel>(entity);
                }
                else
                {
                    r.Status = 404;
                    r.Msg = "未匹配到数据";
                }
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 添加博客文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Add([FromBody] BlogArticleEditWebModel model)
        {
            var r = new ApiResult<string>();
            try
            {
                r.Status = await _blogArticleServices.AddAsync(new BlogArticle() { }) > 0 ? 200 : 400;
                r.Msg = r.Status == 200 ? "添加成功" : "添加失败";
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 更新博客文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ApiResult<string>> Update(int id, [FromBody] BlogArticleEditWebModel model)
        {
            var r = new ApiResult<string>();
            if (!id.Equals(model.Id))
            {
                r.Status = 400;
                r.Msg = "传入Id与实体Id不一致";
                return r;
            }
            var isExist = await _blogArticleServices.IsExist(id);
            if (!isExist)
            {
                r.Status = 404;
                r.Msg = "未匹配到数据";
                return r;
            }
            try
            {
                r.Status = await _blogArticleServices.UpdateAsync(t => t.Id == id, t => new BlogArticle() { MDate = DateTime.Now.ToLocalTime() }) == 0 ? 200 : 400;
                r.Msg = r.Status == 200 ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 删除博客文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResult<string>> Delete(int id)
        {
            var r = new ApiResult<string>();
            try
            {
                var isExist = await _blogArticleServices.IsExist(id);
                if (isExist)
                {
                    r.Status = await _blogArticleServices.DeleteById(id) > 0 ? 200 : 400;
                    r.Msg = r.Status == 200 ? "删除成功" : "删除失败";
                }
                else
                {
                    r.Status = 400;
                    r.Msg = "未匹配到数据";
                }
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }
        #endregion
    }
}