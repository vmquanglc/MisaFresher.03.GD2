using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class CustomerAndGroup : BaseEntity
    {
        public Guid CustomerGroupId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
