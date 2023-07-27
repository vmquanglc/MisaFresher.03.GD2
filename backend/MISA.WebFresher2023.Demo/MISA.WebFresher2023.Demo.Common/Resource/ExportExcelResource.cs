using MISA.WebFresher2023.Demo.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.Common.Resource
{
    /// <summary>
    /// Class tài nguyên của xuất excel
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public static class ExportExcelResource
    {
        public static string SheetName(string table)
        {
            return table switch
            {
                "Employee" => "Nhân viên",
                "Customer" => "Khách hàng",
                "Account" => "Tài khoản",
                "Receipt" => "Phiếu thu",
                _ => table
            };
        }

        public static string Header(string Field)
        {
            return Field switch
            {
                // Employee
                "FullName" => "Họ tên",
                "EmployeeCode" => "Mã nhân viên",
                "Gender" => "Giới tính",
                "DateOfBirth" => "Ngày sinh",
                "IdentityNumber" => "Số chứng minh thư",
                "PositionName" => "Chức danh",
                "DepartmentName" => "Tên đơn vị",
                "BankAccountNumber" => "Số tài khoản",
                "NameOfBank" => "Tên ngân hàng",
                "BankAccountBranch" => "Chi nhánh ngân hàng",


                // Customer
                "CustomerType" => "Loại khách hàng",
                "IsSupplier" => "Là nhà cung cấp",
                "TaxCode" => "Mã số thuế",
                "CustomerCode" => "Mã khách hàng",
                "Address" => "Địa chỉ",
                "PhoneNumber" => "Số điện thoại",
                "MaxDaysOwed" => "Số ngày được nợ",
                "MaxAmountOfDebt" => "Số nợ tối đa",
                "Note" => "Ghi chú",
                "IdentityDate" => "Ngày cấp",
                "IdentityPlace" => "Nơi cấp",
                "CreatedDate" => "Ngày tạo",
                "CreatedBy" => "Người tạo",
                "ModifiedDate" => "Ngày sửa",
                "ModifiedBy" => "Người sửa",

                // Account
                "AccountCode" => "Số tài khoản",
                "NameVi" => "Tên tài khoản",
                "NameEn" => "Tên tiếng anh",
                "AccountPropertyName" => "Tính chất",
                "HasForeignCurrencyAccounting" => "Có hạch toán ngoại tệ",
                "ObjectType" => "Loại đối tượng",
                "CostAggregationObject" => "Đối tượng tập hợp chi phí",
                "TheOrder" => "Đơn đặt hàng",
                "PurchaseContract" => "Hợp đồng mua",
                "Unit" => "Đơn vị",
                "BankAccount" => "Tài khoản ngân hàng",
                "Construction" => "Công trình",
                "ContractOfSale" => "Hợp đồng bán",
                "ExpenseItem" => "Khoản mục chi phí",
                "StatisticalCode" => "Mã thống kê",

                // Receipt
                "ReceiptCode" => "Số phiếu thu",
                "ReceiptType" => "Loại phiếu thu",
                "CustomerName" => "Tên khách hàng",
                "Payer" => "Người nộp",
                "Reason" => "Lý do nộp",
                "Amount" => "Số lượng",
                "AccountingDate" => "Ngày hạch toán",
                "ReceiptDate" => "Ngày phiếu thu",
                "AmountOfMoney" => "Tiền",

                _ => Field,
            };
        }

        public static string FileName(string table)
        {
            return table switch
            {
                "Employee" => "Danh_sach_nhan_vien.xlsx",
                "Customer" => "Danh_sach_khach_hang.xlsx",
                "Receipt" => "Danh_sach_phieu_thu.xlsx",
                "Account" => "Danh_sach_tai_khoan.xlsx",
                _ => "Danh_sach.xlsx",
            };
        }
        /// <summary>
        /// Tiêu đề trang tính 
        /// </summary>
        /// Author: LeDucTiep (07/06/2023)
        public static string SheetTitle(string table)
        {
            return table switch
            {
                "Employee" => "DANH SÁCH NHÂN VIÊN",
                "Customer" => "DANH SÁCH KHÁCH HÀNG",
                "Account" => "DANH SÁCH TÀI KHOẢN",
                "Receipt" => "DANH SÁCH PHIẾU THU",
                _ => "DANH SÁCH",
            };
        }

        /// <summary>
        /// Số thứ tự 
        /// </summary>
        /// Author: LeDucTiep (07/06/2023)
        public static string NumericalOrder = "STT";

        // Giới tính nam
        public static readonly string Male = "Nam";

        // Giới tính nữ
        public static readonly string Female = "Nữ";
    }
}
