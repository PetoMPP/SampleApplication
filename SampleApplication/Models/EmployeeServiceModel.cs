using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Models
{
    public class EmployeeServiceModel
    {
        public int Id { get; set; }
        public EmployeeModel Employee { get; set; }
        public ServiceModel Service { get; set; }
        [NotMapped]
        public List<EmployeeModel> AllEmployees { get; set; }
        [NotMapped]
        public List<ServiceModel> AllServices { get; set; }
        [NotMapped]
        public EmployeeModel ActiveEmployee { get; set; }
    }
}
