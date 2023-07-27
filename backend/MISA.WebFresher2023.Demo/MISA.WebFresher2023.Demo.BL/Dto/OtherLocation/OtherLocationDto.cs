using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class OtherLocationDto
    {
        public Guid? OtherLocationId { get; set; }
        public SpecificAddress[]? SpecificAddresses { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? NationId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? CommuneId { get; set; }
        public bool? IsSameCustomerAddress { get; set; }
    }
}
