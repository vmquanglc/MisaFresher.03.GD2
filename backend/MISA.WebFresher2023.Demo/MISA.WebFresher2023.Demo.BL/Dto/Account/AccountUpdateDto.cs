using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class AccountUpdateDto
    {
        public string AccountCode { get; set; }

        public int Rank { get; set; }

        public string MisaCode { get; set; }

        public string? NameVi { get; set; }

        public string? NameEn { get; set; }

        public Guid? AccountSyntheticId { get; set; }

        public Guid? AccountPropertyId { get; set; }

        public string? Note { get; set; }

        public bool? HasForeignCurrencyAccounting { get; set; }

        public int? ObjectType { get; set; }

        public int? CostAggregationObject { get; set; }

        public int? TheOrder { get; set; }

        public int? PurchaseContract { get; set; }

        public int? Unit { get; set; }

        public bool? BankAccount { get; set; }

        public int? Construction { get; set; }

        public int? ContractOfSale { get; set; }

        public int? ExpenseItem { get; set; }

        public int? StatisticalCode { get; set; }
    }
}
