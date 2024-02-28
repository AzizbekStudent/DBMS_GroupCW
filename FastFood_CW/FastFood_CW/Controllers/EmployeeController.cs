using FastFood_CW.DAL.Interface;
using FastFood_CW.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastFood_CW.Controllers
{
    // Students ID: 00013836, 00014725, 00014896
    public class EmployeeController : Controller
    {
        private readonly IRepository<Employee> _repository;

        public EmployeeController(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        // Get All employees
        public async Task<IActionResult> Index()
        {
            TempData["RepositoryName"] = _repository.GetType().Name;

            var empList = await _repository.GetAllAsync();

            return View(empList);
        }

        // Get particular Employee
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // Create new Employee
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee emp,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] imageData = null;
                    if (image != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            imageData = memoryStream.ToArray();
                            emp.Image = imageData;
                        }
                    }

                    var employee = new Employee
                    {
                        FName = emp.FName,
                        LName = emp.LName,
                        Telephone = emp.Telephone,
                        Job = emp.Job,
                        Age = emp.Age,
                        Salary = emp.Salary,
                        HireDate = emp.HireDate,
                        Image = emp.Image, 
                        FullTime = emp.FullTime 
                    };

                    int id = await _repository.CreateAsync(employee);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(emp);
        }

        // Updating Information about employee
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee emp, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _repository.GetByIdAsync(id);

                    if (employee == null)
                    {
                        return NotFound();
                    }

                    employee.FName = emp.FName;
                    employee.LName = emp.LName;
                    employee.Telephone = emp.Telephone;
                    employee.Job = emp.Job;
                    employee.Age = emp.Age;
                    employee.Salary = emp.Salary;
                    employee.HireDate = emp.HireDate;
                    employee.FullTime = emp.FullTime;

                    if (image != null && image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            employee.Image = memoryStream.ToArray();
                        }
                    }

                    await _repository.UpdateAsync(employee);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var emp = await _repository.GetByIdAsync(id);
                if (emp != null)
                {
                    await _repository.DeleteAsync(emp);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteErrors"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
