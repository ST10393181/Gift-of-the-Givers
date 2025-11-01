using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public DateTime? LastLogin { get; set; }

        // Relationships
        public ICollection<Resource> Resources { get; set; }
        public ICollection<Volunteer> Volunteers { get; set; }

    }
}
