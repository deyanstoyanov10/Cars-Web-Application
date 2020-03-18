namespace CarsWebApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Brand
    {
        public Brand()
        {
            this.Models = new HashSet<Model>();
            this.Cars = new HashSet<Car>();
        }

        [Key]
        public int Id { get; set; }

        public string BrandType { get; set; }

        public virtual ICollection<Model> Models { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
