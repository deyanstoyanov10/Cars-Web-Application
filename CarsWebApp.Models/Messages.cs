namespace CarsWebApp.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Messages
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Message { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [DefaultValue(true)]
        public bool IsNew { get; set; } = true;
    }
}
