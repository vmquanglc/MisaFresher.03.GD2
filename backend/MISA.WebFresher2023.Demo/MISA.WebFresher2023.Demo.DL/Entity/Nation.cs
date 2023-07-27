using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class Nation : BaseEntity
    {
        public Guid NationId { get; set; }
        public string Name { get; set; }
    }
}
