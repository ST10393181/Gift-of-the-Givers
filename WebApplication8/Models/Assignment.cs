using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class Assignment
    {
        [Key]
        public int Assignment_ID { get; set; }

        // Foreign Keys
        public int Volunteer_ID { get; set; }
        public int Disaster_ID { get; set; }

        [ForeignKey("Volunteer_ID")]
        public Volunteer Volunteer { get; set; }

        [ForeignKey("Disaster_ID")]
        public Disaster Disaster { get; set; }

        public string Role { get; set; }
        public DateTime AssignedDate { get; set; }
        public string Status { get; set; }

    }
}
