using Data;
using IServices;
using Microsoft.EntityFrameworkCore;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly MySqlDbContext _context;

        public AdministratorService(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task<List<Administrator>> GetAll()
        {
            return await _context.administrators.ToListAsync();
        }

        public async Task<Administrator> GetById(int Id)
        {
            return await _context.administrators.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
