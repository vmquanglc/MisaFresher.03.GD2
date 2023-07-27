using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class BankAccount : BaseEntity
    {
        public Guid BankAccountId { get; set; }
        public Guid CustomerId { get; set; }
        public string BankAccountNumber { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string BankCity { get; set; }
    }
}
