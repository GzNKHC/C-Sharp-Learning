using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");
            Ui.Start("Thực hành 07b – Nhập liệu qua biểu mẫu",
                new[] { "Danh sách sinh viên", "Phiếu nhập hàng", "Mẫu · Quản lý khoa" },
                () => new StudentForm(), () => new ReceiptForm(), () => new DepartmentForm());
        }
    }
}
