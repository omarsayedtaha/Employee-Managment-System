using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoPL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Display(Name ="Role Name")]
        public string RoleName { get; set; }

        public bool IsSelected { get; set; }

        public RoleViewModel()
        {
            Id=Guid.NewGuid().ToString();       
        }
    }
}
