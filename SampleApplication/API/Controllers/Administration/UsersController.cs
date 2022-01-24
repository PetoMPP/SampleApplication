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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        // GET: api/Users
        [HttpGet]
        public ActionResult<IEnumerable<IdentityUser>> GetIdentityUsers()
        {
            return context.Users.ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityUser>> GetIdentityUser(string id)
        {
            IdentityUser user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest($"No user was found with id: {id}");
            }
            return context.Users.ToList().Where(u => u.Id == id).First();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdentityUser(string id, [Bind("Id, Name")] IdentityUser identityUser)
        {
            if (identityUser.UserName == null)
            {
                return BadRequest("UserName must be specified!");
            }
            if (id != identityUser.Id)
            {
                return BadRequest("Ids mismatch!");
            }
            if (context.Roles.Any(r => r.NormalizedName == identityUser.UserName.ToUpper()))
            {
                return BadRequest("UserName already assigned to another user!");
            }

            context.Entry(identityUser).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency value changed at the time of request or was not specified");
            }
            return Ok();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<IdentityUser>> PostIdentityRole([Bind("Name")] IdentityUser identityUser)
        {
            if (identityUser.UserName == null)
            {
                return BadRequest("Role Name must be specified!");
            }
            if (await context.Users.AnyAsync(r => r.NormalizedUserName == identityUser.UserName.ToUpper()))
            {
                return BadRequest($"Role with name: {identityUser.UserName} already exists!");
            }

            identityUser.NormalizedUserName = identityUser.UserName.ToUpper();

            await context.Users.AddAsync(identityUser);
            await context.SaveChangesAsync();

            return CreatedAtAction("PostIdentityRole", new { id = identityUser.Id }, identityUser);
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentityRole(string id)
        {
            IdentityUser identityUser = await context.Users.FindAsync(id);
            if (identityUser == null)
            {
                return NotFound();
            }
            context.Users.Remove(identityUser);
            await context.SaveChangesAsync();

            return NoContent();
        }

        //
        // User Roles API Section //
        //

        // GET: api/Users/5/Roles
        [HttpGet("{userId}/Roles")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetUserIdentityRoles(string userId)
        {
            IdentityUser identityUser = await context.Users.FindAsync(userId);
            if (identityUser == null)
            {
                return NotFound($"User with id: {userId} was not found!");
            }
            List<string> roleNames = (List<string>)userManager.GetRolesAsync(identityUser).GetAwaiter().GetResult();
            if (roleNames.Count == 0)
            {
                return NoContent();
            }
            List<IdentityRole> roles = new List<IdentityRole>();
            foreach (string roleName in roleNames)
            {
                IdentityRole role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
                roles.Add(role);
            }
            return roles;
        }
        // POST: api/Users/5/Roles
        [HttpPost("{userId}/Roles")]
        public async Task<IActionResult> PostUserIdentityRole(string userId, IdentityRole identityRole)
        {
            if (identityRole.Name == null)
            {
                return BadRequest("Role Name must be specified!");
            }
            if (roleManager.FindByNameAsync(identityRole.Name).GetAwaiter().GetResult() == null)
            {
                return BadRequest($"Role with name: {identityRole.Name} was not found!");
            }
            IdentityUser user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest($"User with Id: {userId} was not found!");
            }
            if (userManager.GetRolesAsync(user).GetAwaiter().GetResult().Any(u => u == identityRole.Name))
            {
                return BadRequest($"User: {userId} is already assigned with role: {identityRole.Name}!");
            }
            try
            {
                await userManager.AddToRoleAsync(user, identityRole.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency value changed at the time of request or was not specified!");
            }
            return Ok();
        }
        // DELETE: api/Users/5/Roles/5
        [HttpDelete("{userId}/Roles/{roleName}")]
        public async Task<IActionResult> DeleteUserIdentityRole(string userId, string roleName)
        {
            IdentityUser user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                return BadRequest($"User with Id: {userId} was not found!");
            }

            IdentityRole role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return BadRequest($"Role with Id: {roleName} was not found!");
            }

            if (!userManager.GetRolesAsync(user).GetAwaiter().GetResult().Any(u => u == role.Name))
            {
                return BadRequest($"User: {userId} is not with role: {role.Name}!");
            }
            try
            {
                await userManager.RemoveFromRoleAsync(user, role.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Concurrency value changed at the time of request or was not specified!");
            }
            return Ok();
        }
    }
}
