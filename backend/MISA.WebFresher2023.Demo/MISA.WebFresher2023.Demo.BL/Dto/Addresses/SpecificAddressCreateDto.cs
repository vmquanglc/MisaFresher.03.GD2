using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class SpecificAddressCreateDto
    {
        public Guid SpecificAddressId { get; set; }
        public string OtherLocationId { get; set; }
        public string Address { get; set; }
    }
}
