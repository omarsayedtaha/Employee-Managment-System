using Demo.DAL.Model;
using DemoPL.Helper;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DemoPL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager, IEmailSettings emailSettings)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            this.emailSettings = emailSettings;
        }

		#region Register

		public IActionResult Register()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel Model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser()
				{
					FName = Model.FName,
					LName = Model.LName,
					UserName = Model.Email.Split('@')[0],
					Email = Model.Email,
					IsAgree = Model.IsAgree
				};

				var result = await _userManager.CreateAsync(user, Model.Password);

				if (result.Succeeded) {
					return RedirectToAction(nameof(Login));

				}

				foreach (var error in result.Errors)
				  ModelState.AddModelError(string.Empty, error.Description);
				
			}
			return View(Model);
		}
		#endregion

		#region Login
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);

					if (flag)
					{
						await _signInManager.PasswordSignInAsync(user, model.Password , model.RememberMe,false);
						return RedirectToAction("Index", "Home"); 
					}
					ModelState.AddModelError(string.Empty, "Invalid Password");

				}
				ModelState.AddModelError(string.Empty, "Email does not exist");
			}
			return View(model);
		}
		#endregion

		#region Sign out 

		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}


		#endregion

		#region Forget Password
		public IActionResult ForgetPassword()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await  _userManager.GeneratePasswordResetTokenAsync(user); //Token Valid For This User Only One-Time
					var passwordResetLlink = Url.Action("ResetPassword" , "Account" , new {email = user.Email ,token} ,Request.Scheme);

                    //https://localhost:44386/Aaccount/ReetPassword?email=os.taha007@gmail.com
                    var email = new Email()
					{
						Subject = "Reset Password",
						To = user.Email,
						Body = passwordResetLlink

                    };
					emailSettings.SendEmail(email);
					return View("CheckYourInbox", "Account");
				}
				ModelState.AddModelError(string.Empty, "Email does not Exist");
			}
			return View();
		}

		[HttpGet]
		public IActionResult CheckYourInbox()
		{

			return View();
		}

        public IActionResult ResetPassword(string email , string token)
        {
			TempData["email"] = email;
			TempData["token"] = token;

			return View();
        }

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
				var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

				if (result.Succeeded)
					return RedirectToAction(nameof(Login));

				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}
		#endregion


	}
}
