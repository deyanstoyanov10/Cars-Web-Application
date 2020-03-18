namespace CarsWebApp.Controllers
{
    using System.Linq;
    using System.Globalization;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using CarsWebApp.Services.Contracts;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ICarService carService;
        private readonly IMessageService messageService;

        public ValuesController(UserManager<IdentityUser> userManager,
            ICarService carService,
            IMessageService messageService)
        {
            this.userManager = userManager;
            this.carService = carService;
            this.messageService = messageService;
        }

        [HttpGet]
        public async Task<string> GetModelById(int? modelId)
        {
            if (modelId == null)
            {
                return "Error";
            }
            var models = await this.carService.GetModelsById(modelId);
            return await Task.Run(() => JsonConvert.SerializeObject(models));
        }

        [HttpGet]
        public async Task<string> GetChatList(string username)
        {
            if (username == null || this.userManager.Users.Where(x => x.UserName == username).FirstOrDefault() == null)
            {
                return "Error";
            }

            var listResult = await this.messageService.GetChatList(username);
            return listResult;
        }

        public async Task<string> GetMessages(string from, string to)
        {
            if (from == null || to == null)
            {
                return "Error";
            }

            var messages = await this.messageService.GetMessages(from, to);
            return messages;
        }

        public async Task<string> NotificationMessages(string username)
        {
            if (username == null || this.userManager.Users.Where(x => x.UserName == username).FirstOrDefault() == null)
            {
                return "Error";
            }

            var count = await this.messageService.NotificationMessages(username);
            return count;
        }

        public async Task<string> PredictCarPrice(int? brandType, int? modelType, int? year, int? fuelType, int? mileage, int? horsePower)
        {
            if (brandType == null || modelType == null || year == null || horsePower == null)
            {
                return "Error";
            }

            var outputScore = await this.carService.PredictCarPrice(brandType, modelType, year, fuelType, mileage, horsePower);

            return outputScore.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
