namespace CarsWebApp.Models.Enums
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum DoorsType
    {
        [Display(Name = "2/3")]
        [Description("2/3")]
        First = 0,
        [Display(Name = "4/5")]
        [Description("4/5")]
        Second = 1
    }
}
