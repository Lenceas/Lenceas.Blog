using AutoMapper;
using Lenceas.Core.Extensions;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Controllers
{
    /// <summary>
    /// 菜单接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [Authorize]
    public class MenuController : ControllerBase
    {
        #region 构造函数
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisBaseRepository _redis;
        private readonly IMapper _mapper;
        private static AuthModel _userRole;
        private readonly IMenuServices _menuServices;
        public MenuController(IHttpContextAccessor accessor, IRedisBaseRepository redis, IMenuServices menuServices, IMapper mapper)
        {
            _accessor = accessor;
            _redis = redis;
            _mapper = mapper;
            _userRole = new AuthHelper(accessor, redis).UserRoleCache;
            _menuServices = menuServices;
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 获取菜单结构树
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenuTree")]
        public async Task<ApiResult<IEnumerable<MenuTreeWebModel>>> GetMenuTree()
        {
            var r = new ApiResult<IEnumerable<MenuTreeWebModel>>();
            try
            {
                r.msg = "查询成功";
                r.data = await _menuServices.GetMenuTree(_userRole);
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取菜单分页
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet("GetPage")]
        public async Task<ApiResult<PageViewModel<MenuWebModel>>> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            var r = new ApiResult<PageViewModel<MenuWebModel>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<MenuWebModel>>(await _menuServices.GetPage(pageIndex, pageSize)).AsPageViewModel(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public async Task<ApiResult<List<MenuWebModel>>> GetList()
        {
            var r = new ApiResult<List<MenuWebModel>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<MenuWebModel>>(await _menuServices.GetList());
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResult<MenuWebModel>> GetById(int id)
        {
            var r = new ApiResult<MenuWebModel>();
            try
            {
                var entity = await _menuServices.GetById(id);
                if (entity != null)
                {
                    r.msg = "查询成功";
                    r.data = _mapper.Map<MenuWebModel>(entity);
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
        /// 添加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Add([FromBody] MenuEditWebModel model)
        {
            var r = new ApiResult<string>();
            var isNew = await _menuServices.GetEntity(t => t.MenuName.Equals(model.MenuName)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "菜单名称已存在";
                return r;
            }
            isNew = await _menuServices.GetEntity(t => t.MenuUrl.Equals(model.MenuUrl)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "菜单路由已存在";
                return r;
            }
            try
            {
                r.status = await _menuServices.AddAsync(new Menu(model.MenuPID, model.MenuName, model.MenuUrl, model.MenuIcon)) > 0 ? 200 : 400;
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
        /// 更新菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ApiResult<string>> Update(int id, [FromBody] MenuEditWebModel model)
        {
            var r = new ApiResult<string>();
            if (!id.Equals(model.Id))
            {
                r.status = 400;
                r.msg = "传入Id与实体Id不一致";
                return r;
            }
            var isExist = await _menuServices.IsExist(id);
            if (!isExist)
            {
                r.status = 404;
                r.msg = "未匹配到数据";
                return r;
            }
            var isNew = await _menuServices.GetEntity(t => t.MenuName.Equals(model.MenuName)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "菜单名称已存在";
                return r;
            }
            isNew = await _menuServices.GetEntity(t => t.MenuUrl.Equals(model.MenuUrl)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "菜单路由已存在";
                return r;
            }
            try
            {
                r.status = await _menuServices.UpdateAsync(t => t.Id == id, t => new Menu() { MenuPID = model.MenuPID, MenuName = model.MenuName, MenuUrl = model.MenuUrl, MenuIcon = string.IsNullOrEmpty(model.MenuIcon) ? "el-icon-s-home" : model.MenuIcon, MDate = DateTime.Now }) == 0 ? 200 : 400;
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
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResult<string>> Delete(int id)
        {
            var r = new ApiResult<string>();
            try
            {
                var isExist = await _menuServices.IsExist(id);
                if (isExist)
                {
                    r.status = await _menuServices.DeleteById(id) > 0 ? 200 : 400;
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
