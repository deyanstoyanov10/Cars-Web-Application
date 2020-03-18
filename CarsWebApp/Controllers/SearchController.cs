namespace CarsWebApp.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CarsWebApp.Models;

    public class SearchController : Controller
    {
        public async Task<IActionResult> Find(SearchModel searchModel, int page = 1)
        {
            var viewModel = new ListCarsViewModel(searchModel, page);
            return await Task.Run(() => View(viewModel));
        }
    }
}
