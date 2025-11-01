using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class ReliefProject
    {
        [Key]
        public int ReliefProjectID { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; }

        // Foreign Key
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

    }
}
