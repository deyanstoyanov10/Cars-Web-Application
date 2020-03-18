namespace CarsWebApp.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CarsWebApp.Models;
    using CarsWebApp.Services.Contracts;

    [ViewComponent(Name = "ListCars")]
    public class ListCarsViewComponent : ViewComponent 
    {
        private readonly IListCars listCars;

        public ListCarsViewComponent(IListCars listCars)
        {
            this.listCars = listCars;
        }

        public async Task<IViewComponentResult> InvokeAsync(ListCarsViewModel viewModel)
        {
            viewModel.ListedCars = await this.listCars.GetListedCars(viewModel.SearchModel, true);
            return await Task.Run(() => this.View(viewModel));
        }
    }
}
