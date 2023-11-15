using AutoMapper;
using Demo.DAL.Model;
using DemoPL.Helper;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DemoPL.Controllers
{
	public class UserController1 : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController1(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager

             ,RoleManager<IdentityRole> roleManager, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

		public async Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				var users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();  
				return View(users);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(email);
				var mappedUser	 =  new UserViewModel()
				{
					Id = user.Id,
					FName = user.FName,
					LName = user.LName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Roles = _userManager.GetRolesAsync(user).Result
				};

				return View(new List<UserViewModel>() { mappedUser });
			}
		}

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

			var User = await _userManager.FindByIdAsync(id);

            if (User is null)
                return NotFound();

            var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(User);

            return View(viewName, mappedUser);
        }

        // User/Edit/Guid
        //[HttpGet]
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

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    var AllRoles = _roleManager.FindByIdAsync(id);
                    user.FName = updatedUser.FName;
                    user.LName = updatedUser.LName;
                    user.PhoneNumber = updatedUser.PhoneNumber;
                   
                    
                   
                    //user.Email = updatedUser.Email;



                    await _userManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log Exception
                    // 2. Friendly Message

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(updatedUser);
        }



        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost(Name ="Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
          
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                
                await _userManager.DeleteAsync(user);
                 

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error" , "Home");
            }
        }
    }
}
