using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace ThucHanh8
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");
            Ui.Start("Thực hành 08 – Crystal Reports",
                new[]
                {
                    "Mẫu · Báo cáo tất cả sinh viên",
                    "Bài 1 · In danh sách sinh viên theo lớp",
                    "Bài 2 · Quản lý sinh viên và xem in",
                    "Bài 13 · Phiếu nhập hàng"
                },
                () => new StudentReportForm("Báo cáo tất cả sinh viên", false),
                () => new StudentReportForm("In danh sách sinh viên theo lớp", true),
                () => new StudentForm(), () => new ReceiptForm());
        }
    }
}
