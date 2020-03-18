namespace CarsWebApp.Services
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CarsWebApp.Data;
    using CarsWebApp.Models;
    using CarsWebApp.Models.Enums;
    using CarsWebApp.Services.Contracts;

    public class ListCarsService : IListCars
    {
        private CarsDbContext context;

        public ListCarsService(CarsDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Car>> GetListedCars(SearchModel searchModel, bool active)
        {
            var properties = searchModel.GetType().GetProperties();

            var carsList = await this.context.Cars.ToListAsync();

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(searchModel) != null)
                {
                    switch (properties[i].Name.ToLower().ToString())
                    {
                        case "bodytype": carsList = carsList.Where(x => x.BodyType == (BodyType)int.Parse(searchModel.BodyType)).ToList(); break;
                        case "brandtype": carsList = carsList = carsList.Where(x => x.BrandId == int.Parse(searchModel.BrandType)).ToList(); break;
                        case "modeltype": carsList = carsList.Where(x => x.ModelId == int.Parse(searchModel.ModelType)).ToList(); break;
                        case "fueltype": carsList = carsList.Where(x => x.FuelType == (FuelType)int.Parse(searchModel.FuelType)).ToList(); break;
                        case "transmissiontype": carsList = carsList.Where(x => x.Transmission == (TransmissionType)int.Parse(searchModel.TransmissionType)).ToList(); break;
                        case "minyear": carsList = carsList.Where(x => x.Year.Year >= int.Parse(searchModel.MinYear)).ToList(); break;
                        case "minprice": carsList = carsList.Where(x => x.Price >= int.Parse(searchModel.MinPrice)).ToList(); break;
                        case "maxyear": carsList = carsList.Where(x => x.Year.Year <= int.Parse(searchModel.MaxYear)).ToList(); break;
                        case "maxprice": carsList = carsList.Where(x => x.Price <= int.Parse(searchModel.MaxPrice)).ToList(); break;
                        default:
                            break;
                    }
                }
            }

            carsList = carsList.Where(x => x.IsActive == active).ToList();

            return carsList;
        }

        public string GetBrandName(int id)
        {
            string brandType = this.context.Brands.Where(x => x.Id == id).Select(x => x.BrandType).FirstOrDefault();

            return brandType;
        }

        public string GetModelName(int id)
        {
            string modelType = this.context.Models.Where(x => x.Id == id).Select(x => x.ModelType).FirstOrDefault();

            return modelType;
        }

        public string GetPicture(string carId)
        {
            string picture = Convert.ToBase64String(this.context.Pictures.Where(x => x.CarId == carId).Select(x => x.Picture).FirstOrDefault());

            return picture;
        }

        public string GetReturnUrlWithParameters(string action, string controller, object model, int page)
        {
            var sb = new StringBuilder();
            sb.Append("/").Append(controller).Append("/").Append(action).Append("/?").Append("page=").Append(page);

            var properties = model.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetValue(model) == null)
                {
                    continue;
                }
                else
                {
                    sb.Append("&").Append(properties[i].Name).Append("=").Append(properties[i].GetValue(model));
                }
            }
            return sb.ToString();
        }
    }
}
