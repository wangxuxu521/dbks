using Microsoft.AspNetCore.Mvc;
using System;
using dbks.Models;
using dbks.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace dbks.Controllers
{
    public class AdministratorController: Controller    
    {
        private readonly DbksContext _dbContext;

        public AdministratorController(DbksContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Administrator()
        {
            var position = _dbContext.Positions.ToList(); // 获取所有部门数据
            ViewBag.Posittions = position; // 将职位数据传递给视图
            return View();
        }

        public async Task<IActionResult> AdministratorUser(string searchString)
        {
            using (var context = new DbksContext())
            {
                var employees = from e in context.Employees
                                select e;

                if (!string.IsNullOrEmpty(searchString))
                {
                    employees = employees.Where(s => s.Name.Contains(searchString) );
                }

                return View(await employees.ToListAsync());
            }
        }
        public async Task<IActionResult> AdministratorUser1(string searchString)
        {
            using (var context = new DbksContext())
            {
                // 初始化查询，选择所有员工，并预先加载相关的薪资和部门信息
                var employees = context.Employees
                    .Include(e => e.Salary)
                    .Include(e => e.Dept) // 假设Employee模型中有Department导航属性
                    .Include(e => e.Position) // 假设Employee模型中有Position导航属性
                    .AsQueryable();

                // 如果搜索字符串不为空或null，则添加过滤条件
                if (!string.IsNullOrEmpty(searchString))
                {
                    // 使用Contains方法过滤出名字中包含搜索字符串的员工
                    employees = employees.Where(e => e.Name.Contains(searchString));
                }

                // 将过滤后的员工列表转换为List，并异步加载数据
                var employeeList = await employees.ToListAsync();

                // 将员工列表传递给视图进行显示
                return View(employeeList);
            }
        }


        public IActionResult AddEmployee()
        {
            var positions= _dbContext.Positions.ToList(); // 获取所有职位数据
            var departments = _dbContext.Departments.ToList(); // 获取所有职位数据
            ViewBag.Positions = positions; // 将部门数据传递给视图
            ViewBag.Department = departments;

            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            var positions = await _dbContext.Positions.ToListAsync();
            var departments = await _dbContext.Departments.ToListAsync();
            ViewBag.Positions = positions;

            if (ModelState.IsValid)
            {
                // 获取部门和职位列表，以便验证DeptId和PositionId是否存在

                // 验证DeptId和PositionId是否存在于数据库中
                if (!departments.Any(d => d.DeptId == employee.DeptId))
                {
                    ModelState.AddModelError("DeptId", "部门ID不存在");
                }

                if (!positions.Any(p => p.PositionId == employee.PositionId))
                {
                    ModelState.AddModelError("PositionId", "职位ID不存在");
                }

                // 如果验证通过，保存员工信息到数据库
                if (ModelState.IsValid)
                {
                    _dbContext.Employees.Add(employee);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("AdministratorUser1"); // 或者返回一个成功消息视图
                }
            }   

            // 如果模型状态无效，重新显示表单并显示错误消息
            //ViewBag.Positions = positions; // 使用正确的变量名
            return View(employee);
        }
        public IActionResult AdministratorUser2()
        {
            return View();
        }


        public ActionResult AdminEdit(string id)
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
        public ActionResult AdminEdit(Employee updatedEmployee)
        {
            // 处理表单提交，更新员工信息
            if (ModelState.IsValid)
            {
                _dbContext.Employees.Update(updatedEmployee);
                _dbContext.SaveChanges();
                return RedirectToAction("AdministratorUser1", "Administrator"); // 或者返回到员工列表视图
            }

            // 如果模型状态无效，重新加载数据并返回编辑视图
            ViewBag.Positions = _dbContext.Positions.ToList();
            ViewBag.Departments = _dbContext.Departments.ToList();
            return View(updatedEmployee);
        }

        public IActionResult LoginUser(Administrator admin)
        {
            using (var db = new DbksContext())
            {
                var User = db.Administrators.Find(admin.Administratorid);
                if(User==null)
                {
                    ViewData["Message"] = "无效ID或Name错误";
                    return View("Administrator");
                }
                else if (admin.Administratorname == User.Administratorname && admin.Administratorid == User.Administratorid) // Example check
                {
                    ViewData["Message"] = "登录成功";
                    return RedirectToAction("AdministratorUser", "Administrator"); // Redirect on successful login
                }
                else
                {
                    ViewData["Message"] = "无效ID或Name错误";
                    return View("Administrator");
                }
            }
            return View();
        }

        public async Task<IActionResult> AdministratorUser3(string name,string employId,string position,string deptId)
        {
            using (var context = new DbksContext())
            {
                // 初始化查询，选择所有员工，并预先加载相关的薪资和部门信息
                var employees = context.Employees
                    .Include(e => e.Salary)
                    .Include(e => e.Dept) // 假设Employee模型中有Department导航属性
                    .Include(e => e.Position) // 假设Employee模型中有Position导航属性
                    .AsQueryable();

                // 如果搜索字符串不为空或null，则添加过滤条件

                if (!string.IsNullOrEmpty(name))
                {
                    employees = employees.Where(s => s.Name.Contains(name));

                }
                if (!string.IsNullOrEmpty(employId))
                {
                    employees = employees.Where(s => s.EmployeeId.Contains(employId));
                }
                if (!string.IsNullOrEmpty(position))
                {
                    employees = employees.Where(s => s.Position.PositionId.Contains(position));
                }
                if (!string.IsNullOrEmpty(deptId))
                {
                    employees = employees.Where(s => s.Dept.DeptId.Contains(deptId));
                }

                return View(await employees.ToListAsync());

            }
            
        }
    }

                
 }

