namespace CarsWebApp.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class FirstExtras
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Air Conditioning")]
        public bool AirConditioning { get; set; }

        [Display(Name = "Climatronic")]
        public bool Climatronic { get; set; }

        [Display(Name = "Climate Control")]
        public bool ClimateControl { get; set; }

        [Display(Name = "Alloy Wheels")]
        public bool AlloyWheels { get; set; }

        [Display(Name = "Heated Seats")]
        public bool HeatedSeats { get; set; }

        [Display(Name = "DABradio")]
        public bool DABradio { get; set; }

        [Display(Name = "Cruise Control")]
        public bool CruiseControl { get; set; }

        public string CarId { get; set; }

        public virtual Car Car { get; set; }
    }
}
