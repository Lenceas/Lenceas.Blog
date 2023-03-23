using AutoMapper;
using Lenceas.Core.Extensions;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Api.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        #region 构造函数
        private readonly ITestServices _testServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _cache;
        private readonly IRedisBaseRepository _redis;

        public TestController(ITestServices testServices, IMapper mapper, IHttpContextAccessor accessor, IMemoryCache memoryCache, IDistributedCache cache, IRedisBaseRepository redis)
        {
            _testServices = testServices;
            _mapper = mapper;
            _accessor = accessor;
            _memoryCache = memoryCache;
            _cache = cache;
            _redis = redis;
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 获取测试信息分页
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet("GetPage")]
        public async Task<ApiResult<PageViewModel<TestWebModel>>> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            var r = new ApiResult<PageViewModel<TestWebModel>>();
            try
            {
                r.Msg = "查询成功";
                r.Data = _mapper.Map<List<TestWebModel>>(await _testServices.GetPage(pageIndex, pageSize)).AsPageViewModel(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取测试信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public async Task<ApiResult<List<TestWebModel>>> GetList()
        {
            var r = new ApiResult<List<TestWebModel>>();
            try
            {
                r.Msg = "查询成功";
                r.Data = _mapper.Map<List<TestWebModel>>(await _testServices.GetList());
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取测试信息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResult<TestWebModel>> GetById(int id)
        {
            var r = new ApiResult<TestWebModel>();
            try
            {
                var entity = await _testServices.GetById(id);
                if (entity != null)
                {
                    r.Msg = "查询成功";
                    r.Data = _mapper.Map<TestWebModel>(entity);
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
        /// 添加测试信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Add([FromBody] TestEditWebModel model)
        {
            var r = new ApiResult<string>();
            try
            {
                r.Status = await _testServices.AddAsync(new Test(model.Name)) > 0 ? 200 : 400;
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
        /// 更新测试信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ApiResult<string>> Update(int id, [FromBody] TestEditWebModel model)
        {
            var r = new ApiResult<string>();
            if (!id.Equals(model.Id))
            {
                r.Status = 400;
                r.Msg = "传入Id与实体Id不一致";
                return r;
            }
            var isExist = await _testServices.IsExist(id);
            if (!isExist)
            {
                r.Status = 404;
                r.Msg = "未匹配到数据";
                return r;
            }
            try
            {
                r.Status = await _testServices.UpdateAsync(t => t.Id == id, t => new Test() { Name = model.Name, MDate = DateTime.Now.ToLocalTime() }) == 0 ? 200 : 400;
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
        /// 删除测试信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResult<string>> Delete(int id)
        {
            var r = new ApiResult<string>();
            try
            {
                var isExist = await _testServices.IsExist(id);
                if (isExist)
                {
                    r.Status = await _testServices.DeleteById(id) > 0 ? 200 : 400;
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

        /// <summary>
        /// 获取 MiniProfiler Html
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMiniProfilerHtml")]
        public ApiResult<string> GetMiniProfilerHtml()
        {
            var r = new ApiResult<string>();
            try
            {
                r.Status = 200;
                r.Msg = "查询成功";
                r.Data = MiniProfiler.Current.RenderIncludes(_accessor.HttpContext).ToString();
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 测试-MemoryCache
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestMemoryCache")]
        public ApiResult<string> TestMemoryCache()
        {
            var r = new ApiResult<string>();
            try
            {
                r.Status = 200;
                r.Msg = "查询成功";
                string key = "Test-MemoryCache";
                if (_memoryCache.TryGetValue(key, out string time))
                {
                    r.Data = time;
                }
                else
                {
                    r.Data = time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                    _memoryCache.Set(key, time, TimeSpan.FromSeconds(5));
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
        /// 测试-DistributedCache
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestDistributedCache")]
        public ApiResult<string> TestDistributedCache()
        {
            var r = new ApiResult<string>();
            try
            {
                r.Status = 200;
                r.Msg = "查询成功";
                string key = "Test-Redis";
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                if (!string.IsNullOrEmpty(_cache.GetString(key)))
                {
                    time = _cache.GetString(key);
                }
                else
                {
                    _cache.SetString(key, time, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5) });
                }
                r.Data = time;
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 测试-Redis
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestRedis")]
        public async Task<ApiResult<TokenInfoViewModel>> TestRedis()
        {
            var r = new ApiResult<TokenInfoViewModel>();
            try
            {
                await _redis.SetValueAsync("Blog-Token", new TokenInfoViewModel() { Token = "x", Expires_In = (int)TimeSpan.FromMinutes(15).TotalSeconds, Token_Type = "Bearer" }, 15);
                var redisKeyValue = await _redis.GetAsync<TokenInfoViewModel>("Blog-Token");
                for (int i = 0; i < 100; i++)
                {
                    await _redis.ListLeftPushAsync("List", $"{i}");
                }
                if (redisKeyValue != null)
                {
                    r.Status = 200;
                    r.Msg = "成功";
                    r.Data = redisKeyValue;
                }
                else
                {
                    r.Status = 400;
                    r.Msg = "失败";
                }
            }
            catch (Exception ex)
            {
                r.Status = 500;
                r.Msg = ex.Message;
            }
            return r;
        }
    }
}
