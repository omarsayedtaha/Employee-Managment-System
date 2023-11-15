using Demo.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepositroy<Employee>
    {
       IQueryable<Employee> GetEmployeesByAddress(string address);

        IEnumerable<Employee> SearchEmployeeByName(string name); 
    }
}
