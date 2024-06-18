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

    }

}
