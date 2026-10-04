using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacementManagementSystem.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        // Identity user connected to this company account
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        // Company information
        [Required]
        [StringLength(150)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Official Email")]
        public string OfficialEmail { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Industry { get; set; } = string.Empty;

        [Url]
        [StringLength(300)]
        public string? Website { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // TPO approval
        public bool IsApproved { get; set; } = false;

        public DateTime? ApprovedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}