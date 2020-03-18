namespace CarsWebApp.Services.Contracts
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using CarsWebApp.Models;

    public interface ICarService
    {
        Task<List<Brand>> GetBrands();

        Task<List<Model>> GetModelsById(int? modelId);

        Task AddCar(CarModel carModel, string username);

        Task<List<string>> GetBooleanPropertyNames(object obj);

        Task<List<Car>> GetLatestCars();

        Task<CarViewModel> CreateCarViewModel(long searchKey);

        Task<double> PredictCarPrice(int? brandType, int? modelType, int? year, int? fuelType, int? mileage, int? horsePower);
    }
}
