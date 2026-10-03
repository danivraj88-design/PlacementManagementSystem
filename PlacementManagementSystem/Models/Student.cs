using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlacementManagementSystem.Models
{
    public class Student
    {
        // Primary Key
        [Key]
        public int StudentId { get; set; }

        // Link to ASP.NET Identity User
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }


        // ==============================
        // PERSONAL INFORMATION
        // ==============================

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        [Display(Name = "Roll Number")]
        public string RollNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Phone]
        [StringLength(15)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }


        // ==============================
        // ADDRESS
        // ==============================

        [StringLength(500)]
        [Display(Name = "Present Address")]
        public string? PresentAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Permanent Address")]
        public string? PermanentAddress { get; set; }


        // ==============================
        // EMERGENCY CONTACT
        // ==============================

        [StringLength(100)]
        [Display(Name = "Emergency Contact Name")]
        public string? EmergencyContactName { get; set; }

        [Phone]
        [StringLength(15)]
        [Display(Name = "Emergency Contact Number")]
        public string? EmergencyContactNumber { get; set; }


        // ==============================
        // ACADEMIC INFORMATION
        // ==============================

        [Range(0, 100)]
        [Display(Name = "10th Percentage")]
        public decimal? TenthPercentage { get; set; }

        [Range(0, 100)]
        [Display(Name = "12th Percentage")]
        public decimal? TwelfthPercentage { get; set; }

        [Range(0, 100)]
        [Display(Name = "Diploma Percentage")]
        public decimal? DiplomaPercentage { get; set; }

        [Range(0, 10)]
        [Display(Name = "Current CGPA")]
        public decimal? CurrentCGPA { get; set; }

        [Range(0, 100)]
        [Display(Name = "Active Backlogs")]
        public int ActiveBacklogs { get; set; }

        [Range(0, 100)]
        [Display(Name = "Cleared Backlogs")]
        public int ClearedBacklogs { get; set; }

        [Range(2000, 2100)]
        [Display(Name = "Passing Year")]
        public int? PassingYear { get; set; }

        [StringLength(100)]
        public string? Specialization { get; set; }


        // ==============================
        // SKILLS & SOCIAL LINKS
        // ==============================

        [StringLength(1000)]
        public string? Skills { get; set; }

        [Url]
        [StringLength(300)]
        [Display(Name = "GitHub Profile")]
        public string? GitHubUrl { get; set; }

        [Url]
        [StringLength(300)]
        [Display(Name = "LinkedIn Profile")]
        public string? LinkedInUrl { get; set; }


        // ==============================
        // RESUME
        // ==============================

        [StringLength(255)]
        public string? ResumeFileName { get; set; }

        public DateTime? ResumeUploadedAt { get; set; }


        // ==============================
        // PROFILE STATUS
        // ==============================

        public bool IsProfileVerified { get; set; } = false;

        public bool IsProfileFrozen { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}