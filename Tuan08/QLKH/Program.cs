using System;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Ui.Start("QLKH · Quản lý kho điện thoại",
                new Entry("Quản lý hãng sản xuất", store => new CatalogForm<Hang>(store, options: new CatalogOptions<Hang>
                {
                    Search = new[] { "mahang", "tenhang" }
                })),
                new Entry("Quản lý điện thoại", store => new CatalogForm<DienThoai>(store, options: new CatalogOptions<DienThoai>
                {
                    Search = new[] { "ma", "ten" }, Filter = "mahang"
                })));
        }
    }
}
