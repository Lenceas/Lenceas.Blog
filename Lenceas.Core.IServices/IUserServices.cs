using Lenceas.Core.Model;
using System.Threading.Tasks;

namespace Lenceas.Core.IServices
{
    public interface IUserServices : IBaseServices<User>
    {
        Task<User> Register(string name, string pwd);

        Task<bool> ExistName(string name);
    }
}