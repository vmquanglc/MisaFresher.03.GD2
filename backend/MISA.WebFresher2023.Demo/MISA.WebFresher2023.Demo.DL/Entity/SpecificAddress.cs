using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class SpecificAddress : BaseEntity
    {
        public Guid? SpecificAddressId { get; set; }
        public Guid? OtherLocationId { get; set; }
        public string? Address { get; set; }
    }
}
