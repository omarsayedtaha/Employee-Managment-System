using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Model;
using DemoPL.Helper;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    public class EmployeeController1 : Controller
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController1(IUnitOfWork unitOfWork
           , IMapper mapper)
        {
            //_EmployeeRepository = EmployeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string Searchvalue)
        {
            //1. ViewData 
            ViewData["Message"] = "Hellow View Data";


            //2.ViewBag
            ViewBag.Message= "Hellow View Bag";

            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(Searchvalue))
                employees= await _unitOfWork.EmployeeRepository.GetAll();

            else
                employees=  _unitOfWork.EmployeeRepository.SearchEmployeeByName(Searchvalue);

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(mappedEmps);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Department = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                #region Manual Maping
                //var employee = new Employee()
                //{
                //    Name = employeeVM.Name,
                //    Address = employeeVM.Address,
                //    Email = employeeVM.Email,
                //    Salary = employeeVM.Salary,
                //    Age = employeeVM.Age,
                //    DepartmentId = employeeVM.DepartmentId,
                //    IsActive = employeeVM.IsActive,
                //    HireDate = employeeVM.HireDate,
                //    PhoneNumber = employeeVM.PhoneNumber
                //} 
                #endregion
                
                    employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "Images");

                //Employee employee = (Employee)employeeVM;

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                await _unitOfWork.EmployeeRepository.Add(mappedEmp);

                await _unitOfWork.Complete();
                //3.Temp Data 
                //if (count > 0)
                //    TempData["Message"] = "Employee is Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var Employee = await _unitOfWork.EmployeeRepository.Get(id.Value);

            if (Employee is null)
                return NotFound();

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);

            return View(viewName, mappedEmp);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmp);

                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(String.Empty, ex.Message);
                    return View(employeeVM);
                }

            }
            return View(employeeVM);

        }


        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                int count = await _unitOfWork.Complete();

                if (count > 0 && employeeVM.ImageName is not null)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }

    }
}

