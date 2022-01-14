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
        private IBaseRepository<UserRole> _baseUserRoleDal;
        public UserServices(IBaseRepository<User> baseDal, IBaseRepository<UserRole> baseUserRoleDal) : base(baseDal)
        {
            _baseUserRoleDal = baseUserRoleDal;
        }

        public async Task<User> Register(string name, string pwd)
        {
            var en = new User(name, MD5Helper.MD5Encrypt32(pwd));
            await BaseDal.AddAsync(en);
            en = await BaseDal.GetEntity(c => c.UserName.Equals(name));
            // 新注册用户默认绑定 普通用户角色
            var userRole = new UserRole() { UserID = en.Id, RoleID = 3 };
            await new UserRoleServices(_baseUserRoleDal).AddAsync(userRole);
            return en;
        }

        public async Task<bool> ExistName(string name)
        {
            return await BaseDal.GetEntity(c => c.UserName.Equals(name)) != null;
        }
    }
}