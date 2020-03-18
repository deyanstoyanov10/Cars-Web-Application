namespace CarsWebApp.Models
{
    using System.Linq;
    using System.Collections.Generic;

    using CarsWebApp.Data;

    public class BrandsViewModel
    {
        private CarsDbContext context;

        public BrandsViewModel(CarsDbContext context)
        {
            this.context = context;
        }

        public ICollection<Brand> MyProperty => this.context.Brands.ToList();
    }
}
