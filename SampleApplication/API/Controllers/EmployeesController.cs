using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApplication.Data;
using SampleApplication.Models;

namespace SampleApplication.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public EmployeesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetEmployeeModel()
        {
            List<EmployeeModel> employeeModels = await _context.EmployeeModel.ToListAsync();
            foreach (EmployeeModel e in employeeModels)
            {
                IdentityUser user = await _context.Users.FindAsync(e.UserId);
                e.UserName = user.UserName;
            }
            return await _context.EmployeeModel.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeModel>> GetEmployeeModel(int id)
        {
            var employeeModel = await _context.EmployeeModel.FindAsync(id);

            if (employeeModel == null)
            {
                return NotFound($"Employee with id: {id} does not exist!");
            }
            IdentityUser user = await _context.Users.FindAsync(employeeModel.UserId);
            employeeModel.UserName = user.UserName;

            return employeeModel;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeModel(int id, EmployeeModel employeeModel)
        {
            if (employeeModel.Id == 0)
            {
                return BadRequest("Employee Id must be greater than 0!");
            }
            if (id != employeeModel.Id)
            {
                return BadRequest("Ids mismatch!");
            }
            //if (employeeModel.UserId == null && employeeModel.UserName == null)
            //{
            //    return BadRequest("Model must be provided with UserName and/or UserId!");
            //}
            //if (employeeModel.UserId == null)
            //{
            //    if (!_context.Users.Any(u => u.UserName == employeeModel.UserName))
            //    {
            //        return BadRequest("Employee UserName must be an existing UserName in the Database!");
            //    }
            //    employeeModel.UserId = _context.Users.Where(u => u.UserName == employeeModel.UserName).First().Id;
            //}
            //else
            //{
            if (!await _context.Users.AnyAsync(u => u.Id == employeeModel.UserId))
            {
                return BadRequest("Employee UserId must be an existing UserId in the Database!");
            }
            //}
            EmployeeModel employeeWithMatchingUserId = _context.EmployeeModel.Where(e => e.UserId == employeeModel.UserId).FirstOrDefault();
            //if (_context.EmployeeModel.Any(e => e.UserId == employeeModel.UserId && e.UserId == employeeModel.UserId))
            if (employeeWithMatchingUserId != null)
            {
                if (employeeWithMatchingUserId.Id != employeeModel.Id)
                {
                    return BadRequest("Specified DataBase User already has been assigned to another Employee!");
                }
            }

            _context.Entry(employeeWithMatchingUserId).State = EntityState.Detached;

            _context.Entry(employeeModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> PostEmployeeModel(EmployeeModel employeeModel)
        {
            if (employeeModel.Id != 0)
            {
                BadRequest("Id must not be provided or be equal to 0!");
            }
            if (await _context.EmployeeModel.AnyAsync(e => e.UserId == employeeModel.UserId))
            {
                return BadRequest("Specified DataBase User already has been assigned to an existing Employee!");
            }

            _context.EmployeeModel.Add(employeeModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeModel", new { id = employeeModel.Id }, employeeModel);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeModel(int id)
        {
            var employeeModel = await _context.EmployeeModel.FindAsync(id);
            if (employeeModel == null)
            {
                return NotFound();
            }

            _context.EmployeeModel.Remove(employeeModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeModelExists(int id)
        {
            return _context.EmployeeModel.Any(e => e.Id == id);
        }

        //
        // Employee Services API Section
        //

        public class ServiceViewModel
        {
            public int Id { get; set; }
        }

        // GET: api/Employees/5/Services
        [HttpGet("{employeeId}/Services")]
        public ActionResult<IEnumerable<ServiceModel>> GetEmployeeServiceModel(int employeeId)
        {
            if (!EmployeeModelExists(employeeId))
            {
                return BadRequest($"No employee was found with id: {employeeId}!");
            }
            _ = _context.ServiceModel.ToList();
            _ = _context.EmployeeModel.ToList();
            List<ServiceModel> services = new List<ServiceModel>();
            List<EmployeeServiceModel> models = _context.EmployeeServiceModel
                .ToList()
                .Where(es => es.Employee.Id == employeeId)
                .ToList();
            foreach (EmployeeServiceModel model in models)
            {
                services.Add(model.Service);
            }
            return services;
        }

        // POST: api/Employees/5/Services
        [HttpPost("{employeeId}/Services")]
        public async Task<ActionResult> PostEmployeeServiceModel(int employeeId, ServiceViewModel service)
        {
            if (service.Id == 0)
            {
                return BadRequest("Id must be provided or greater than 0!");
            }
            if (!EmployeeModelExists(employeeId))
            {
                return BadRequest($"No employee was found with id: {employeeId}!");
            }
            _ = _context.EmployeeModel.ToList();
            List<ServiceModel> services = _context.ServiceModel.ToList();
            if (!services.Any(s => s.Id == service.Id))
            {
                return BadRequest($"No service was found with id: {service.Id}!");
            }
            List<EmployeeServiceModel> models = _context.EmployeeServiceModel.ToList();
            if (models.Any(es => es.Employee.Id == employeeId && es.Service.Id == service.Id))
            {
                return BadRequest($"Employee with id: {employeeId} is already assigned with service with id: {service.Id}");
            }
            ServiceModel ServiceModel = await _context.ServiceModel.FindAsync(service.Id);
            EmployeeServiceModel model = new()
            {
                Employee = await _context.EmployeeModel.FindAsync(employeeId),
                Service = ServiceModel
            };

            _context.EmployeeServiceModel.Add(model);
            await _context.SaveChangesAsync();

            return Ok();
        }
        // DELETE: api/Employees/5/Services/5
        [HttpDelete("{employeeId}/Services/{serviceId}")]
        public async Task<ActionResult> DeleteEmployeeServiceModel(int employeeId, int serviceId)
        {
            if (!EmployeeModelExists(employeeId))
            {
                return BadRequest($"No employee was found with id: {employeeId}!");
            }
            _ = _context.EmployeeModel.ToList();
            List<ServiceModel> services = _context.ServiceModel.ToList();
            if (!services.Any(s => s.Id == serviceId))
            {
                return BadRequest($"No service was found with id: {serviceId}!");
            }

            ServiceModel ServiceModel = await _context.ServiceModel.FindAsync(serviceId);
            List<EmployeeServiceModel> models = _context.EmployeeServiceModel.ToList();
            EmployeeServiceModel model = models.Where(es => es.Employee.Id == employeeId && es.Service.Id == serviceId).FirstOrDefault();
            if (model == null)
            {
                return BadRequest($"Employee with id: {employeeId} is not assigned with service with id: {serviceId}");
            }

            _context.EmployeeServiceModel.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
