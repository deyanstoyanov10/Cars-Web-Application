namespace CarsWebApp.Models
{
    using System;
    using System.Collections.Generic;

    using CarsWebApp.Models.Enums;

    public class CarViewModel
    {
        public CarViewModel()
        {
            this.Pictures = new List<byte[]>();
        }

        public string BrandType { get; set; }

        public string ModelType { get; set; }

        public StateType State { get; set; }

        public decimal Price { get; set; }

        public BodyType BodyType { get; set; }

        public string EngineModification { get; set; }

        public DateTime CarYear { get; set; }

        public int Mileage { get; set; }

        public FuelType FuelType { get; set; }

        public TransmissionType Transmission { get; set; }

        public int HorsePower { get; set; }

        public int EngineVolume { get; set; }

        public DoorsType Doors { get; set; }

        public ColorType Color { get; set; }

        public bool Metallic { get; set; }

        public EuroStandartType EuroStandart{ get; set; }

        public string Description { get; set; }

        public string AddedBy { get; set; }

        public FirstExtras FirstExtras { get; set; }

        public SecondExtras SecondExtras { get; set; }

        public ThirdExtras ThirdExtras { get; set; }

        public List<byte[]> Pictures { get; set; }
    }
}
