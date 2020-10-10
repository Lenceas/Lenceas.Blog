using Models.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IAdministratorService
    {
        Task<List<Administrator>> GetAll();

        Task<Administrator> GetById(int Id);
    }
}
