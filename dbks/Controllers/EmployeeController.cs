using Microsoft.AspNetCore.Mvc;
using dbks.Controllers;
using dbks.Models;
using Microsoft.EntityFrameworkCore;
namespace dbks.Controllers
{
    public class EmployeeController:Controller
    {
        public IActionResult Employee()

        {
            return View();
        }
        public IActionResult EmployeeUser1()

        {
            return View();
        }
        public IActionResult EmployeeUser2()

        {
            return View();
        }
        public async Task<IActionResult> EmployeeUser(string searchString)
        {
            using (var context = new DbksContext())
            {
                var employees = from e in context.Employees
                                select e;

                if (!string.IsNullOrEmpty(searchString))
                {
                    employees = employees.Where(s => s.Name.Contains(searchString));
                }

                return View(await employees.ToListAsync());
            }
        }
        public IActionResult LoginUser(Employee admin)
        {
            using (var db = new DbksContext())
            {
                var User = db.Employees.Find(admin.EmployeeId);
                if (User == null)
                {
                    ViewData["Message"] = "无效ID或Name错误";
                    return View("Employee");
                }
                else if (admin.Name == User.Name && admin.EmployeeId == User.EmployeeId) // Example check
                {
                    ViewData["Message"] = "登录成功";
                    return RedirectToAction("EmployeeUser", "Employee"); // Redirect on successful login
                }
                else
                {
                    ViewData["Message"] = "无效ID或Name错误";
                    return View("Employee");
                }
            }
            return View();
        }
    }
}
