using MISA.WebFresher2023.Demo.Common.Attribute;
using MISA.WebFresher2023.Demo.Common.Constant;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }

        public bool IsParent { get; set; }

        public int Rank { get; set; }
        public string MisaCode { get; set; }

        [MSMaxLength(Length = 20, ErrorCode = (int)AccountErrorCode.CodeTooLong)]
        [MSRequired(ErrorCode = (int)AccountErrorCode.CodeIsRequired)]
        public string AccountCode { get; set; }

        [MSRequired(ErrorCode = (int)AccountErrorCode.NameIsRequired)]
        [MSMaxLength(Length = 100, ErrorCode = (int)AccountErrorCode.NameViTooLong)]
        public string NameVi { get; set; }

        [MSMaxLength(Length = 100, ErrorCode = (int)AccountErrorCode.NameEnTooLong)]
        public string NameEn { get; set; }

        public Guid AccountSyntheticId { get; set; }

        [MSRequired(ErrorCode = (int)AccountErrorCode.AccountPropertyIsRequired)]
        public Guid AccountPropertyId { get; set; }

        public string AccountPropertyName { get; set; }

        [MSMaxLength(Length = 255, ErrorCode = (int)AccountErrorCode.NoteTooLong)]
        public string Note { get; set; }

        public bool HasForeignCurrencyAccounting { get; set; }

        public int ObjectType { get; set; }

        public int CostAggregationObject { get; set; }

        public int TheOrder { get; set; }

        public int PurchaseContract { get; set; }

        public int Unit { get; set; }

        public bool BankAccount { get; set; }

        public int Construction { get; set; }

        public int ContractOfSale { get; set; }

        public int ExpenseItem { get; set; }

        public int StatisticalCode { get; set; }
    }
}
