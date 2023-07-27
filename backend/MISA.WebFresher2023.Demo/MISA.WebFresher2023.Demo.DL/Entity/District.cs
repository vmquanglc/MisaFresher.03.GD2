using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class District : BaseEntity
    {
        public Guid DistrictId { get; set; }
        public string Name { get; set; }
        public Guid CityId { get; set; }
    }
}
