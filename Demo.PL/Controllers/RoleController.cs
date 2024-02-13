using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole>roleManager,IMapper mapper,UserManager<ApplicationUser>userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = _roleManager.Roles.Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    RoleName= r.Name
                }).ToList();
                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);

                if (role is not null)
                {
                    var mappedRole = new RoleViewModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    };
                    return View(new List<RoleViewModel>() { mappedRole });
                }

                return View(Enumerable.Empty<RoleViewModel>());
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var mappedRole=_mapper.Map<RoleViewModel,IdentityRole>(model);

                await _roleManager.CreateAsync(mappedRole);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);


            return View(ViewName, mappedRole);
        }

        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel updatedRole)
        {
            if (id != updatedRole.Id)
                return BadRequest();

            //if (ModelState.IsValid)
            //{
            try
            {
                var role = await _roleManager.FindByIdAsync(id);

                role.Name = updatedRole.RoleName;

                await _roleManager.UpdateAsync(role);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                //1. Log Exception
                //2. Friendly message
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            //}


            return View(updatedRole);
        }

        public async Task<IActionResult> Delete(string id)
        {

            return await Details(id, "Delete");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {

            try
            {
                var role = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //1. Log Exception
                //2. Friendly message

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role == null)
                return BadRequest();

            ViewBag.RoleId = RoleId;

            var usersInRole = new List<UserInRoleViewModel>();

            foreach (var user in await _userManager.Users.ToListAsync())
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else userInRole.IsSelected = false;

                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> users, string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser != null)
                    {
                        if (user.IsSelected && !(await _userManager.IsInRoleAsync(appUser, role.Name)))
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        else if (!user.IsSelected && (await _userManager.IsInRoleAsync(appUser, role.Name)))
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }

                }
                return RedirectToAction(nameof(Update), new { id = RoleId });
            }
            return View(users);
        }
    }
}
