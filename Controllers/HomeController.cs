using CoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml.Linq;

namespace CoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserInfo User)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(User.FirstName) || string.IsNullOrWhiteSpace(User.LastName))
                {
                    ViewBag.ErrorMessage = "FirstName and LastName are required fields";

                }
                else
                {
                    User.Id = Guid.NewGuid().ToString("N");

                    User.CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ");


                    string UserJSON = JsonConvert.SerializeObject(User);


                    string FolderPath = Directory.GetCurrentDirectory() + "\\SavedData\\";
                    // Save user data in json file in UserData folder
                    // if UserData folder does not exists then create new folder
                    if (!Directory.Exists(FolderPath))
                    {
                        Directory.CreateDirectory(FolderPath);
                    }

                    System.IO.File.WriteAllText(FolderPath + $"User_{User.Id}.json", UserJSON);

                    ViewBag.SuccessMessage = "User saved successfully.";
                }
            }
            catch (Exception ex)            {

                ViewBag.ErrorMessage = "Some error occured please try again.";

                _logger.Log(LogLevel.Error, ex, ex.ToString());
            }

            return View();
        }

        public IActionResult Privacy()
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