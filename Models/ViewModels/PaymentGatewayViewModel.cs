namespace MHC_AFMS.Models.ViewModels
{
    public class PaymentGatewayViewModel
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }

        // "Card", "Bkash", "Nagad", "Rocket"
        public string PaymentMethod { get; set; }

        // Card Details
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryDate { get; set; }
        public string? Cvv { get; set; }

        // MFS Details
        public string? MobileNumber { get; set; }
        public string? Pin { get; set; }
    }
}