using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class OtherLocationUpdateDto
    {
        public Guid OtherLocationId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid NationId { get; set; }
        public Guid CityId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid CommuneId { get; set; }
        public bool IsSameCustomerAddress { get; set; }
    }
}
