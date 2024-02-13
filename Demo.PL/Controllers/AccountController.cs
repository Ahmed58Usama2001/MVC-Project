using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers;

public class AccountController : Controller
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMailSetting _mailSetting;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IMailSetting mailSetting)
	{
		_userManager = userManager;
		_signInManager = signInManager;
        _mailSetting = mailSetting;
    }

	public IActionResult SignUp()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> SignUp(SignUpViewModel model)
	{
		if (ModelState.IsValid) //Server side validation
		{
			var user = await _userManager.FindByNameAsync(model.UserName);

			if (user is null)
			{

				user = new ApplicationUser
				{

					UserName = model.UserName,
					Email = model.Email,
					IsAgree = model.IsAgree,
					FName = model.FName,
					LName = model.LName,

				};

				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
					return RedirectToAction(nameof(SignIn));

				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);

			}

			ModelState.AddModelError(string.Empty, "Username Already Exists");
		}

		return View(model);
	}


	public IActionResult SignIn()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> SignIn(SignInViewModel model)
	{
		if (ModelState.IsValid)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is not null)
			{
				var flag = await _userManager.CheckPasswordAsync(user, model.Password);

				if (flag)
				{
					var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
					if (result.Succeeded) return RedirectToAction(nameof(HomeController.Index), "Home");
				}
			}

			ModelState.AddModelError(string.Empty, "Invalid Login");

		}

		return View(model);
	}

	public async new Task<IActionResult> SignOut()
	{
		await _signInManager.SignOutAsync();

		return RedirectToAction(nameof(SignIn));
	}

	public IActionResult ForgetPassword()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> SendResetPaswordUrl(ForgetPasswordViewModel model)
	{
		if(ModelState.IsValid)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is not null)
			{
				var token=await _userManager.GeneratePasswordResetTokenAsync(user);
				var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email ,token},Request.Scheme);

				var email = new Email()
				{
					Subject="Reset Your Password",
					Recipients=model.Email,
					Body= resetPasswordUrl
                };
				//EmailSettings.SendEmail(email);
				_mailSetting.SendEmail(email);
				return RedirectToAction(nameof(CheckYourInbox));
			}

			ModelState.AddModelError(string.Empty, "Invalid Email");
		}

		return View(model);
	}

	public IActionResult CheckYourInbox()
	{
		return View();
	}

    public IActionResult ResetPassword(string email,string token)
	{
		TempData["email"] = email;
		TempData["token"] = token;

		return View();
    }

	[HttpPost]
	public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
	{

	if(ModelState.IsValid)
		{
			string email = (string)TempData["email"];
			string token = (string)TempData["token"];

			var user = await _userManager.FindByEmailAsync(email);

			var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

			if (result.Succeeded)
				return RedirectToAction(nameof(SignIn));

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);
		}

		return View(model);
	}
}
