using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Models
{
    public class CreateEmployeeViewModel
    {
        [Required]
        public EmployeeModel Employee { get; set; }
        [Required]
        public SelectList UnassignedUsersWithEmployeeRole { get; set; }
    }
}
