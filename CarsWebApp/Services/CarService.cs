namespace CarsWebApp.Services
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.Extensions.ML;

    using CarsWebApp.Data;
    using CarsWebApp.Models;
    using CarsWebAppML.Model;
    using CarsWebApp.Models.Enums;
    using CarsWebApp.Services.Contracts;

    public class CarService : ICarService
    {
        private CarsDbContext context;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> predictionEngine;

        public CarService(CarsDbContext context, PredictionEnginePool<ModelInput, ModelOutput> predictionEngine)
        {
            this.context = context;
            this.predictionEngine = predictionEngine;
        }

        public async Task<List<Brand>> GetBrands()
        {
            var brands = this.context.Brands.OrderBy(x => x.BrandType).ToList();
            return await Task.Run(() => brands);
        }

        public async Task<List<Model>> GetModelsById(int? modelId)
        {
            var models = this.context.Models.Where(x => x.BrandId == modelId).ToList();
            return await Task.Run(() => models);
        }

        public async Task AddCar(CarModel carModel, string username)
        {
            var newId = Guid.NewGuid().ToString();
            var createSearchKey = Enumerable.Range(1, 101).OrderBy(g => Guid.NewGuid()).Take(5).ToArray();
            var result = new StringBuilder();

            foreach (var item in createSearchKey)
            {
                result.Append(item);
            }

            var searchKey = long.Parse(result.ToString());

            await this.context.Cars.AddAsync(new Car()
            {
                Id = newId,
                SearchKey = searchKey,
                BrandId = carModel.BrandType,
                ModelId = carModel.ModelType,
                State = carModel.State,
                Price = carModel.Price,
                BodyType = carModel.BodyType,
                Year = carModel.CarYear,
                Mileage = carModel.Mileage,
                FuelType = carModel.FuelType,
                Transmission = carModel.Transmission,
                HorsePower = carModel.HorsePower,
                EngineVolume = carModel.EngineVolume,
                EngineModification = carModel.EngineModification,
                Doors = carModel.Doors,
                Metallic = carModel.Metallic,
                EuroStandart = carModel.EuroStandartType,
                Description = carModel.Description,
                AddedBy = username,
                AddedOn = DateTime.UtcNow,
                IsActive = true
            });

            foreach (var item in carModel.Pictures)
            {
                await this.context.Pictures.AddAsync(new Pictures()
                {
                    Picture = item,
                    CarId = newId
                });
            }

            carModel.FirstExtras.CarId = newId;
            carModel.SecondExtras.CarId = newId;
            carModel.ThirdExtras.CarId = newId;

            await this.context.FirstExtras.AddAsync(carModel.FirstExtras);
            await this.context.SecondExtras.AddAsync(carModel.SecondExtras);
            await this.context.ThirdExtras.AddAsync(carModel.ThirdExtras);

            await this.context.SaveChangesAsync();
        }
        
        public async Task<List<string>> GetBooleanPropertyNames(object obj)
        {
            var property = obj.GetType().GetProperties().Where(x => x.PropertyType == typeof(bool)).ToList();
            var propertyNames = new List<string>();

            for (int i = 0; i < property.Count; i++)
            {
                propertyNames.Add(Convert.ToString(property[i].ToString().Replace("Boolean","").Trim()));
            }
            
            return await Task.Run(() => propertyNames);
        }

        

        public async Task<List<Car>> GetLatestCars()
        {
            var latestCars = this.context.Cars.OrderByDescending(x => x.AddedOn).Take(7).ToList();
            return await Task.Run(() => latestCars);
        }

        public async Task<CarViewModel> CreateCarViewModel(long searchKey)
        {
            var car = this.context.Cars.Where(x => x.SearchKey == searchKey).FirstOrDefault();
            string brandType = this.context.Brands.Where(x => x.Id == car.BrandId).Select(x => x.BrandType).FirstOrDefault();
            string modelType = this.context.Models.Where(x => x.Id == car.ModelId).Select(x => x.ModelType).FirstOrDefault();
            var firstExtras = await GetFirstExtras(searchKey);
            var secondExtras = await GetSecondExtras(searchKey);
            var thirdExtras = await GetThirdExtras(searchKey);
            var pictures = this.context.Pictures.Where(x => x.CarId == car.Id).Select(x => x.Picture).ToList();

            CarViewModel model = new CarViewModel()
            {
                BrandType = brandType,
                ModelType = modelType,
                State = car.State,
                Price = car.Price,
                BodyType = car.BodyType,
                CarYear = car.Year,
                Mileage = car.Mileage,
                FuelType = car.FuelType,
                Transmission = car.Transmission,
                HorsePower = car.HorsePower,
                EngineVolume = car.EngineVolume,
                EngineModification = car.EngineModification,
                Doors = car.Doors,
                Metallic = car.Metallic,
                EuroStandart = car.EuroStandart,
                Description = car.Description,
                AddedBy = car.AddedBy,
                FirstExtras = firstExtras,
                SecondExtras = secondExtras,
                ThirdExtras = thirdExtras,
                Pictures = pictures
            };

            return await Task.Run(() => model);
        }

        private async Task<FirstExtras> GetFirstExtras(long searchKey)
        {
            var carId = await GetCarIdBySearchKey(searchKey);
            var firstExtras = this.context.FirstExtras.Where(x => x.CarId == carId).FirstOrDefault();
            return firstExtras;
        }

        private async Task<SecondExtras> GetSecondExtras(long searchKey)
        {
            var carId = await GetCarIdBySearchKey(searchKey);
            var secondExtras = this.context.SecondExtras.Where(x => x.CarId == carId).FirstOrDefault();
            return secondExtras;
        }

        private async Task<ThirdExtras> GetThirdExtras(long searchKey)
        {
            var carId = await GetCarIdBySearchKey(searchKey);
            var thirdExtras = this.context.ThirdExtras.Where(x => x.CarId == carId).FirstOrDefault();
            return thirdExtras;
        }

        private async Task<string> GetCarIdBySearchKey(long searchKey)
        {
            var getCarId = this.context.Cars.Where(x => x.SearchKey == searchKey).Select(x => x.Id).FirstOrDefault();
            return await Task.Run(() => getCarId);
        }

        public async Task<double> PredictCarPrice(int? brandType, int? modelType, int? year, int? fuelType, int? mileage, int? horsePower)
        {
            ModelInput input = new ModelInput();

            string brand = this.context.Brands.Where(x => x.Id == brandType).Select(x => x.BrandType).FirstOrDefault().ToString();
            string model = this.context.Models.Where(x => x.Id == modelType && x.BrandId == brandType).Select(x => x.ModelType).FirstOrDefault().ToString();
            string brandModel = brand + " " + model;

            input.BrandModel = brandModel;
            input.Year = float.Parse(year.ToString());
            input.HorsePower = float.Parse(horsePower.ToString());

            if (fuelType != null)
            {
                input.Fuel = Enum.GetName(typeof(FuelType), fuelType);
            }
            else
            {
                //Default fuel type value
                input.Fuel = "Petrol";
            }

            //here we are making default millage for better prediction
            // default kilometers per year ===> 18 000
            if (mileage == null)
            {
                int years = DateTime.UtcNow.Year - int.Parse(year.ToString());
                var defaultMileage = years * 18000;
                input.Mileage = float.Parse(defaultMileage.ToString());
            }
            else
            {
                input.Mileage = float.Parse(mileage.ToString());
            }

            var output = predictionEngine.Predict(input);
            var outputScore = output.Score / 1.75;

            return await Task.Run(() => outputScore);
        }
    }
}
