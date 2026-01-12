using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHC_AFMS.Models
{
    public class MedicalTest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Test Name")]
        public string TestName { get; set; } // e.g., "Blood Test", "MRI"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cost")]
        public decimal Price { get; set; } // e.g., 500.00

        // Soft Delete: If a test is no longer offered, we hide it instead of deleting history
        public bool IsActive { get; set; } = true;
    }
}