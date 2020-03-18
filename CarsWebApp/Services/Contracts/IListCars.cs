namespace CarsWebApp.Services.Contracts
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using CarsWebApp.Models;

    public interface IListCars
    {
        Task<IEnumerable<Car>> GetListedCars(SearchModel searchModel, bool active);

        string GetBrandName(int id);

        string GetModelName(int id);

        string GetPicture(string carId);

        string GetReturnUrlWithParameters(string action, string controller, object model, int page);
    }
}
