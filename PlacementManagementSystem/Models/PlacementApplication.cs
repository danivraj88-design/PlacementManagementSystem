using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacementManagementSystem.Models
{
    public class PlacementApplication
    {
        [Key]
        public int PlacementApplicationId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }

        [Required]
        public int JobOpeningId { get; set; }

        [ForeignKey(nameof(JobOpeningId))]
        public JobOpening? JobOpening { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Applied";

        [StringLength(500)]
        public string? Remarks { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}