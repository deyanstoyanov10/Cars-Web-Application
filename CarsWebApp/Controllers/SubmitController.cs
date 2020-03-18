namespace CarsWebApp.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Authorization;

    using CarsWebApp.Models;
    using CarsWebApp.Services.Contracts;

    public class SubmitController : Controller
    {
        private ICarService carService;

        public SubmitController(ICarService carService)
        {
            this.carService = carService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCar(CarModel carModel, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in files)
                {
                    if (item.Length > 0)
                    {
                        byte[] picture = null;

                        using (var stream = new MemoryStream())
                        {
                            await item.CopyToAsync(stream);
                            picture = stream.ToArray();
                        }

                        carModel.Pictures.Add(picture);
                    }
                }
                await this.carService.AddCar(carModel, this.User.Identity.Name.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                return View(carModel);
            }
        }
    }
}
