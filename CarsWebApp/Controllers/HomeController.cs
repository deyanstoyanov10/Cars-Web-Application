namespace CarsWebApp.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using CarsWebApp.Data;
    using CarsWebApp.Models;
    
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Calculator()
        {
            return await Task.Run(() => View());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return await Task.Run(() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }

        [HttpPost]
        public async Task<IActionResult> SearchCar(SearchModel searchModel)
        {
            if (searchModel.ModelType == "-1")
            {
                searchModel.ModelType = null;
            }
            return await Task.Run(() => RedirectToAction("Find", "Search", searchModel));
        }
    }
}
