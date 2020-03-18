namespace CarsWebApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SecondExtras
    {
        [Key]
        public int Id { get; set; }

        public bool DualAirbag { get; set; }

        public bool KeylessEntry { get; set; }

        public bool PowerMirrors { get; set; }

        public bool PowerSeat { get; set; }

        public bool PowerSteering { get; set; }

        public bool AntiLockBrakes { get; set; }

        public bool AntiTheft { get; set; }

        public bool AntiStarter { get; set; }

        public string CarId { get; set; }

        public virtual Car Car { get; set; }
    }
}
