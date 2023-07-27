﻿using MISA.WebFresher2023.Demo.Common.Attribute;
using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }    
        public List<Guid> CustomerGroupIds { get; set; }

        public List<BankAccount> BankAccounts { get; set; }

        public OtherLocationDto OtherLocation { get; set; }

        public int CustomerType { get; set; }

        public bool? IsSupplier { get; set; }

        [MSMaxLength(Length = 50, ErrorCode = (int)CustomerErrorCode.TaxCodeTooLong)]
        public string? TaxCode { get; set; }

        [MSRequired(ErrorCode = (int)CustomerErrorCode.CodeIsRequired)]
        [MSMaxLength(Length = 20, ErrorCode = (int)CustomerErrorCode.CustomerCodeTooLong)]
        public string? CustomerCode { get; set; }

        [MSRequired(ErrorCode = (int)CustomerErrorCode.FullNameIsRequired)]
        [MSMaxLength(Length = 100, ErrorCode = (int)CustomerErrorCode.FullNameTooLong)]
        public string? FullName { get; set; }

        [MSMaxLength(Length = 255, ErrorCode = (int)CustomerErrorCode.AddressTooLong)]
        public string? Address { get; set; }

        [MSMaxLength(Length = 50, ErrorCode = (int)CustomerErrorCode.PhoneNumberTooLong)]
        public string? PhoneNumber { get; set; }

        [MSMaxLength(Length = 255, ErrorCode = (int)CustomerErrorCode.WebsiteTooLong)]
        public string? Website { get; set; }

        public Guid? EmployeeId { get; set; }

        public ContactInforDto? ContactInfor { get; set; }

        public Guid? TermsOfPaymentId { get; set; }

        public int? MaxDaysOwed { get; set; }

        public double MaxAmountOfDebt { get; set; }

        public Guid? AccountCollectId { get; set; }

        public Guid? AccountPayId { get; set; }

        [MSMaxLength(Length = 255, ErrorCode = (int)CustomerErrorCode.NoteTooLong)]
        public string? Note { get; set; }

        [MSMaxLength(Length = 25, ErrorCode = (int)CustomerErrorCode.IdentityNumberTooLong)]
        public string? IdentityNumber { get; set; }

        public DateTime? IdentityDate { get; set; }

        [MSMaxLength(Length = 255, ErrorCode = (int)CustomerErrorCode.IdentityPlaceTooLong)]
        public string? IdentityPlace { get; set; }
    }
}
