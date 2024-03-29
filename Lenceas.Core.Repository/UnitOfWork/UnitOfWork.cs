﻿using Lenceas.Core.Model;

namespace Lenceas.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MySqlContext _context;

        public UnitOfWork(MySqlContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}