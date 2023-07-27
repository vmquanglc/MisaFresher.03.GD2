﻿using MISA.WebFresher2023.Demo.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.BL.Dto
{
    public class ReceiptDto
    {
        public Guid? ReceiptId { get; set; }

        public bool IsRecorded { get; set; }
        public string? ReceiptCode { get; set; }

        public int? ReceiptType { get; set; }

        public Guid? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? Payer { get; set; }

        public string? Address { get; set; }

        public Guid? EmployeeId { get; set; }

        public string? Reason { get; set; }

        public int? Amount { get; set; }

        public DateTime? AccountingDate { get; set; }

        public DateTime? ReceiptDate { get; set; }

        public double? TotalMoney { get; set; }

        public string? CustomerCode { get; set; }

        public List<BookKeeping>? BookKeepings { get; set; }
    }
}
