namespace CarsWebApp.Models
{
    using System.Collections.Generic;

    public class ListCarsViewModel
    {
        public ListCarsViewModel(SearchModel searchModel, int page)
        {
            this.SearchModel = searchModel;
            this.Page = page;
        }

        public SearchModel SearchModel { get; set; }

        public int Page { get; set; }

        public IEnumerable<Car> ListedCars { get; set; }
    }
}
