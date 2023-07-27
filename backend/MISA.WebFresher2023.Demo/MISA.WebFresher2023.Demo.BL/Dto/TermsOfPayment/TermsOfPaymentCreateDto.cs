namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class TermsOfPaymentCreateDto
    {
        public Guid TermsOfPaymentId { get; set; }
        public string? TermsOfPaymentCode { get; set; }
        public string? Name { get; set; }
        public int? MaxDaysOwed { get; set; }
        public int? DiscountPeriod { get; set; }
        public int? DiscountRate { get; set; }

    }
}
