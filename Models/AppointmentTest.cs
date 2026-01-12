using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHC_AFMS.Models
{
    public class AppointmentTest
    {
        [Key]
        public int Id { get; set; }

        // Link to the Appointment
        public int AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }

        // Link to the specific Medical Test (X-Ray, MRI, etc.)
        public int MedicalTestId { get; set; }
        [ForeignKey("MedicalTestId")]
        public MedicalTest MedicalTest { get; set; }

        // Status: "Suggested", "Accepted", "Rejected", "Completed"
        public string Status { get; set; } = "Suggested";
    }
}