using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Models
{
    public class Resource
    {
        [Key]
        public int Resource_ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Type { get; set; }

        public int Quantity { get; set; }

        public string StorageLocation { get; set; }

        public DateTime LastUpdated { get; set; }

        // Foreign Key
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        // Relationships
        public ICollection<Donation> Donations { get; set; }

    }
}
