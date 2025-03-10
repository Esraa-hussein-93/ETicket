using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Tickets.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } 

        public string? Description { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        [RegularExpression("^.*\\.(png|jpg)$")]

        public string? ImgUrl { get; set; }
        [RegularExpression("^.*\\.(mp4|avi|mov|mkv|flv|wmv)$", ErrorMessage = "Only video files are allowed.")]

        public string? TrailerUrl { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MovieStatus MovieStatus { get; set; }

        public int CinemaId { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]

        public Category Category { get; set; }
        [ValidateNever]

        public Cinema Cinema { get; set; }

        [ValidateNever]

        public ICollection<ActorMovie> Actors { get; set; } = new List<ActorMovie>();
    }
}
