using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Tickets.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; } 

        public string? Bio { get; set; }

        [RegularExpression("^.*\\.(png|jpg)$")]

        public string? ProfilePicture { get; set; }

        public string? News { get; set; }
        [ValidateNever]
        public ICollection<ActorMovie> Movies { get; set; } = new List<ActorMovie>();
    }
}
