using System.ComponentModel.DataAnnotations;

namespace PlacementManagementSystem.Models
{
    public class PlacementDrive
    {
        [Key]
        public int PlacementDriveId { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Drive Name")]
        public string DriveName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Drive Date")]
        public DateTime DriveDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Venue")]
        public string? Venue { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<JobOpening> JobOpenings { get; set; }
            = new List<JobOpening>();
    }
}