namespace CarsWebApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ThirdExtras
    {
        [Key]
        public int Id { get; set; }

        public bool CDPlayer { get; set; }

        public bool DriverSideAirbag { get; set; }

        public bool PowerWindows { get; set; }

        public bool RemoteStart { get; set; }

        public bool Autopilot { get; set; }

        public string CarId { get; set; }

        public virtual Car Car { get; set; }
    }
}
