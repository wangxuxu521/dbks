using Microsoft.AspNetCore.Mvc;
using System;
using dbks.Models;
using dbks.Controllers;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Reflection.PortableExecutable;



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
                    .Include(e => e.Salaries)
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
        // GET: EmployeeLeave
        public ActionResult EmployeeLeave(string id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return RedirectToAction("AdministratorUser1", "Administrator"); // 或者返回一个错误视图
            }
            // 获取所有职位和部门数据
            ViewBag.Positions = _dbContext.Positions.ToList();
            ViewBag.Departments = _dbContext.Departments.ToList();

            // 将员工信息传递给视图
            return View(employee);
        }

        // POST: EmployeeLeave
        [HttpPost]
        public ActionResult EmployeeLeave(Employee updatedEmployee)
        {
            if (ModelState.IsValid)
            {
                // 更新员工的在职状态为“离职”
                updatedEmployee.OnJob = "离职";

                try
                {
                    // 尝试保存更改，触发器将处理其他属性的更新
                    _dbContext.Employees.Update(updatedEmployee);
                    _dbContext.SaveChanges();

                    // 如果成功更新，可以重定向到员工列表或其他页面
                    return RedirectToAction("Index", "Employee");
                }
                catch (DbUpdateException ex)
                {
                    // 如果触发器阻止了更新，捕获异常并返回错误信息
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("Cannot update data for a leaved employee."))
                    {
                        ModelState.AddModelError("Error", "Cannot update data for a leaved employee.");
                        return View(updatedEmployee);
                    }
                    else
                    {
                        throw; // 其他类型的异常，重新抛出
                    }
                }
            }

            // 如果模型状态无效，重新加载数据并返回编辑视图
            return View(updatedEmployee);
        }
        public ActionResult ImportEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportEmployee(IFormFile file, string selectedColumns)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
                return Content("File not selected");

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                var employees = new List<Employee>();

                try
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        var selectedColumnNames = selectedColumns.Split(',');
                        var selectedColumnIndices = selectedColumnNames
                            .Select(header => worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns]
                                .First(cell => cell.Text == header).Start.Column)
                            .ToArray();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var employee = new Employee();

                            for (int i = 0; i < selectedColumnIndices.Length; i++)
                            {
                                var columnIndex = selectedColumnIndices[i];
                                var cellValue = worksheet.Cells[row, columnIndex].Text.Trim();

                                // 动态设置Employee对象的属性
                                switch (selectedColumnNames[i])
                                {
                                    case "EmployeeId":
                                        employee.EmployeeId = cellValue;
                                        break;
                                    case "Idnumber":
                                        employee.Idnumber = cellValue;
                                        break;
                                    case "Name":
                                        employee.Name = cellValue;
                                        break;
                                    case "DeptId":
                                        employee.DeptId = cellValue;
                                        break;
                                    case "PositionId":
                                        employee.PositionId = cellValue;
                                        break;
                                    case "SalaryId":
                                        employee.SalaryId = cellValue;
                                        break;
                                    case "OnJob":
                                        employee.OnJob = cellValue;
                                        break;
                                    case "Age":
                                        if (int.TryParse(cellValue, out int age))
                                            employee.Age = age;
                                        break;
                                    case "Gender":
                                        employee.Gender = cellValue;
                                        break;
                                    case "Phone":
                                        employee.Phone = cellValue;
                                        break;
                                    case "Email":
                                        employee.Email = cellValue;
                                        break;
                                    case "Address":
                                        employee.Address = cellValue;
                                        break;
                                    case "FamilyInfo":
                                        employee.FamilyInfo = cellValue;
                                        break;
                                        // 添加更多字段映射...
                                }
                            }

                            // 验证DeptID和PositionID是否存在
                            if (!_dbContext.Departments.Any(d => d.DeptId == employee.DeptId) ||
                                !_dbContext.Positions.Any(p => p.PositionId == employee.PositionId))
                            {
                                // 如果DeptID或PositionID不存在，则跳过此员工
                                continue;
                            }

                            // 验证SalaryID是否存在，如果不存在则添加到Salary表
                            var salary = _dbContext.Salaries.FirstOrDefault(s => s.SalaryId == employee.SalaryId);
                            if (salary == null)
                            {
                                salary = new Salary { SalaryId = employee.SalaryId };
                                _dbContext.Salaries.Add(salary);
                            }

                            employees.Add(employee);
                        }

                        _dbContext.Employees.AddRange(employees);
                        try
                        {
                            _dbContext.SaveChanges();
                        }
                        catch (DbUpdateException ex)
                        {
                            // 检查内部异常以获取更详细的错误信息
                            if (ex.InnerException != null)
                            {
                                return Content("Error importing file: " + ex.InnerException.Message);
                            }
                            else
                            {
                                return Content("Error importing file: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Content("Error importing file: " + ex.Message);
                }
            }

            return RedirectToAction("AdministratorUser1", "Administrator");
        }

        public async Task<IActionResult> AdministratorUser2(string searchString)
        {
            using (var context = new DbksContext())
            {
                // 初始化查询，选择所有员工，并预先加载相关的薪资和部门信息
                var employees = context.Employees
                    .Include(e => e.Dept) // 假设Employee模型中有Department导航属性
                    .Include(e => e.Position) // 假设Employee模型中有Position导航属性
                    .AsQueryable();

                // 如果搜索字符串不为空或null，则添加过滤条件
                if (!string.IsNullOrEmpty(searchString))
                {
                    // 使用Contains方法过滤出名字中包含搜索字符串的员工
                    employees = employees.Where(e => e.Name.Contains(searchString));
                }

                // 使用连表查询获取员工和薪资信息，并按员工分组
                var employeeSalaries = await (from e in employees
                                              join s in context.Salaries on e.SalaryId equals s.SalaryId into salariesGroup
                                              from s in salariesGroup.DefaultIfEmpty()
                                              group new { e, s } by new { e.EmployeeId, e.Name } into g
                                              select new EmployeeSalaryViewModel
                                              {
                                                  EmployeeId = g.Key.EmployeeId,
                                                  Name = g.Key.Name,
                                                  Salaries = g.Where(x => x.s != null).Select(x => new SalaryViewModel
                                                  {
                                                      SalaryId = x.s.SalaryId,
                                                      BasicSalary = x.s.BasicSalary,
                                                      PersonalIncome = x.s.PersonalIncome,
                                                      Bonus = x.s.Bonus,
                                                      PayDate = x.s.PayDate,
                                                      TaxRate = x.s.TaxRate
                                                  }).ToList()
                                              }).ToListAsync();

                // 将员工薪资列表传递给视图进行显示
                return View(employeeSalaries);
            }
        }

        public IActionResult AddSalary()
        {
            return View();
        }

        // POST: Salary/AddSalary
        [HttpPost]
        public async Task<IActionResult> AddSalary(Salary salary)
        {
            if (ModelState.IsValid)
            {
                // 检查SalaryID是否在Employee表中存在
                if (!_dbContext.Employees.Any(e => e.SalaryId == salary.SalaryId))
                {
                    ModelState.AddModelError("SalaryId", "SalaryID does not exist in the Employee table.");
                    return View(salary);
                }

                // 保存工资信息到数据库
                _dbContext.Salaries.Add(salary);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index"); // 或者返回一个成功消息视图
            }

            // 如果模型状态无效，重新显示表单并显示错误消息
            return View(salary);
        }
        public ActionResult ImportSalary()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportSalary(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
                return Content("File not selected");

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                var salaries = new List<Salary>();

                try
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        var requiredColumns = new[] { "SalaryID", "Basic_salary", "Bonus", "PayDate" };
                        var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns].Select(cell => cell.Text).ToArray();
                        var columnIndices = requiredColumns.Select(col => Array.IndexOf(headerRow, col)).ToArray();

                        if (columnIndices.Any(index => index == -1))
                        {
                            return Content("Missing required columns in the file.");
                        }

                        for (int row = 2; row <= rowCount; row++)
                        {
                            var salaryId = worksheet.Cells[row, columnIndices[0] + 1].Text.Trim();

                            // 检查SalaryID是否在Employee表中存在
                            if (!_dbContext.Employees.Any(e => e.SalaryId == salaryId))
                            {
                                continue; // 跳过此记录
                            }

                            var salary = new Salary();

                            for (int i = 0; i < requiredColumns.Length; i++)
                            {
                                var cellValue = worksheet.Cells[row, columnIndices[i] + 1].Text.Trim();

                                switch (requiredColumns[i])
                                {
                                    case "SalaryID":
                                        salary.SalaryId = cellValue;
                                        break;
                                    case "Basic_salary":
                                        if (decimal.TryParse(cellValue, out decimal basicSalary))
                                            salary.BasicSalary = basicSalary;
                                        break;
                                    case "Bonus":
                                        if (decimal.TryParse(cellValue, out decimal bonus))
                                            salary.Bonus = bonus;
                                        break;
                                    case "PayDate":
                                        if (DateOnly.TryParse(cellValue, out DateOnly payDate))
                                            salary.PayDate = payDate;
                                        break;
                                }
                            }

                            salaries.Add(salary);
                        }

                        _dbContext.Salaries.AddRange(salaries);
                        try
                        {
                            _dbContext.SaveChanges();
                        }
                        catch (DbUpdateException ex)
                        {
                            if (ex.InnerException != null)
                            {
                                return Content("Error importing file: " + ex.InnerException.Message);
                            }
                            else
                            {
                                return Content("Error importing file: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Content("Error importing file: " + ex.Message);
                }
            }

            return RedirectToAction("AdministratorUser1", "Administrator");
        }

        public async Task<IActionResult> MakeSalaryByDepartment()
        {
            // 查询按部门分类的员工薪资信息
            // 查询按部门分类的员工薪资信息
            var salaryInfo = await _dbContext.Departments
                .Include(d => d.Employees)
                .ThenInclude(e => e.Salaries)
                .Select(d => new DepartmentSalaryViewModel
                {
                    DeptID = d.DeptId,
                    DeptName = d.DeptName,
                    Employees = d.Employees.Select(e => new EmployeeSalaryDetailsViewModel
                    {
                        EmployeeID = e.EmployeeId,
                        Name = e.Name,
                        PositionID = e.PositionId,
                        SalaryID = e.SalaryId,
                        Salary_M = e.Salaries.SalaryM,
                        Basic_salary = e.Salaries.BasicSalary,
                        Personal_income = e.Salaries.PersonalIncome,
                        Bonus = e.Salaries.Bonus,
                        PayDate = e.Salaries.PayDate
                    }).ToList()
                }).ToListAsync();


            return View(salaryInfo);
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
                    .Include(e => e.Salaries)
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
        public IActionResult AdministratorUser4()
        {
            List<Department> departments = _dbContext.Departments.ToList();
            List<Employee> employees = _dbContext.Employees.ToList();

            ViewBag.Departments = departments;
            ViewBag.Employees = employees;

            return View();
        }

        public IActionResult GetStatistics(string department, string employeeId, DateOnly? startDate, DateOnly? endDate)
        {
            var employees = _dbContext.Employees
                .Include(e => e.Dept)
                .Include(e => e.Position)
                .Include(e => e.Salaries);

            if (!string.IsNullOrEmpty(department))
            {
                employees = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Employee, Salary?>)employees.Where(e => e.Dept.DeptId == department);
            }

            if (!string.IsNullOrEmpty(employeeId))
            {
                employees = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Employee, Salary?>)employees.Where(e => e.EmployeeId == employeeId);
            }

            var results = employees.ToList(); // 将数据加载到内存中

            if (startDate.HasValue && endDate.HasValue)
            {
                results = results.Where(e =>
                {
                    if (e.Salaries == null)
                        return false; // 如果 e.Salary 为 null，则不满足条件
                    return e.Salaries.PayDate >= startDate && e.Salaries.PayDate <= endDate;
                }).ToList();
            }

           
            return View(results);
        }
        
    }
        

}

 

                

