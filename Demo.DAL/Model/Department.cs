using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Model
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code is Required !!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required !!")]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime DateofCreation { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
