using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class BankAccountDto
    {
        public Guid BankAccountId { get; set; }
        public Guid CustomerId { get; set; }
        public string BankAccountNumber { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string BankCity { get; set; }
    }
}
