using lab12.Models;
using lab12.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace lab12.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CalcService _calcService;

		public HomeController(
            ILogger<HomeController> logger,
            CalcService calcService
        )
        {
            _logger = logger;
            _calcService = calcService;
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManualSingle()
        {
            if (Request.Method == "POST")
            {
                ViewData["data"] = _checkInput();

				return View("Result");
			}

			ViewData["Title"] = "Manual";

			return View("Input");
        }

		[HttpGet, ActionName("ManualSeparate")]
		public IActionResult ManualSeparateGet()
        {
			ViewData["Title"] = "ManualWithSeparateHandlers";

			return View("Input");
        }

		[HttpPost, ActionName("ManualSeparate")]
		public IActionResult ManualSeparatePost()
		{
			ViewData["data"] = _checkInput();

			return View("Result");
		}

		[HttpGet]
		public IActionResult ModelBindingParams()
        {
			ViewData["Title"] = "ModelBindingInParameters";

			return View("Input");
        }

		[HttpPost]
		public IActionResult ModelBindingParams(int firstValue, string operation, int secondValue)
		{
			if (ModelState.IsValid)
			{
				ViewData["data"] = _calcService.calc(firstValue, operation, secondValue);
			}
			else
			{
				ViewData["data"] = "Incorrect input data";
			}
		
			return View("Result");
		}

		[HttpGet]
		public IActionResult ModelBindingSeparate()
        {
			ViewData["Title"] = "ModelBindingInSeparateModel";

			return View("Input");
        }

		[HttpPost]
		public IActionResult ModelBindingSeparate(FormData data)
		{
			ViewData["data"] = ModelState.IsValid
				? _calcService.calc(data.firstValue, data.operation, data.secondValue)
				: "Incorrect input data";

			return View("Result");
		}

		private string _checkInput()
        {
            var result = "";

			try
            {
				result = _calcService.calc(
					Int32.Parse(Request.Form["firstValue"]),
					Request.Form["operation"],
					Int32.Parse(Request.Form["secondValue"])
				);

                return result;
			} 
            catch(Exception ex)
            {
				result = "Incorrect input data";

				return result;
			}
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}