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
    public class EmployeesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public EmployeesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            _context = context;
            this.userManager = userManager;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            List<EmployeeModel> employeeModels = await _context.EmployeeModel.ToListAsync();
            foreach (EmployeeModel e in employeeModels)
            {
                IdentityUser user = await _context.Users.FindAsync(e.UserId);
                e.UserName = user.UserName;
            }
            return View(employeeModels);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.EmployeeModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeModel == null)
            {
                return NotFound();
            }
            IdentityUser user = await _context.Users.FindAsync(employeeModel.UserId);
            employeeModel.UserName = user.UserName;

            return View(employeeModel);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create(CreateEmployeeViewModel model, string error)
        {
            IEnumerable<IdentityUser> employees = await userManager.GetUsersInRoleAsync("Employee");
            IEnumerable<IdentityUser> usedUsers = await GetEmployeesUsersAsync();

            IEnumerable<IdentityUser> unassignedUsers = employees.Except(usedUsers);

            if (unassignedUsers.Count() != 0)
            {
                model.UnassignedUsersWithEmployeeRole = new SelectList(unassignedUsers, unassignedUsers.First().UserName);
            }
            return View(model);
        }

        private async Task<IEnumerable<IdentityUser>> GetEmployeesUsersAsync()
        {
            List<EmployeeModel> employees = _context.EmployeeModel.ToList();
            foreach (EmployeeModel e in employees)
            {
                IdentityUser user = await _context.Users.FindAsync(e.UserId);
                e.UserName = user.UserName;
            }
            List<IdentityUser> userNames = new List<IdentityUser>();
            foreach (EmployeeModel employee in employees)
            {
                userNames.Add(await userManager.FindByNameAsync(employee.UserName));
            }
            return userNames;
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            if (model.Employee.UserName != null)
            {
                IdentityUser user = await userManager.FindByNameAsync(model.Employee.UserName);
                model.Employee.UserId = user.Id;
                _context.Add(model.Employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.EmployeeModel.FindAsync(id);
            if (employeeModel == null)
            {
                return NotFound();
            }
            IdentityUser user = await _context.Users.FindAsync(employeeModel.UserId);
            employeeModel.UserName = user.UserName;
            return View(employeeModel);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeModel employeeModel)
        {
            if (id != employeeModel.Id)
            {
                return NotFound();
            }
            try
            {
                IdentityUser user = await userManager.FindByNameAsync(employeeModel.UserName);
                employeeModel.UserId = user.Id;
                _context.Update(employeeModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeModelExists(employeeModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View(employeeModel);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeModel = await _context.EmployeeModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeModel == null)
            {
                return NotFound();
            }


            return View(employeeModel);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeModel = await _context.EmployeeModel.FindAsync(id);
            _context.EmployeeModel.Remove(employeeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.EmployeeModel.Any(e => e.Id == id);
        }
    }
}
