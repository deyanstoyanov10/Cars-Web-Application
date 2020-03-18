namespace CarsWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CarsWebApp.Models.Enums;

    public class Car
    {
        public Car()
        {
            this.Pictures = new HashSet<Pictures>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public long SearchKey { get; set; }

        [Required]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        [Required]
        public StateType State { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public BodyType BodyType { get; set; }

        [Required]
        public DateTime Year { get; set; }

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
        public string EngineModification { get; set; }

        [Required]
        public DoorsType Doors { get; set; }

        [Required]
        public ColorType Color { get; set; }

        public bool Metallic { get; set; }

        public EuroStandartType EuroStandart { get; set; }

        public string AddedBy { get; set; }

        public bool IsActive { get; set; }

        public DateTime AddedOn { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Pictures> Pictures { get; set; }

        public ICollection<FirstExtras> FirstExtras { get; set; }

        public ICollection<SecondExtras> SecondExtras { get; set; }

        public ICollection<ThirdExtras> ThirdExtras { get; set; }
    }
}
