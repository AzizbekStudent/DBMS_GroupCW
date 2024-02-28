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
        public async Task<IActionResult> Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = await _repository.CreateAsync(emp);
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
        public async Task<IActionResult> Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateAsync(emp);
                    return RedirectToAction("Details", new { id = emp.Employee_ID });
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
