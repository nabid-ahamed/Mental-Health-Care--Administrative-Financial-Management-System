using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHC_AFMS.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        // --- LINK TO APPOINTMENT ---
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; } // 🟢 ADDED THIS (Fixes the error)

        // --- LINK TO PATIENT ---
        public string PatientId { get; set; }

        [ForeignKey("PatientId")]
        public ApplicationUser Patient { get; set; } // 🟢 ADDED THIS (Good practice)

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string Status { get; set; } = "Unpaid"; // Unpaid, Paid

        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);
    }
}