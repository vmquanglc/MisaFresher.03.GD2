using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class Commune : BaseEntity
    {
        public Guid CommuneId { get; set; }
        public string Name { get; set; }
        public Guid DistrictId { get; set; }
    }
}
