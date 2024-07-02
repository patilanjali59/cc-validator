namespace PaymentValidationAPI.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string CreditCardType { get; set; }
        public string[] Errors { get; set; }
    }
}
