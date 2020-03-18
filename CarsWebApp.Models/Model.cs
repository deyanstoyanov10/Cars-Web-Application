namespace CarsWebApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Model
    {
        public Model()
        {
            this.Cars = new HashSet<Car>();
        }

        [Key]
        public int Id { get; set; }

        public string ModelType { get; set; }

        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
