using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers;
[Authorize]
public class EmployeeController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    //private readonly IEmployeeRepository _employeeRepository;
    //private readonly IDepartmentRepository _departmentRepository;

    public EmployeeController(IMapper mapper,
        IUnitOfWork unitOfWork
        
        /*IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository*/)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        //_employeeRepository = employeeRepository;
        //_departmentRepository = departmentRepository; //Because we don't need an object for all the actions
    }

    public IActionResult Index(string SearchInput)
    {
        var employees = Enumerable.Empty<Employee>();

        if(string.IsNullOrEmpty(SearchInput))
             employees = _unitOfWork.EmployeeRepository.GetAll();
        
        else
             employees = _unitOfWork.EmployeeRepository.SearchByName(SearchInput.ToLower());

        var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

        return View(mappedEmps);
    }

    [HttpGet]
    public IActionResult Create()
    {
        //ViewBag.Departments = _departmentRepository.GetAll();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EmployeeViewModel employeeVM)
    {
        if (ModelState.IsValid)
        {
            employeeVM.ImageName=DocumentSettings.UploadFile(employeeVM.Image, "Images");
            //Manual Mapping
            //var mappedEmp = new Employee()
            //{
            //    Name = employeeVM.Name,
            //    Salary = employeeVM.Salary,
            //    PhoneNumber = employeeVM.PhoneNumber,
            //    Email = employeeVM.Email,
            //    Age = employeeVM.Age,
            //    IsActive = employeeVM.IsActive,
            //    HireDate = employeeVM.HireDate,
            //    Address = employeeVM.Address
            //};
            var mappedEmp=_mapper.Map<EmployeeViewModel,Employee>(employeeVM);   
            
                _unitOfWork.EmployeeRepository.Add(mappedEmp);

            var count = _unitOfWork.Complete();
            if (count > 0)
                TempData["Message"] = "Employee is created successfully";

            else
                TempData["Message"] = "An Error has occured, Employee not created";

            return RedirectToAction(nameof(Index));
        }

        return View(employeeVM);
    }

    public IActionResult Details(int? id, string ViewName = "Details")
    {
        if (!id.HasValue)
            return BadRequest();

        var employee = _unitOfWork.EmployeeRepository.Get(id.Value);

        var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

        if (employee is null)
            return NotFound();

        return View(ViewName, mappedEmp);
    }

    public IActionResult Edit(int? id)
    {
        ///if (!id.HasValue)
        ///    return BadRequest();
        ///var department = _departmentRepository.Get(id.Value);
        ///if (department is null)
        ///    return NotFound();
        ///return View(department);
        //ViewBag.Departments = _departmentRepository.GetAll();

        return Details(id, "Edit");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
    {
        if (id != employeeVM.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            try
            {
                var oldEmployee = _unitOfWork.EmployeeRepository.Get(id);

                _unitOfWork.EmployeeRepository.Detach(oldEmployee);

                if (!string.IsNullOrEmpty(oldEmployee.ImageName))
                    DocumentSettings.DeleteFile("Images", oldEmployee.ImageName);

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                mappedEmp.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                _unitOfWork.EmployeeRepository.Update(mappedEmp);

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


        return View(employeeVM);
    }

    public IActionResult Delete(int? id)
    {

        return Details(id, "Delete");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete([FromRoute] int id, EmployeeViewModel employeeVM)
    {
        if (id != employeeVM.Id)
            return BadRequest();

        try
        {
            var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

            _unitOfWork.EmployeeRepository.Delete(mappedEmp);

           var count= _unitOfWork.Complete();

            if (count > 0)
                DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            //1. Log Exception
            //2. Friendly message

            ModelState.AddModelError(string.Empty, ex.Message);
            return View(employeeVM);
        }
    }
}
