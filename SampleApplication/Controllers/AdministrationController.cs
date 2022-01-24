using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApplication.Data;
using SampleApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            _context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<RoleModel> roleModels = new List<RoleModel>();
            foreach (var role in roleManager.Roles)
            {
                RoleModel model = new RoleModel
                {
                    Name = role.Name,
                    RoleDbId = role.Id
                };
                roleModels.Add(model);
            }
            return View(roleModels);
        }
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.Name
                };
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult RoleEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<RoleModel> models = new List<RoleModel>();
            foreach (IdentityRole role in roleManager.Roles)
            {
                RoleModel r = new RoleModel
                {
                    RoleDbId = role.Id,
                    Name = role.Name
                };
                models.Add(r);
            }
            if (!models.Where(x => x.RoleDbId == id).Any())
            {
                return NotFound();
            }
            return View(models.Where(x => x.RoleDbId == id).First());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleEdit(string id, RoleModel model)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (!roleManager.Roles.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                try
                {
                    var r = await roleManager.FindByIdAsync(id);
                    r.Name = model.Name;
                    await roleManager.UpdateAsync(r);
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult RoleDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<RoleModel> models = new List<RoleModel>();
            foreach (IdentityRole role in roleManager.Roles)
            {
                RoleModel r = new RoleModel
                {
                    RoleDbId = role.Id,
                    Name = role.Name
                };
                models.Add(r);
            }
            if (!models.Where(x => x.RoleDbId == id).Any())
            {
                return NotFound();
            }
            return View(models.Where(x => x.RoleDbId == id).First());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleDelete(string id, RoleModel model)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (!roleManager.Roles.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                try
                {
                    var r = await roleManager.FindByIdAsync(id);
                    IdentityResult result = await roleManager.DeleteAsync(r);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UserIndex()
        {
            List<UserModel> users = new List<UserModel>();
            foreach (var user in userManager.Users)
            {
                List<string> curRoles = (List<string>)await userManager.GetRolesAsync(user);
                List<string> allRoles = new List<string>();
                foreach (IdentityRole r in roleManager.Roles)
                {
                    allRoles.Add(r.Name);
                }
                List<string> availRoles = allRoles.Except(curRoles).ToList();
                UserModel u = new UserModel
                {
                    UserDbId = user.Id,
                    Name = user.UserName,
                    CurrentRoles = curRoles,
                    AvailableRoles = availRoles
                };
                users.Add(u);
            }
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> UserRoleEdit(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            List<string> curRoles = (List<string>)await userManager.GetRolesAsync(user);
            List<string> allRoles = new List<string>();
            foreach (IdentityRole r in roleManager.Roles)
            {
                allRoles.Add(r.Name);
            }
            List<string> availRoles = allRoles.Except(curRoles).ToList();
            UserModel u = new UserModel
            {
                UserDbId = user.Id,
                Name = user.UserName,
                CurrentRoles = curRoles,
                AvailableRoles = availRoles
            };
            return View(u);
        }
        //[Route("{userId}&{roleName}")]
        [HttpGet]
        public async Task<IActionResult> UserRoleAdd(string userId, string roleName)
        {
            IdentityUser user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRoleAsync(user, roleName);
            return RedirectToAction("UserRoleEdit", new {id = userId });
        }
        //[Route("{userId}&{roleName}")]
        [HttpGet]
        public async Task<IActionResult> UserRoleRemove(string userId, string roleName)
        {
            IdentityUser user = await userManager.FindByIdAsync(userId);
            await userManager.RemoveFromRoleAsync(user, roleName);
            return RedirectToAction("UserRoleEdit", new { id = userId });
        }
    }
} 
