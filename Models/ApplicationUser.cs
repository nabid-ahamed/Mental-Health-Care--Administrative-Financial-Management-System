using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHC_AFMS.Models
{
    // Must inherit from IdentityUser to work with the AddEntityFrameworkStores error
    public class ApplicationUser : IdentityUser
    {
        // Id, Email, PhoneNumber, etc. are already included in IdentityUser.

        public string FullName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = "";
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string Address { get; set; } = "";
        public string Specialization { get; set; } = "";

        // 🆕 NEW: Fee for this doctor (e.g., 1500.00)
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsultationFee { get; set; }
    }
}