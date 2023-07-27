// Code này không dùng nữa

// import * as XLSX from "xlsx-js-style";

// const MSExcel = {
//   /**
//    * Hàm xuất excel
//    * Author: LeDucTiep (01/06/2023)
//    */
//   async save(data) {
//     const aoaData = [
//       ["DANH SÁCH NHÂN VIÊN"],
//       [],
//       [
//         "STT",
//         "Mã nhân viên",
//         "Tên nhân viên",
//         "Giới tính",
//         "Ngày sinh",
//         "Chức danh",
//         "Tên đơn vị",
//         "Số tài khoản",
//         "Tên ngân hàng",
//       ],
//     ];

//     const workbook = XLSX.utils.book_new();

//     const worksheet = XLSX.utils.aoa_to_sheet(aoaData);

//     const merge = [
//       { s: { r: 0, c: 0 }, e: { r: 0, c: 8 } },
//       { s: { r: 1, c: 0 }, e: { r: 1, c: 8 } },
//     ];

//     worksheet["!merges"] = merge;

//     const _rowIndex = Math.max(
//       (Math.floor(data.length) + "").length,
//       aoaData[2][0].length
//     );
//     const _employeeCode = data.reduce(
//       (w, r) => Math.max(w, r.EmployeeCode ? r.EmployeeCode.length : 10),
//       aoaData[2][1].length
//     );
//     const _fullName = data.reduce(
//       (w, r) => Math.max(w, r.FullName ? r.FullName.length : 10),
//       aoaData[2][2].length
//     );
//     const _gender = data.reduce(
//       (w, r) => Math.max(w, r.Gender ? r.Gender.length : 10),
//       aoaData[2][3].length
//     );
//     const _dateOfBirth = data.reduce(
//       (w, r) => Math.max(w, r.DateOfBirth ? r.DateOfBirth.length : 10),
//       aoaData[2][4].length
//     );
//     const _positionName = data.reduce(
//       (w, r) => Math.max(w, r.PositionName ? r.PositionName.length : 10),
//       aoaData[2][5].length
//     );
//     const _departmentName = data.reduce(
//       (w, r) => Math.max(w, r.DepartmentName ? r.DepartmentName.length : 10),
//       aoaData[2][6].length
//     );
//     const _bankAccountNumber = data.reduce(
//       (w, r) =>
//         Math.max(w, r.BankAccountNumber ? r.BankAccountNumber.length : 10),
//       aoaData[2][7].length
//     );
//     const _nameOfBank = data.reduce(
//       (w, r) => Math.max(w, r.NameOfBank ? r.NameOfBank.length : 10),
//       aoaData[2][8].length
//     );

//     worksheet["!cols"] = [
//       { wch: _rowIndex > 25 ? 25 : _rowIndex * 1.5 },
//       { wch: _employeeCode > 25 ? 25 : _employeeCode * 1.5 },
//       { wch: _fullName > 25 ? 25 : _fullName * 1.5 },
//       { wch: _gender > 25 ? 25 : _gender * 1.5 },
//       { wch: _dateOfBirth > 25 ? 25 : _dateOfBirth * 1.5 },
//       { wch: _positionName > 25 ? 25 : _positionName * 1.5 },
//       { wch: _departmentName > 25 ? 25 : _departmentName * 1.5 },
//       { wch: _bankAccountNumber > 15 ? 15 : _bankAccountNumber * 1.5 },
//       { wch: _nameOfBank > 25 ? 25 : _nameOfBank * 1.5 },
//     ];

//     worksheet["!rows"] = [{ hpt: 20.5 }, { hpt: 22 }];

//     let rowIndex = 4;

//     data.forEach((employee) => {
//       XLSX.utils.sheet_add_aoa(
//         worksheet,
//         [
//           [
//             rowIndex - 3,
//             employee.EmployeeCode ? employee.EmployeeCode : "",
//             employee.FullName ? employee.FullName : "",
//             employee.Gender ? employee.Gender : "",
//             employee.DateOfBirth ? employee.DateOfBirth : "",
//             employee.PositionName ? employee.PositionName : "",
//             employee.DepartmentName ? employee.DepartmentName : "",
//             employee.BankAccountNumber ? employee.BankAccountNumber : "",
//             employee.NameOfBank ? employee.NameOfBank : "",
//           ],
//         ],
//         {
//           origin: `A${rowIndex}`,
//         }
//       );
//       rowIndex++;
//     });

//     // style dong tieu de
//     let cell = XLSX.utils.decode_cell("A1");

//     worksheet[XLSX.utils.encode_cell(cell)].s = {
//       font: {
//         name: "Arial",
//         bold: true,
//         sz: 16,
//       },
//       alignment: {
//         vertical: "center",
//         horizontal: "center",
//         wrapText: "1",
//       },
//     };

//     // A3:I3
//     const row = 2;
//     for (let col = 0; col <= 8; col++) {
//       try {
//         const cellPosition = { c: col, r: row };
//         cell = XLSX.utils.encode_cell(cellPosition);
//         worksheet[cell].s = {
//           font: {
//             name: "Arial",
//             bold: true,
//             sz: 10,
//           },
//           alignment: {
//             vertical: "center",
//             horizontal: "center",
//             wrapText: "1",
//           },
//           fill: {
//             // background color
//             patternType: "solid",
//             fgColor: { rgb: "D8D8D8" },
//             bgColor: { rgb: "000000" },
//             wrapText: "1",
//           },
//           border: {
//             top: {
//               style: "thin",
//               color: "000000",
//             },
//             bottom: {
//               style: "thin",
//               color: "000000",
//             },
//             right: {
//               style: "thin",
//               color: "000000",
//             },
//             left: {
//               style: "thin",
//               color: "000000",
//             },
//           },
//         };
//       } catch (e) {
//         console.log(e);
//       }
//     }

//     // A4:I21
//     for (let col = 0; col <= 8; col++) {
//       for (let row = 3; row <= data.length + 2; row++) {
//         try {
//           const cellPosition = { c: col, r: row };
//           cell = XLSX.utils.encode_cell(cellPosition);
//           worksheet[cell].s = {
//             font: {
//               name: "Times New Roman",
//               bold: false,
//               sz: 11,
//             },
//             alignment: {
//               vertical: "bottom",
//               horizontal: "left",
//               wrapText: "1",
//             },
//             border: {
//               top: {
//                 style: "thin",
//                 color: "000000",
//               },
//               bottom: {
//                 style: "thin",
//                 color: "000000",
//               },
//               right: {
//                 style: "thin",
//                 color: "000000",
//               },
//               left: {
//                 style: "thin",
//                 color: "000000",
//               },
//             },
//           };
//         } catch (e) {
//           console.log(e);
//         }
//       }
//     }

//     XLSX.utils.book_append_sheet(workbook, worksheet, "DANH SÁCH NHÂN VIÊN");

//     await XLSX.writeFile(workbook, "Danh_sach_nhan_vien.xlsx");
//   },
// };

// export default MSExcel;
