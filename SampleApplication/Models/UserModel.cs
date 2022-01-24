using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Models
{
    public class UserModel
    {
        public string UserDbId { get; set; }
        public string Name { get; set; }
        public List<string> CurrentRoles { get; set; }
        public List<string> AvailableRoles { get; set; }
        public string DefaultPassword { get; set; }
        public UserModel()
        {
            DefaultPassword = Startup.DefaultPassword;
        }
    }
}
