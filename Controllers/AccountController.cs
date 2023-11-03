using Furn.DAL;
using Furn.Models;
using Furn.Models.Auth;
using Furn.Models.Interface;
using Furn.ViewModel;
using Furn.ViewModel.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furn.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IEmailSender _emailSender;
		private readonly AppDbContext _context;


		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_context = context;
		}
		public async Task<IActionResult> Index(string id)
        {
			var users = await _context.Users.FindAsync(id);

			return View(users);
		}
        public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{
			if (!ModelState.IsValid) return View(registerVM);
			AppUser newUser = new AppUser()
			{
				Fullname = registerVM.Fullname,
				UserName = registerVM.Username,
				Email = registerVM.Email
			};
			IdentityResult registerResult = await _userManager.CreateAsync(newUser, registerVM.Password);
			if (!registerResult.Succeeded)
			{
				foreach (IdentityError error in registerResult.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(registerVM);
			}
			//IdentityResult roleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.User.ToString());
			//if (!roleResult.Succeeded)
			//{
			//	foreach (IdentityError error in roleResult.Errors)
			//	{
			//		ModelState.AddModelError("", error.Description);
			//	}
			//	return View(registerVM);
			//}
			return RedirectToAction(nameof(Login));
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginVM login, string? ReturnUrl)
		{
			if (!ModelState.IsValid) return View(login);
			AppUser user = await _userManager.FindByEmailAsync(login.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Email or Password is wrong!");
				return View(login);
			}
			Microsoft.AspNetCore.Identity.SignInResult signinResult =
				await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

			if (!signinResult.Succeeded)
			{
				ModelState.AddModelError("", "Email or Password is wrong!");
				return View(login);
			}
			await _signInManager.SignInAsync(user, login.RememberMe);
			if (Url.IsLocalUrl(ReturnUrl))
			{
				return Redirect(ReturnUrl);
			}

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}
		//public async Task<IActionResult> AddRoles()
		//{
		//    foreach (var role in Enum.GetValues(typeof(UserRoles)))
		//    {
		//        if (!await _roleManager.RoleExistsAsync(role.ToString()))
		//        {
		//            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
		//        }
		//    }
		//    return Json("Ok");
		//}
		public IActionResult AccessDenied()
		{
			return View();
		}

		public enum UserRoles
		{
			Admin,
			User,
			Moderator
		}







		//// AccountController.cs faylında

		//// Şifrəni unutduğunuzda e-poçt ünvanınızı daxil etdiyiniz səhifəni göstərən metod
		//[HttpGet]
		//[AllowAnonymous]
		//public IActionResult ForgotPassword()
		//{
		//	return View();
		//}

		//// Şifrəni unutduğunuzda e-poçt ünvanınızı daxil etdiyiniz səhifədə formu təsdiqlədiyiniz zaman işləyən metod
		//[HttpPost]
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		// E-poçt ünvanına uyğun istifadəçini tapırıq
		//		var user = await _userManager.FindByEmailAsync(model.Mail);
		//		// Əgər istifadəçi mövcud deyilsə və ya hesabını təsdiqləməyibsə, sükutla sifarişinizi aldığımızı bildiririk
		//		if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
		//		{
		//			return View("ForgotPasswordConfirmation");
		//		}

		//		// Yeni şifrə yaratmaq üçün istifadə oluna biləcək kodu generasiya edirik
		//		var code = await _userManager.GeneratePasswordResetTokenAsync(user);
		//		// Yeni şifrə yaratmaq üçün keçid linkini yaradırıq
		//		var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
		//		// E-poçt göndərilməsi üçün lazım olan xidməti çağırırıq
		//		await _emailSender.SendEmailAsync(model.Mail, "Şifrəni yenilə", $"Şifrənizi yenilamak üçün <a href='{callbackUrl}'>buraya</a> kliklәyin.");

		//		return View("ForgotPasswordConfirmation");
		//	}

		//	return View(model);
		//}

		//// Yeni şifrә yaratmaq üçün e-poçtdan gәlәn link vasitәsilә aзılan sәhifәni göstәrәn metod
		//[HttpGet]
		//[AllowAnonymous]
		//public IActionResult ResetPassword(string code = null)
		//{
		//	if (code == null)
		//	{
		//		throw new ApplicationException("Şifrәni yenilәmәk üçün kod tәlәb olunur.");
		//	}
		//	var model = new ResetPasswordViewModel { Code = code };
		//	return View(model);
		//}

		//// Yeni şifrә yaratmaq üçün e-poçtdan gәlәn link vasitәsilә aзılan sәhifәdә formu tәsdiqlәdiyiniz zaman işlәyәn metod
		//[HttpPost]
		//[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return View(model);
		//	}
		//	// E-poçt ünvanına uyğun istifadеçini tapırıq
		//	var user = await _userManager.FindByEmailAsync(model.Email);
		//	if (user == null)
		//	{
		//		// İstifadеçi mövcud deyilsе, sükutla şifrеnizi yenilеdik deyirik
		//		return RedirectToAction(nameof(ResetPasswordConfirmation));
		//	}
		//	// İstifadеçinin şifrеsini yenilеyirik
		//	var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
		//	if (result.Succeeded)
		//	{
		//		// Şifrе yenilеnməsi uğurlu olduqda, tәsdiqlәmә sәhifәsinә yönlәndiririk
		//		return RedirectToAction(nameof(ResetPasswordConfirmation));
		//	}
		//	return View();
		//}

		//// Şifrәnizi unutduğunuzda e-poçt göndərildiyini bildirәn sәhifәni göstәrәn metod
		//[HttpGet]
		//[AllowAnonymous]
		//public IActionResult ForgotPasswordConfirmation()
		//{
		//	return View();
		//}

		//// Şifrәnizi yenilәdikdәn sonra tәsdiqlәmә sәhifәsini göstәrәn metod
		//[HttpGet]
		//[AllowAnonymous]
		//public IActionResult ResetPasswordConfirmation()
		//{
		//	return View();
		//}

	}
}
