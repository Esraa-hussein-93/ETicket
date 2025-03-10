using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Tickets.Models
{
    public class Cinema
    {

        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? CinemaLogo { get; set; }

        public string? Address { get; set; }
        [ValidateNever]
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

    }
}
