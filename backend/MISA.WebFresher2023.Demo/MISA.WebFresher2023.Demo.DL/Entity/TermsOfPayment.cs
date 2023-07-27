using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class TermsOfPayment : BaseEntity
    {
        public Guid TermsOfPaymentId { get; set; }
        public string TermsOfPaymentCode { get; set; }
        public string Name { get; set; }
        public int MaxDaysOwed { get; set; }
        public int DiscountPeriod { get; set; }
        public int DiscountRate { get; set; }

    }
}
