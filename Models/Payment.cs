public class Payment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }

}
