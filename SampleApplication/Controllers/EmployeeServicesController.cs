using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SampleApplication.Data;
using SampleApplication.Models;

namespace SampleApplication.Controllers
{
    public class EmployeeServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeServices
        public async Task<IActionResult> Index()
        {
            List<EmployeeServiceModel> models = await _context.EmployeeServiceModel.ToListAsync();
            List<EmployeeModel> employees = await _context.EmployeeModel.ToListAsync();

            foreach (EmployeeModel e in employees)
            {
                IdentityUser user = await _context.Users.FindAsync(e.UserId);
                e.UserName = user.UserName;
            }
            //I have to include getting all ServiceModels to populate EmployeeServiceModels
            _ = await _context.ServiceModel.ToListAsync();
            foreach (EmployeeServiceModel es in models)
            {
                es.AllEmployees = employees;
            }

            if (models.Count() == 0)
            {
                EmployeeServiceModel dummyModel = new EmployeeServiceModel
                {
                    AllEmployees = employees
                };
                models.Add(dummyModel);
            }
            return View(models);
        }

        // GET: EmployeeServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeServiceModel = await _context.EmployeeServiceModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeServiceModel == null)
            {
                return NotFound();
            }

            return View(employeeServiceModel);
        }

        // GET: EmployeeServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: EmployeeServices/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<EmployeeServiceModel> models = await _context.EmployeeServiceModel.ToListAsync();
            List<EmployeeModel> employees = await _context.EmployeeModel.ToListAsync();
            List<ServiceModel> services = await _context.ServiceModel.ToListAsync();
            EmployeeModel activeEmployee = await _context.EmployeeModel.FindAsync(int.Parse(id));
            
            foreach (EmployeeModel e in employees)
            {
                IdentityUser user = await _context.Users.FindAsync(e.UserId);
                e.UserName = user.UserName;
            }
            foreach (EmployeeServiceModel es in models)
            {
                es.AllEmployees = employees;
                es.AllServices = services;
                es.ActiveEmployee = activeEmployee;
            }

            if (models.Count() == 0)
            {
                EmployeeServiceModel dummyModel = new EmployeeServiceModel
                {
                    AllEmployees = employees,
                    AllServices = services,
                    ActiveEmployee = activeEmployee
                };
                models.Add(dummyModel);
            }
            return View(models);
        }
        public async Task<IActionResult> Add(string userId, string serviceId)
        {
            EmployeeServiceModel model = new EmployeeServiceModel
            {
                Employee = await _context.EmployeeModel.FindAsync(int.Parse(userId)),
                Service = await _context.ServiceModel.FindAsync(int.Parse(serviceId))
            };
            _context.EmployeeServiceModel.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = userId});
        }
        public async Task<IActionResult> Delete(string userId, string serviceId)
        {
            var model = await _context.EmployeeServiceModel
                .FirstOrDefaultAsync(m => m.Employee.Id == int.Parse(userId) && m.Service.Id == int.Parse(serviceId));
            _context.EmployeeServiceModel.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = userId });
        }
    }
}
