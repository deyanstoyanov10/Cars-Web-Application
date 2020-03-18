namespace CarsWebApp.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CarsWebApp.Services.Contracts;

    public class CarController : Controller
    {
        private ICarService carService;
        private readonly IMessageService messageService;

        public CarController(ICarService carService, IMessageService messageService)
        {
            this.carService = carService;
            this.messageService = messageService;
        }

        public async Task<IActionResult> Index(long searchKey)
        {
            var model = await this.carService.CreateCarViewModel(searchKey);

            return await Task.Run(() => View(model));
        }

        [HttpPost]
        public async Task SendMessage(string from, string to, string message)
        {
            if (from != null && to != null && message != null)
            {
                if (from != to)
                {
                    await this.messageService.SendMessage(from, to, message);
                }
            }
        }
    }
}