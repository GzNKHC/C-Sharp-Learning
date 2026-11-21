using System;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Ui.Start("QLBT · Quản lý bán thuốc",
                new Entry("a. Quản lý đơn hàng", store => new CatalogForm<DonHang>(store, "Quản lý đơn hàng", OrderForms.Options(true))),
                new Entry("b. Quản lý thuốc", store => new CatalogForm<MatHang>(store, "Quản lý thuốc",
                    new CatalogOptions<MatHang> { Navigate = true, Validate = (s, row, old) =>
                    {
                        if (row.NgayHetHanDung <= row.NgaySanXuat)
                            throw new InvalidOperationException("Ngày hết hạn dùng phải sau ngày sản xuất.");
                    } })),
                new Entry("c. Quản lý bán thuốc", store => OrderForms.Details(store, "Nhập chi tiết đơn hàng thuốc")),
                new Entry("d. Tra cứu thuốc", store => new SearchForm<MatHang>(store, "Tra cứu thuốc", true, false, "CongDung", "TenThuoc")),
                new Entry("e. Lập đơn hàng cho khách", OrderForms.CustomerOrders));
        }
    }
}
