using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public  class GenericRepository<T> : IGenericRepositroy<T> where T : class 
    {
        private protected readonly MVCAppG02DbContext _dbContext;

        public GenericRepository(MVCAppG02DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(T item)
        {
           await _dbContext.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }

        public async Task<T> Get(int id)
            => await _dbContext.Set<T>().FindAsync(id);
        

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
              return (IEnumerable<T>) await _dbContext.Employees.Include(E => E .Department).ToListAsync();
            else
                return _dbContext.Set<T>().ToList();


        }
        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }
    }
}
