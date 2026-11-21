using System;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Ui.Start("QLBH · Quản lý bán hàng",
                new Entry("a. Quản lý mặt hàng", store => new CatalogForm<MatHang>(store, "Quản lý mặt hàng",
                    new CatalogOptions<MatHang> { Navigate = true, Edit = false, Delete = false })),
                new Entry("b. Tìm kiếm mặt hàng", store => new SearchForm<MatHang>(store, "Tìm kiếm mặt hàng",
                    true, true, "MaMatHang", "TenMatHang")),
                new Entry("c. Quản lý đại lý", store => new CatalogForm<DoiTac>(store, "Quản lý đại lý",
                    new CatalogOptions<DoiTac> { Delete = false })),
                new Entry("d. Quản lý đơn hàng", store => new CatalogForm<DonHang>(store, "Lập đơn hàng", OrderForms.Options(false))),
                new Entry("e. Nhập chi tiết đơn hàng", store => OrderForms.Details(store, "Nhập chi tiết đơn hàng")),
                new Entry("f. Hủy đơn hàng theo đại lý", store => OrderForms.ForPartner(store, "Hủy đơn hàng theo đại lý", true)),
                new Entry("g. Lập đơn hàng theo đại lý", store => OrderForms.ForPartner(store, "Lập đơn hàng theo đại lý", false)),
                new Entry("h. Lập chi tiết đơn hàng", store => OrderForms.Details(store, "Lập chi tiết đơn hàng")));
        }
    }
}
