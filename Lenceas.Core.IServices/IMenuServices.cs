using Lenceas.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lenceas.Core.IServices
{
    public interface IMenuServices : IBaseServices<Menu>
    {
        Task<IEnumerable<MenuTreeWebModel>> GetMenuTree(AuthModel userRole);
    }
}