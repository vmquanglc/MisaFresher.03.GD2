using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class CustomerGroup : BaseEntity
    {
        public Guid CustomerGroupId { get; set; }

        public string CustomerGroupCode { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
