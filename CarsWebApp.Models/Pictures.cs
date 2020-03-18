using System.ComponentModel.DataAnnotations;

namespace CarsWebApp.Models
{
    public class Pictures
    {
        [Key]
        public int Id { get; set; }

        public byte[] Picture { get; set; }

        public string CarId { get; set; }

        public Car Car { get; set; }
    }
}
