using Microsoft.AspNetCore.Mvc;
using System;
using dbks.Models;
using dbks.Controllers;
namespace dbks.Controllers
{
    public class AdministratorController: Controller    
    {
        public IActionResult Administrator()
        {
            return View();
        }
        public IActionResult AdministratorUser()
        {
            return View();
        }
        public IActionResult LoginUser(Administrator model)
        {
            using(var db = new DbksContext())
            {
                var admin = db.Administrators.Find(model.Administratorname);
                if(admin == null)
                {
                    ViewData["Message"] = "Invalid User Name or Password";
                    return RedirectToAction("Administrator", "Administrator");
                }
                if(model.Administratorid== admin.Administratorid)
                {
                    return RedirectToAction("AdministratorUser", "Administrator");
                }
                else 
                {
                    ViewData["Message"] = "Invalid User Name or Password";
                    return RedirectToAction("Administrator", "Administrator");
                }
            }

        }

    }

}
