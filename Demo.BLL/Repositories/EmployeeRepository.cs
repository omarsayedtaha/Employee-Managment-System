using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        
        public EmployeeRepository(MVCAppG02DbContext dbContext):base(dbContext)
        {
            
        } 
     
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> SearchEmployeeByName(string name)
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower()));

    }
}
