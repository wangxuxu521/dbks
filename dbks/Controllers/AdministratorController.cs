using Microsoft.AspNetCore.Mvc;
using System;
using dbks.Models;
using dbks.Controllers;
using Microsoft.EntityFrameworkCore;
namespace dbks.Controllers
{
    public class AdministratorController: Controller    
    {
        public IActionResult Administrator()
        {
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
                var employees = from e in context.Employees
                                select e;

                if (!string.IsNullOrEmpty(searchString))
                {
                    employees = employees.Where(s => s.Name.Contains(searchString));
                }

                return View(await employees.ToListAsync());
            }
        }
        public IActionResult AdministratorUser2()
        {
            return View();
        }
        public IActionResult AdministratorUser3()
        {
            return View();
        }
        public IActionResult LoginUser(Administrator admin)
        {
            using (var db = new DbksContext())
            {
                var User = db.Administrators.Find(admin.Administratorid);
                if (admin.Administratorname == User.Administratorname && admin.Administratorid == User.Administratorid) // Example check
                {
                    ViewData["Message"] = "登录成功";
                    return RedirectToAction("AdministratorUser", "Administrator"); // Redirect on successful login
                }
                else
                {
                    ViewData["Message"] = "用户名或密码错误";
                }
            }
            return View();
        }

    }

                
 }

