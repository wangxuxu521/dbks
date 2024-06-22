using Microsoft.AspNetCore.Mvc;
using dbks.Controllers;
using dbks.Models;
using Microsoft.EntityFrameworkCore;
namespace dbks.Controllers
{
    public class EmployeeController:Controller
    {
        private readonly DbksContext _dbContext;

        public EmployeeController(DbksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Employee()

        {
            return View();
        }
        public IActionResult EmployeeUser1()
        {
            var employeeId = HttpContext.Session.GetString("EmployeeId");
            using (var dbksContext = new DbksContext())
            {
                var employees = dbksContext.Employees
                    .Include(e => e.Salary)
                    .Include(e => e.Dept) // 假设Employee模型中有Department导航属性
                    .Include(e => e.Position) // 假设Employee模型中有Position导航属性
                    .AsQueryable();

                var employee = dbksContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
        }

       public ActionResult EmployeeEdit(string id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(e => e.EmployeeId ==id);

            if (employee == null)
            {
                return RedirectToAction("Index", "Home"); // 或者返回一个错误视图
            }

            // 获取所有职位和部门数据
            ViewBag.Positions = _dbContext.Positions.ToList();
            ViewBag.Departments = _dbContext.Departments.ToList();

            // 将员工信息传递给视图
            return View(employee);
        }

        [HttpPost]
        public ActionResult EmployeeEdit(Employee updatedEmployee)
        {
            // 处理表单提交，更新员工信息
            if (ModelState.IsValid)
            {
                _dbContext.Employees.Update(updatedEmployee);
                _dbContext.SaveChanges();
                return RedirectToAction("EmployeeUser1", "Employee"); // 或者返回到员工列表视图
            }

            // 如果模型状态无效，重新加载数据并返回编辑视图
            ViewBag.Positions = _dbContext.Positions.ToList();
            ViewBag.Departments = _dbContext.Departments.ToList();
            return View(updatedEmployee);
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
        public IActionResult LoginUser(Employee employee)
        {
            using (var db = new DbksContext())
            {
                var User = db.Employees.Find(employee.EmployeeId);
                if (User == null)
                {
                    ViewData["Message"] = "无效ID或Name错误";
                    return View("Employee");
                }
                else if (employee.EmployeeId == User.EmployeeId) // Example check
                {
                    HttpContext.Session.SetString("EmployeeId", employee.EmployeeId);
                    ViewData["Message"] = "登录成功";
                    return RedirectToAction("EmployeeUser1", "Employee"); // Redirect on successful login
                }
                else
                {
                    ViewData["Message"] = "无效ID或Name错误";
                    return View("Employee");
                }
            }
            return View();
        }

        public IActionResult EmployeeUser2()
        {
            var employeeId = HttpContext.Session.GetString("EmployeeId");

            // 从数据库中获取员工信息和薪资信息
            var employees = _dbContext.Employees
                .Include(e => e.Salary)
                .Include(e => e.Dept)
                .Include(e => e.Position)
                .Where(e => e.EmployeeId == employeeId)
                .ToList();


            //var employees = _dbContext.Employees
            //    .Include(e => e.Dept)
            //    .Include(e => e.Position)
            //    .Include(e => e.Salary)
            //    .Where(e => e.EmployeeId == employeeId)
            //    .ToList();
            



            return View(employees);
        }
    }
}
