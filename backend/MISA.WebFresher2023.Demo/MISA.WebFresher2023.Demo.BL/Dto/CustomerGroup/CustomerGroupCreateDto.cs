using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class CustomerGroupCreateDto
    {
        public Guid CustomerGroupId { get; set; }
        public string CustomerGroupCode { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
