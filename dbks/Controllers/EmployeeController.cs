using Microsoft.AspNetCore.Mvc;
using dbks.Controllers;
using dbks.Models;
namespace dbks.Controllers
{
    public class EmployeeController:Controller
    {
        public IActionResult Employee()

        {
            return View();
        }
    }
}
