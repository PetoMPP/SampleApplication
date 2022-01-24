using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SampleApplication.Models;

namespace SampleApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SampleApplication.Models.EmployeeModel> EmployeeModel { get; set; }
        public DbSet<SampleApplication.Models.ServiceModel> ServiceModel { get; set; }
        public DbSet<SampleApplication.Models.EmployeeServiceModel> EmployeeServiceModel { get; set; }
    }
}
