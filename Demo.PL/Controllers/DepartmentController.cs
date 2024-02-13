using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IUnitOfWork unitOfWork/*IDepartmentRepository departmentRepository*/)
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;
        }

        public IActionResult Index()
        {
            var deparmetns=_unitOfWork.DepartmentRepository.GetAll();
            return View(deparmetns);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid)
            {
                 _unitOfWork.DepartmentRepository.Add(department);
                var count =_unitOfWork.Complete();
                if (count > 0)                
                    TempData["Message"] = "Department is created successfully";
                
                else
                    TempData["Message"] = "An Error has occured, Department not created";

                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        public IActionResult Details(int? id,string ViewName="Details")
        {
            if(!id.HasValue)
                return BadRequest();

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            if(department is null)
                return NotFound();

            return View(ViewName, department);
        }

        public IActionResult Edit(int? id)
        {
            ///if (!id.HasValue)
            ///    return BadRequest();
            ///var department = _departmentRepository.Get(id.Value);
            ///if (department is null)
            ///    return NotFound();
            ///return View(department);

            return Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Department department)
        {
            if(id!=department.Id)
                return BadRequest();

            if(ModelState.IsValid)
            {
                try
                {

                    _unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {

                    //1. Log Exception
                    //2. Friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

          
            return View(department);
        }

        public IActionResult Delete(int? id)
        {
       
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id,Department department)
        {
            if (id != department.Id)
                return BadRequest();

            try
            {
                _unitOfWork.DepartmentRepository.Delete(department);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                //1. Log Exception
                //2. Friendly message

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }
        }
    }
}
