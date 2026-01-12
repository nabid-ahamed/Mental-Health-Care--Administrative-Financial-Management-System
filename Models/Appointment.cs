using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHC_AFMS.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        // --- LINKS TO USERS ---
        [Required]
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public ApplicationUser Doctor { get; set; }

        [Required]
        public string PatientId { get; set; }
        [ForeignKey("PatientId")]
        public ApplicationUser Patient { get; set; }

        // --- TIMING ---
        [Required]
        public DateTime AppointmentDate { get; set; } // e.g., 2025-10-15

        [Required]
        public TimeSpan StartTime { get; set; } // e.g., 09:15

        [Required]
        public TimeSpan EndTime { get; set; }   // e.g., 09:30

        // --- STATUS & CLINICAL ---
        public string Status { get; set; } = "Pending"; // Pending, Approved, Completed, Cancelled
        public string Description { get; set; } = "";   // Reason for visit
        public string Diagnosis { get; set; } = "";     // Clinical note
        public string Prescription { get; set; } = "";  // Medicine

        // ❌ REMOVED: public string? RecommendedTests { get; set; } (Replaced by list below)

        public DateTime? FollowUpDate { get; set; }     // New

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- FINANCIAL ---
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsultationFee { get; set; }

        // 🆕 NEW: Total Invoice Amount (Fee + Tests - Discount)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // 🆕 NEW: Prevents duplicate invoices
        public bool IsInvoiceGenerated { get; set; } = false;

        // --- RELATIONSHIPS ---
        // 🆕 NEW: Link to the table that tracks suggested/accepted tests
        public List<AppointmentTest> AppointmentTests { get; set; } = new List<AppointmentTest>();
    }
}