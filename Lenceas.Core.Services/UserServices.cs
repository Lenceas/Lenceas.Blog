using Lenceas.Core.Common;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;
using System.Threading.Tasks;
using System;

namespace Lenceas.Core.Services
{
    public class UserServices : BaseServices<User>, IUserServices
    {
        public UserServices(IBaseRepository<User> baseDal) : base(baseDal)
        {
        }

        public async Task<User> Register(string name, string pwd)
        {
            var en = new User()
            {
                UserName = name,
                Password = MD5Helper.MD5Encrypt32(pwd),
                CDate = DateTime.Now,
                MDate = DateTime.Now,
                Email = string.Empty,
            };
            await BaseDal.AddAsync(en);
            return await BaseDal.GetEntity(c => c.UserName.Equals(name));
        }

        public async Task<bool> ExistName(string name)
        {
            return await BaseDal.GetEntity(c => c.UserName.Equals(name)) != null;
        }
    }
}