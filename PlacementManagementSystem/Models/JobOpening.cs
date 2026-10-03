using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacementManagementSystem.Models
{
    public class JobOpening
    {
        [Key]
        public int JobOpeningId { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [StringLength(100)]
        public string? JobLocation { get; set; }

        [StringLength(100)]
        public string? EmploymentType { get; set; }

        [Range(0, 100000000)]
        [Display(Name = "Salary / Package")]
        public decimal? SalaryPackage { get; set; }

        [Range(0, 10)]
        [Display(Name = "Minimum CGPA")]
        public decimal MinimumCGPA { get; set; } = 0;

        [Range(0, 100)]
        [Display(Name = "Maximum Backlogs")]
        public int MaximumBacklogs { get; set; } = 0;

        [StringLength(500)]
        public string? EligibleDepartments { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Application Deadline")]
        public DateTime? ApplicationDeadline { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Company
        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }

        // Placement Drive
        public int? PlacementDriveId { get; set; }

        [ForeignKey(nameof(PlacementDriveId))]
        public PlacementDrive? PlacementDrive { get; set; }

        public ICollection<PlacementApplication> Applications { get; set; }
            = new List<PlacementApplication>();
    }
}