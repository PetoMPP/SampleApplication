using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.API.Controllers
{
    [Route("api/Administration/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RolesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: api/Roles
        [HttpGet]
        public ActionResult<IEnumerable<IdentityRole>> GetIdentityRoles()
        {
            return context.Roles.ToList();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityRole>> GetIdentityRole(string id)
        {
            IdentityRole role = await context.Roles.FindAsync(id);
            if (role == null)
            {
                return BadRequest($"No role was found with id: {id}");
            }
            return context.Roles.ToList().Where(r => r.Id == id).First();
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdentityRole(string id, IdentityRole identityRole)
        {
            if (identityRole.Name == null)
            {
                return BadRequest("Role Name must be specified!");
            }
            if (id != identityRole.Id)
            {
                return BadRequest("Ids mismatch!");
            }
            if (context.Roles.Any(r => r.NormalizedName == identityRole.Name.ToUpper()))
            {
                return BadRequest("Role Name already assigned to another role!");
            }

            context.Entry(identityRole).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency value changed at the time of request or was not specified!");
            }
            return Ok();
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<IdentityRole>> PostIdentityRole([Bind("Name")] IdentityRole identityRole)
        {
            if (identityRole.Name == null)
            {
                return BadRequest("Role Name must be specified!");
            }
            if (await context.Roles.AnyAsync(r => r.NormalizedName == identityRole.Name.ToUpper()))
            {
                return BadRequest($"Role with name: {identityRole.Name} already exists!");
            }

            identityRole.NormalizedName = identityRole.Name.ToUpper();

            await context.Roles.AddAsync(identityRole);
            await context.SaveChangesAsync();

            return CreatedAtAction("PostIdentityRole", new { id = identityRole.Id }, identityRole);
        }
        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentityRole(string id)
        {
            IdentityRole identityRole = await context.Roles.FindAsync(id);
            if (identityRole == null)
            {
                return NotFound();
            }
            context.Roles.Remove(identityRole);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
