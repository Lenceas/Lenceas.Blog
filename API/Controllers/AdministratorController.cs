using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Model;

namespace API.Controllers
{
    /// <summary>
    /// 管理员
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly ILogger<AdministratorController> _logger;
        private readonly IAdministratorService _administratorService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="administratorService"></param>

        public AdministratorController(ILogger<AdministratorController> logger, IAdministratorService administratorService)
        {
            _logger = logger;
            _administratorService = administratorService;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        // GET: api/<AdministratorController>
        [HttpGet]
        public async Task<List<Administrator>> GetAll()
        {
            return await _administratorService.GetAll();
        }

        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        // GET api/<AdministratorController>/5
        [HttpGet("{id}")]
        public async Task<Administrator> GetById(int id)
        {
            return await _administratorService.GetById(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="value"></param>
        // POST api/<AdministratorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/<AdministratorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<AdministratorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
