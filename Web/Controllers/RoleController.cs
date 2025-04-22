using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Get All Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();  // list of roles in system
            return View(roles);
        }

        // Get All Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();  // list of roles in system
            return View(users);
        }
        // Create Role
        public async Task<IActionResult> CreateRole() => View();
        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole roleIdentity)
        {
            string roleName = roleIdentity.Name; // get role name from view
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("Name", "Role name is required");
                return View(roleName);
            }
            // check if role is exists
            var existingRole = await _roleManager.RoleExistsAsync(roleName); // Admin , admin
            if(existingRole)
            {
                ModelState.AddModelError("Name", "Role already exists");
                return View(roleName);
            }


            // create role
            var role = new IdentityRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(roleName);

        }




        // delete role
        [HttpPost]
        public async Task<IActionResult> Delete (string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = "Role not found";
                return RedirectToAction("Index");
            }
            await _roleManager.DeleteAsync(role);
             return RedirectToAction("Index");
            
        }



        // asign role to user

        [HttpGet]
        public async Task<IActionResult> AsignRolesToUser(string userId) // userId = fff 
        {
            // get user by id
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return RedirectToAction("Index");
            }
            // get all user roles
            var userRoles = await _userManager.GetRolesAsync(user);
            // get all roles in system

            var roles = await _roleManager.Roles.ToListAsync();
            if (!roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
                roles = await _roleManager.Roles.ToListAsync(); // reload roles
            }
            var roleList = roles.Select(r => new RoleViewModel
            {
                RoleId = r.Id,
                RoleName = r.Name,
                UserRole = userRoles.Contains(r.Name), // check if user has this role
                //UserRole= userRoles.Any(x => x == r.Name)
            }).ToList();


            ViewBag.UserId = userId; // pass userId to view
            ViewBag.UserName = user.UserName; // pass userName to view
            ViewBag.FullName = user.FullName; // pass userFullName to view
            return View(roleList);


        }
        [HttpPost]
        public async Task<IActionResult> AsignRolesToUser(string userId ,string jsonRoles)
        {
            // get user by id
            var user = await _userManager.FindByIdAsync(userId);
            if(string.IsNullOrWhiteSpace(jsonRoles))
            {
                ModelState.AddModelError("Role", "Please select at least one role");
                return View();
            }
            // get roles from json
            List<RoleViewModel> MyRoles = JsonConvert.DeserializeObject<List<RoleViewModel>>(jsonRoles);


            if(user is not null)
            {
                // get user roles
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in MyRoles)
                {
                    // check if user has this role but not selected
                    if (userRoles.Contains(role.RoleName.Trim()) && !role.UserRole) // !true == false
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.RoleName.Trim());
                    }
                    // check if user don't have this role but selected
                    if (!userRoles.Contains(role.RoleName.Trim()) && role.UserRole) // !true == false
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName.Trim());
                    }
                }
                return RedirectToAction("users"); // roles

            }
            else
            {
                ModelState.AddModelError("Role", "User id not valid");
                return View();
            }

        }


    }
}
