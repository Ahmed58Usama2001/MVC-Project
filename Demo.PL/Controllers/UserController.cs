using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
	public class UserController : Controller
	{
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public UserController(IMapper mapper,UserManager<ApplicationUser>userManager,SignInManager<ApplicationUser>signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<IActionResult> Index(string email)
		{
			if(string.IsNullOrEmpty(email))
			{
				var users = _userManager.Users.Select(u => new UserViewModel()
				{
					Id = u.Id,
					FName = u.FName,
					LName = u.LName,
					Email = u.Email,
					Roles = _userManager.GetRolesAsync(u).Result
				}).ToList() ;
				return View(users);
			}
			else
			{
				var user=await _userManager.FindByEmailAsync(email);

				var mappedUser = new UserViewModel()
				{
					Id = user.Id,
					FName = user.FName,
					LName = user.LName,
					Email = user.Email,
					Roles = _userManager.GetRolesAsync(user).Result
				};
				return View(new List<UserViewModel>() { mappedUser} );
			}

		}

        public async Task<IActionResult> Details(string  id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);


            return View(ViewName, mappedUser);
        }

        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest();

            //if (ModelState.IsValid)
            //{
                try
                {
                  var user=await _userManager.FindByIdAsync(id);

                    user.FName = updatedUser.FName;
                    user.LName= updatedUser.LName;
                    user.PhoneNumber = updatedUser.PhoneNumber;
                   

                    //user.Email = updatedUser.Email;
                    //user.SecurityStamp = Guid.NewGuid().ToString();


                    await _userManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {

                    //1. Log Exception
                    //2. Friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            //}


            return View(updatedUser);
        }

        public async Task<IActionResult> Delete(string id)
        {

            return await Details(id, "Delete");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete( string id)
        {

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //1. Log Exception
                //2. Friendly message

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error","Home");
            }
        }

    }
}
