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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly MVCAppG02DbContext _dbContext;

        public DepartmentRepository(MVCAppG02DbContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }
     
   
    }
}
