using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHC_AFMS.Models
{
    public class DoctorSchedule
    {
        [Key]
        public int Id { get; set; }

        // Link this schedule to a specific Doctor
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public ApplicationUser Doctor { get; set; }

        // The Day they work (e.g., "Monday", "Tuesday")
        public string DayOfWeek { get; set; }

        // Start Time (We store ONLY the hour, e.g., 9 for 9:00 AM)
        [Range(9, 21)] // Valid values: 9 to 21 (9 PM)
        public int StartHour { get; set; }

        // End Time (We store ONLY the hour, e.g., 17 for 5:00 PM)
        [Range(10, 22)] // Valid values: 10 to 22 (10 PM)
        public int EndHour { get; set; }
    }
}