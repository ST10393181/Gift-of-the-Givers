using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class Donation
    {
        [Key]
        public int Donation_ID { get; set; }

        public string DonorName { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime DonationDate { get; set; }

        // Foreign Keys
        public int Resource_ID { get; set; }
        public int Disaster_ID { get; set; }

        [ForeignKey("Resource_ID")]
        public Resource Resource { get; set; }

        [ForeignKey("Disaster_ID")]
        public Disaster Disaster { get; set; }

    }
}
