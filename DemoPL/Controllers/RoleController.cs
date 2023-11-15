﻿using Demo.DAL.Model;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DemoPL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
            
       

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var Roles  = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();

                return View(Roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                var mappedRole = new RoleViewModel()
                {
                    Id = role.Id,
                    RoleName = role.Name
                };

                return View(new List<RoleViewModel>() { mappedRole });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                var mappedRole = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));

            }

            return View(roleVM);
        }


        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);

            if (User is null)
                return NotFound();

            var mappedRole = _mapper.Map<IdentityRole , RoleViewModel>(role);

            return View(viewName, mappedRole);
        }

        // Role/Edit/Guid
        //[HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel updatedRole)
        {
            if (id != updatedRole.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = updatedRole.RoleName;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(String.Empty, ex.Message);
                    return View(updatedRole);
                }

            }
            return View(updatedRole);

        }


        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost(Name = "Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {

            try
            {
                var user = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(user);


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
