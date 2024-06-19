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
        public IActionResult AdministratorUser(Administrator admin)
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
            return View("Login");
        }

    }

                
 }

