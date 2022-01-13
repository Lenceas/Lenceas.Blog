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

namespace Lenceas.Core.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [Authorize]
    public class UserController : ControllerBase
    {
        #region 构造函数
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _cache;
        public UserController(IUserServices userServices, IMapper mapper, IHttpContextAccessor accessor, IMemoryCache memoryCache, IDistributedCache cache)
        {
            _userServices = userServices;
            _mapper = mapper;
            _accessor = accessor;
            _memoryCache = memoryCache;
            _cache = cache;
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 获取用户分页
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet("GetPage")]
        public async Task<ApiResult<PageViewModel<UserWebModel>>> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            var r = new ApiResult<PageViewModel<UserWebModel>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<UserWebModel>>(await _userServices.GetPage(pageIndex, pageSize)).AsPageViewModel(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public async Task<ApiResult<List<UserWebModel>>> GetList()
        {
            var r = new ApiResult<List<UserWebModel>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<UserWebModel>>(await _userServices.GetList());
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResult<UserWebModel>> GetById(int id)
        {
            var r = new ApiResult<UserWebModel>();
            try
            {
                var entity = await _userServices.GetById(id);
                if (entity != null)
                {
                    r.msg = "查询成功";
                    r.data = _mapper.Map<UserWebModel>(entity);
                }
                else
                {
                    r.status = 404;
                    r.msg = "未匹配到数据";
                }
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Add([FromBody] UserEditWebModel model)
        {
            var r = new ApiResult<string>();
            var isNew = await _userServices.GetEntity(t => t.UserName.Equals(model.UserName)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "账号已存在";
                return r;
            }
            try
            {
                r.status = await _userServices.AddAsync(new User(model.UserName, MD5Helper.MD5Encrypt32(model.Password))) > 0 ? 200 : 400;
                r.msg = r.status == 200 ? "添加成功" : "添加失败";
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ApiResult<string>> Update(int id, [FromBody] UserEditWebModel model)
        {
            var r = new ApiResult<string>();
            if (!id.Equals(model.Id))
            {
                r.status = 400;
                r.msg = "传入Id与实体Id不一致";
                return r;
            }
            var isExist = await _userServices.IsExist(id);
            if (!isExist)
            {
                r.status = 404;
                r.msg = "未匹配到数据";
                return r;
            }
            var isNew = await _userServices.GetEntity(t => t.UserName.Equals(model.UserName)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "账号已存在";
                return r;
            }
            try
            {
                r.status = await _userServices.UpdateAsync(t => t.Id == id, t => new User() { UserName = model.UserName, Password = MD5Helper.MD5Encrypt32(model.Password), MDate = DateTime.Now.ToLocalTime() }) == 0 ? 200 : 400;
                r.msg = r.status == 200 ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResult<string>> Delete(int id)
        {
            var r = new ApiResult<string>();
            try
            {
                var isExist = await _userServices.IsExist(id);
                if (isExist)
                {
                    r.status = await _userServices.DeleteById(id) > 0 ? 200 : 400;
                    r.msg = r.status == 200 ? "删除成功" : "删除失败";
                }
                else
                {
                    r.status = 400;
                    r.msg = "未匹配到数据";
                }
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }
        #endregion
    }
}
