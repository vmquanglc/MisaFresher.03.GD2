using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Entity
{
    public class Customer : BaseEntity
    {
        public Guid? CustomerId { get; set; }

        public int? CustomerType { get; set; }

        public bool? IsSupplier { get; set; }

        public string? TaxCode { get; set; }

        public string? CustomerCode { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Website { get; set; }

        public Guid? EmployeeId { get; set; }

        public Guid? ContactInforId { get; set; }

        public Guid? TermsOfPaymentId { get; set; }

        public int? MaxDaysOwed { get; set; }

        public double? MaxAmountOfDebt { get; set; }

        public Guid? AccountCollectId { get; set; }

        public Guid? AccountPayId { get; set; }

        public string? Note { get; set; }

        public string? IdentityNumber { get; set; }

        public DateTime? IdentityDate { get; set; }

        public string? IdentityPlace { get; set; }

    }
}
