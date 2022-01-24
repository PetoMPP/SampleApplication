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
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceModel>>> GetServiceModel()
        {
            return await _context.ServiceModel.ToListAsync();
        }

        // GET: api/Services/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceModel>> GetServiceModel(int id)
        {
            var serviceModel = await _context.ServiceModel.FindAsync(id);

            if (serviceModel == null)
            {
                return NotFound($"Service with id: {id} does not exist!");
            }

            return serviceModel;
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutServiceModel(int id, ServiceModel serviceModel)
        {
            if (serviceModel.Id == 0)
            {
                return BadRequest("Service Id must be greater than 0!");
            }
            if (serviceModel.Duration == 0)
            {
                return BadRequest("Service Duration must be greater than 0!");
            }
            if (serviceModel.Name == null)
            {
                return BadRequest("Service Name must be specified!");
            }
            if (id != serviceModel.Id)
            {
                return BadRequest("Ids mismatch!");
            } 

            _context.Entry(serviceModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceModelExists(id))
                {
                    return NotFound($"Service with Id: {id} was not found!");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceModel>> PostServiceModel(ServiceModel serviceModel)
        {
            if (serviceModel.Duration == 0)
            {
                return BadRequest("Service Duration must be greater than 0!");
            }
            if (serviceModel.Name == null)
            {
                return BadRequest("Service Name must be specified!");
            }
            _context.ServiceModel.Add(serviceModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceModel", new { id = serviceModel.Id }, serviceModel);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceModel(int id)
        {
            var serviceModel = await _context.ServiceModel.FindAsync(id);
            if (serviceModel == null)
            {
                return NotFound($"Service with id: {id} does not exist!");
            }

            _context.ServiceModel.Remove(serviceModel);
            await _context.SaveChangesAsync();

            return Ok($"Service with id: {id} was successfully deleted!");
        }

        private bool ServiceModelExists(int id)
        {
            return _context.ServiceModel.Any(e => e.Id == id);
        }
    }
}
