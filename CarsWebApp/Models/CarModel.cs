namespace CarsWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CarsWebApp.Models.Enums;

    public class CarModel
    {
        public CarModel()
        {
            this.Pictures = new List<byte[]>();
        }

        [Required(ErrorMessage = "BrandType is required!")]
        public int BrandType { get; set; }

        [Required]
        public int ModelType { get; set; }

        [Required]
        public StateType State { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public BodyType BodyType { get; set; }

        [Required]
        public string EngineModification { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{mm-yyyy}")]
        public DateTime CarYear { get; set; }

        [Required]
        public int Mileage { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public TransmissionType Transmission { get; set; }

        [Required]
        public int HorsePower { get; set; }

        [Required]
        public int EngineVolume { get; set; }

        [Required]
        public DoorsType Doors { get; set; }

        [Required]
        public ColorType Color { get; set; }

        [Required]
        public bool Metallic { get; set; }

        [Required]
        public EuroStandartType EuroStandartType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<byte[]> Pictures { get; set; }

        public FirstExtras FirstExtras { get; set; }

        public SecondExtras SecondExtras { get; set; }

        public ThirdExtras ThirdExtras { get; set; }
    }
}
