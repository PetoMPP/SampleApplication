using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Models
{
    [Index(nameof(UserId), IsUnique = true)]
    public class EmployeeModel
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser IdentityUser{ get; set; }
        [Required]
        public string UserId { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 1)]
        public string LastName { get; set; }
    }
}
