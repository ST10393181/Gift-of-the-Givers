using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class Volunteer
    {
        [Key]
        public int Volunteer_ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Location { get; set; }

        public string AvailabilityStatus { get; set; }

        // Foreign Key
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        // Navigation
        public ICollection<Assignment> Assignments { get; set; }

    }
}
