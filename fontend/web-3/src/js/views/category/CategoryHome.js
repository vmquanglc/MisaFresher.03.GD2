const mixin = {
  name: "CategoryHome",
  data() {
    return {
      categorys: [
        {
          title: "Đối tượng",
          items: [
            { content: "Khách hàng", route: "/customer" },
            { content: "Nhà cung cấp" },
            { content: "Nhân viên", route: "/employee" },
            { content: "Nhóm khách hàng, nhà cung cấp" },
          ],
        },
        {
          title: "Vật tư hàng hóa",
          items: [
            { content: "Vật tư hàng hóa" },
            { content: "Kho" },
            { content: "Nhóm vật tư, hàng hóa, dịch vụ" },
            { content: "Đơn vị tính" },
          ],
        },
        {
          title: "Tài khoản",
          items: [
            { content: "Hệ thống tài khoản", route: "/account" },
            { content: "Tài khoản kết chuyển" },
            { content: "Tài khoản ngầm định" },
          ],
        },

        {
          title: "Chi phí",
          items: [
            { content: "Đối tượng tập hợp chi phí" },
            { content: "Khoản mục chi phí" },
            { content: "Công trình" },
            { content: "Loại công trình" },
          ],
        },
        {
          title: "Ngân hàng",
          items: [{ content: "Ngân hàng" }, { content: "Tài khoản ngân hàng" }],
        },

        {
          title: "Chi nhánh, phòng ban",
          items: [{ content: "Cơ cấu tổ chức" }],
        },
        {
          title: "Tài sản",
          items: [
            { content: "Loại công cụ dụng cụ" },
            { content: "Loại tài sản cố định" },
          ],
        },

        {
          title: "Thuế",
          items: [
            { content: "Biểu thuế tiêu thụ đặc biệt" },
            { content: "Biểu thuế tài nguyên" },
          ],
        },
        {
          title: "Tiền lương",
          items: [
            { content: "Ký hiệu chấm công" },
            { content: "Biểu tính thuế thu nhập" },
          ],
        },
        {
          title: "Khác",
          items: [
            { content: "Điều khoản thanh toán" },
            { content: "Mục thu/chi" },
            { content: "Mã thống kê" },
          ],
        },
      ],
    };
  },
};

export default mixin;
