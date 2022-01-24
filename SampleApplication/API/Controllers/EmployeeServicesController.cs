using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApplication.Data;
using SampleApplication.Models;

namespace SampleApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeServiceModel>>> GetEmployeeServiceModel()
        {
            List<EmployeeModel> employees = _context.EmployeeModel.ToList();
            List<ServiceModel> services = _context.ServiceModel.ToList();
            return await _context.EmployeeServiceModel.ToListAsync();
        }

        // GET: api/EmployeeServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeServiceModel>> GetEmployeeServiceModel(int id)
        {
            var employeeServiceModel = await _context.EmployeeServiceModel.FindAsync(id);

            if (employeeServiceModel == null)
            {
                return NotFound();
            }

            return employeeServiceModel;
        }

        // PUT: api/EmployeeServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeServiceModel(int id, EmployeeServiceModel employeeServiceModel)
        {
            if (id != employeeServiceModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(employeeServiceModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeServiceModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmployeeServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeServiceModel>> PostEmployeeServiceModel(EmployeeServiceModel employeeServiceModel)
        {
            _context.EmployeeServiceModel.Add(employeeServiceModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeServiceModel", new { id = employeeServiceModel.Id }, employeeServiceModel);
        }

        // DELETE: api/EmployeeServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeServiceModel(int id)
        {
            var employeeServiceModel = await _context.EmployeeServiceModel.FindAsync(id);
            if (employeeServiceModel == null)
            {
                return NotFound();
            }

            _context.EmployeeServiceModel.Remove(employeeServiceModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeServiceModelExists(int id)
        {
            return _context.EmployeeServiceModel.Any(e => e.Id == id);
        }
    }
}
