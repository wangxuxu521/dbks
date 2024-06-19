using dbks.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dbks.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

        public string Action { get; set; }

        public IActionResult Administrator()
        {
            return View();
        }

        public IActionResult Employee()
            
        {
            return View();
        }
        //��ת��Administrator���溯��
        public IActionResult ToAdministrator()
        {
            return RedirectToAction("Administrator", "Administrator");
        }
        //��ת��Employee���溯��
        public IActionResult ToEmployee()
        {
            return RedirectToAction("Employee", "Employee");
        }



        public IActionResult Main()
        {
            return View();
        }


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
