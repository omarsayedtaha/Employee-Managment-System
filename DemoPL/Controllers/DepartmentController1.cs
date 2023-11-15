using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{

    public class DepartmentController1 : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public IUnitOfWork _unitOfWork;

        public DepartmentController1(IUnitOfWork unitOfWork)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            //1. ViewData 
            ViewData["Message"] = "Hellow View Data";

            //2.ViewBag
            ViewBag.Message = "Hello View Bag";

            var departments = await _unitOfWork.DepartmentRepository.GetAll();    
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DepartmentRepository.Add(department);

                 await _unitOfWork.Complete(); 
                //3.Temp Data 
                
                 TempData["Message"] = "Deprtment is Created Successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

         public async Task<IActionResult> Details(int? id , string viewName="Details")
         {
            if (id is null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.Get(id.Value);

            if (department is null)
                return NotFound();

            return View(viewName, department);
         }

        public async  Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();

            //var department = _departmentRepository.Get(id.Value);


            //if (department is null)
            //    return NotFound();

            //return View(department);
            return await Details(id , "Edit"); 

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id , Department department)
        {
            if (id !=department.Id)
            {
                return BadRequest();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.Complete(); 
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(String.Empty, ex.Message);
                    return View(department);
                }
              
            }
            return View(department); 

        }


        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id,Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            try
            {
                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.Complete(); 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department); 
            }
        }

    }
}
