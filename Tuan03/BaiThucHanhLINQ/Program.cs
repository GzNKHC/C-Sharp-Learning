using System.Text;

namespace BaiThucHanhLINQ;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n=== THỰC HÀNH LINQ ===");
            Console.WriteLine("1. Bài 2.1 - Truy vấn mảng số nguyên");
            Console.WriteLine("2. Bài 2.2 - Truy vấn mảng chuỗi");
            Console.WriteLine("3. Bài 3.1 - Thống kê mảng số nguyên");
            Console.WriteLine("4. Bài 3.2 - Thống kê mảng món ăn");
            Console.WriteLine("5. Bài 4.1 - Danh sách môn học");
            Console.WriteLine("6. Bài 5.1 - Truy vấn môn học");
            Console.WriteLine("7. Bài 5.2 - Thống kê môn học");
            Console.WriteLine("8. Bài 6.1 - Danh sách hệ đào tạo");
            Console.WriteLine("9. Bài 6.2 - Truy vấn hai nguồn dữ liệu");
            Console.WriteLine("A. Chạy tất cả bài");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn bài: ");

            string? luaChon = Console.ReadLine()?.Trim().ToUpperInvariant();
            if (luaChon == "0")
            {
                break;
            }

            switch (luaChon)
            {
                case "1": Bai2_1(); break;
                case "2": Bai2_2(); break;
                case "3": Bai3_1(); break;
                case "4": Bai3_2(); break;
                case "5": Bai4_1(); break;
                case "6": Bai5_1(); break;
                case "7": Bai5_2(); break;
                case "8": Bai6_1(); break;
                case "9": Bai6_2(); break;
                case "A":
                    Bai2_1();
                    Bai2_2();
                    Bai3_1();
                    Bai3_2();
                    Bai4_1();
                    Bai5_1();
                    Bai5_2();
                    Bai6_1();
                    Bai6_2();
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
        }
    }

    static void Bai2_1()
    {
        int[] mangSo = { 50, 42, 16, 3, 9, 8, 12, 7, 24, 0 };
        Console.WriteLine("\nBài 2.1a - Các số chia hết cho cả 4 và 3:");

        var queryA = from n in mangSo where n % 4 == 0 && n % 3 == 0 select n;
        Console.WriteLine("Query Syntax:");
        foreach (int n in queryA) Console.Write($"{n} ");

        var methodA = mangSo.Where(n => n % 4 == 0 && n % 3 == 0);
        Console.WriteLine("\nMethod Syntax:");
        foreach (int n in methodA) Console.Write($"{n} ");

        Console.WriteLine("\n\nBài 2.1b - Các phần tử nhỏ hơn hoặc bằng 3:");
        var queryB = from n in mangSo where n <= 3 select n;
        Console.WriteLine("Query Syntax:");
        foreach (int n in queryB) Console.Write($"{n} ");

        var methodB = mangSo.Where(n => n <= 3);
        Console.WriteLine("\nMethod Syntax:");
        foreach (int n in methodB) Console.Write($"{n} ");

        Console.WriteLine("\n\nBài 2.1c - Số chẵn chia đôi, số lẻ giữ nguyên:");
        var queryC = from n in mangSo select n % 2 == 0 ? n / 2 : n;
        Console.WriteLine("Query Syntax:");
        foreach (int n in queryC) Console.Write($"{n} ");

        var methodC = mangSo.Select(n => n % 2 == 0 ? n / 2 : n);
        Console.WriteLine("\nMethod Syntax:");
        foreach (int n in methodC) Console.Write($"{n} ");
        Console.WriteLine();
    }

    static void Bai2_2()
    {
        string[] mangChuoi = { "đầu", "lòng", "hai", "ả", "tố", "nga", "Thúy", "Kiều", "là", "chị", "em", "là", "Thúy", "Vân" };
        Console.WriteLine("\nBài 2.2a - Từ có 4 ký tự, sắp xếp theo ký tự đầu:");
        var queryA = from w in mangChuoi where w.Length == 4 orderby w[0] ascending select w;
        Console.WriteLine("Query Syntax:");
        foreach (string w in queryA) Console.WriteLine(w);
        var methodA = mangChuoi.Where(w => w.Length == 4).OrderBy(w => w[0]);
        Console.WriteLine("Method Syntax:");
        foreach (string w in methodA) Console.WriteLine(w);

        Console.WriteLine("\nBài 2.2b - Chuyển đổi chữ thường và chữ hoa:");
        var queryB = from w in mangChuoi select $"{w.ToLower()} - {w.ToUpper()}";
        Console.WriteLine("Query Syntax:");
        foreach (string w in queryB) Console.WriteLine(w);
        var methodB = mangChuoi.Select(w => $"{w.ToLower()} - {w.ToUpper()}");
        Console.WriteLine("Method Syntax:");
        foreach (string w in methodB) Console.WriteLine(w);

        Console.WriteLine("\nBài 2.2c - Từ chứa ký tự u:");
        var queryC = from w in mangChuoi where w.Contains("u") select w;
        Console.WriteLine("Query Syntax:");
        foreach (string w in queryC) Console.WriteLine(w);
        var methodC = mangChuoi.Where(w => w.Contains("u"));
        Console.WriteLine("Method Syntax:");
        foreach (string w in methodC) Console.WriteLine(w);

        Console.WriteLine("\nBài 2.2d - Từ bắt đầu bằng chữ in hoa:");
        var queryD = from w in mangChuoi where char.IsUpper(w[0]) select w;
        Console.WriteLine("Query Syntax:");
        foreach (string w in queryD) Console.WriteLine(w);
        var methodD = mangChuoi.Where(w => char.IsUpper(w[0]));
        Console.WriteLine("Method Syntax:");
        foreach (string w in methodD) Console.WriteLine(w);
    }

    static void Bai3_1()
    {
        int[] mangSo = { 50, 42, 12, 3, 9, 8, 1, 50, 3, 42, 85 };
        Console.WriteLine("\nBài 3.1a - Đếm số phần tử:");
        Console.WriteLine($"Tổng số phần tử: {mangSo.Count()}");
        Console.WriteLine($"Số phần tử chẵn: {mangSo.Count(n => n % 2 == 0)}");
        Console.WriteLine($"Số phần tử lẻ: {mangSo.Count(n => n % 2 != 0)}");

        Console.WriteLine("\nBài 3.1b - Tổng, lớn nhất và nhỏ nhất:");
        Console.WriteLine($"Tổng các giá trị: {mangSo.Sum()}");
        Console.WriteLine($"Giá trị lớn nhất: {mangSo.Max()}");
        Console.WriteLine($"Giá trị nhỏ nhất: {mangSo.Min()}");

        Console.WriteLine("\nBài 3.1c - Số giá trị khác nhau:");
        Console.WriteLine($"Số giá trị khác nhau: {mangSo.Distinct().Count()}");

        var queryGroups = from n in mangSo group n by n % 5;
        Console.WriteLine("\nBài 3.1d - Query Syntax:");
        foreach (var group in queryGroups)
            Console.WriteLine($"Dư {group.Key}: {string.Join(" ", group)}");

        var methodGroups = mangSo.GroupBy(n => n % 5);
        Console.WriteLine("Bài 3.1d - Method Syntax:");
        foreach (var group in methodGroups)
            Console.WriteLine($"Dư {group.Key}: {string.Join(" ", group)}");
    }

    static void Bai3_2()
    {
        string[] monAn =
        {
            "Bún bò Huế", "Hủ tiếu heo", "Bánh canh", "Bánh mì",
            "Nước Cà phê", "Mì quảng", "Cơm tấm", "Nước Chanh dây",
            "Mì xào", "Bún riêu", "Bánh cuốn", "Mì gói",
            "Bún chả", "Hủ tiếu Nam vang"
        };

        int nganNhat = monAn.Min(mon => mon.Length);
        int daiNhat = monAn.Max(mon => mon.Length);
        var nganQuery = from mon in monAn where mon.Length == nganNhat select mon;
        var daiQuery = from mon in monAn where mon.Length == daiNhat select mon;

        Console.WriteLine("\nBài 3.2a - Query Syntax:");
        Console.WriteLine($"Tên ngắn nhất ({nganNhat} ký tự):");
        foreach (string mon in nganQuery) Console.WriteLine(mon);
        Console.WriteLine($"Tên dài nhất ({daiNhat} ký tự):");
        foreach (string mon in daiQuery) Console.WriteLine(mon);

        Console.WriteLine("Bài 3.2a - Method Syntax:");
        foreach (string mon in monAn.Where(mon => mon.Length == nganNhat)) Console.WriteLine(mon);
        foreach (string mon in monAn.Where(mon => mon.Length == daiNhat)) Console.WriteLine(mon);

        var queryGroups = from mon in monAn group mon by mon.Split(' ')[0];
        Console.WriteLine("\nBài 3.2b - Query Syntax:");
        foreach (var group in queryGroups)
            Console.WriteLine($"Nhóm {group.Key}: {string.Join(", ", group)}");

        var methodGroups = monAn.GroupBy(mon => mon.Split(' ')[0]);
        Console.WriteLine("Bài 3.2b - Method Syntax:");
        foreach (var group in methodGroups)
            Console.WriteLine($"Nhóm {group.Key}: {string.Join(", ", group)}");

        int queryCount = (from mon in monAn where mon.Split(' ')[0] == "Bánh" select mon).Count();
        int methodCount = monAn.Count(mon => mon.Split(' ')[0] == "Bánh");
        Console.WriteLine("\nBài 3.2c - Số món có từ đầu tiên là Bánh:");
        Console.WriteLine($"Query Syntax + Count(): {queryCount}");
        Console.WriteLine($"Method Syntax: {methodCount}");
    }

    static void Bai4_1()
    {
        List<MonHoc> danhSach = DuLieu.DS_Mon();
        Console.WriteLine("\nBài 4.1 - Danh sách môn học:");
        Console.WriteLine($"{"Mã môn",-8} | {"Tên môn",-45} | {"Hệ",-5} | {"Số tiết",7}");
        foreach (MonHoc mon in danhSach)
            Console.WriteLine($"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}");
        Console.WriteLine($"Tổng số môn: {danhSach.Count}");
    }

    static void Bai5_1()
    {
        List<MonHoc> danhSach = DuLieu.DS_Mon();
        var queryA = from mon in danhSach where mon.TenMon.StartsWith("Lập trình") select mon.TenMon;
        var methodA = danhSach.Where(mon => mon.TenMon.StartsWith("Lập trình")).Select(mon => mon.TenMon);
        Console.WriteLine("\nBài 5.1a - Môn bắt đầu bằng Lập trình:");
        Console.WriteLine("Query: " + string.Join(", ", queryA));
        Console.WriteLine("Method: " + string.Join(", ", methodA));

        var queryB = from mon in danhSach where mon.He == "CD" orderby mon.SoTiet descending, mon.MaMon ascending select mon;
        var methodB = danhSach.Where(mon => mon.He == "CD").OrderByDescending(mon => mon.SoTiet).ThenBy(mon => mon.MaMon);
        Console.WriteLine("\nBài 5.1b - Môn hệ CD, số tiết giảm dần:");
        foreach (MonHoc mon in queryB) Console.WriteLine($"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet,3}");
        Console.WriteLine("Method Syntax:");
        foreach (MonHoc mon in methodB) Console.WriteLine($"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet,3}");

        var queryC = from mon in danhSach where mon.TenMon.Contains("web", StringComparison.OrdinalIgnoreCase) select new { mon.TenMon, mon.He };
        var methodC = danhSach.Where(mon => mon.TenMon.Contains("web", StringComparison.OrdinalIgnoreCase)).Select(mon => new { mon.TenMon, mon.He });
        Console.WriteLine("\nBài 5.1c - Tên môn có chứa web:");
        foreach (var mon in queryC) Console.WriteLine($"{mon.TenMon} | {mon.He}");
        Console.WriteLine("Method Syntax:");
        foreach (var mon in methodC) Console.WriteLine($"{mon.TenMon} | {mon.He}");

        var queryD = from mon in danhSach where mon.He == "KTV" orderby mon.MaMon ascending select mon;
        var methodD = danhSach.Where(mon => mon.He == "KTV").OrderBy(mon => mon.MaMon);
        Console.WriteLine("\nBài 5.1d - Môn hệ KTV:");
        foreach (MonHoc mon in queryD) Console.WriteLine($"{mon.MaMon} | {mon.TenMon}");
        Console.WriteLine("Method Syntax:");
        foreach (MonHoc mon in methodD) Console.WriteLine($"{mon.MaMon} | {mon.TenMon}");
    }

    static void Bai5_2()
    {
        List<MonHoc> danhSach = DuLieu.DS_Mon();
        Console.WriteLine("\nBài 5.2a - Tổng số môn: " + danhSach.Count());
        Console.WriteLine("Bài 5.2b - Số môn bắt đầu bằng Lập trình: " + danhSach.Count(mon => mon.TenMon.StartsWith("Lập trình")));
        Console.WriteLine("Bài 5.2c - Tổng số tiết hệ KTV: " + danhSach.Where(mon => mon.He == "KTV").Sum(mon => (int)mon.SoTiet));

        Console.WriteLine("\nBài 5.2d - Tổng số môn của mỗi hệ:");
        foreach (var group in danhSach.GroupBy(mon => mon.He))
            Console.WriteLine($"Hệ {(group.Key == "" ? "Chưa khai báo" : group.Key)}: {group.Count()} môn");

        Console.WriteLine("\nBài 5.2e - Số môn theo số tiết, giảm dần:");
        foreach (var group in danhSach.GroupBy(mon => mon.SoTiet).OrderByDescending(group => group.Key))
            Console.WriteLine($"{group.Key} tiết: {group.Count()} môn");

        Console.WriteLine("\nBài 5.2f - Môn có số tiết cao nhất:");
        int tietCaoNhat = danhSach.Max(mon => (int)mon.SoTiet);
        foreach (MonHoc mon in danhSach.Where(mon => mon.SoTiet == tietCaoNhat))
            Console.WriteLine($"{mon.MaMon} | {mon.TenMon} | {mon.He} | {mon.SoTiet} tiết");

        Console.WriteLine("\nBài 5.2g - Thống kê theo hệ:");
        foreach (var group in danhSach.GroupBy(mon => mon.He))
        {
            Console.WriteLine($"Hệ {(group.Key == "" ? "Chưa khai báo" : group.Key)}: {group.Count()} môn, {group.Sum(mon => (int)mon.SoTiet)} tiết, cao nhất {group.Max(mon => mon.SoTiet)}, thấp nhất {group.Min(mon => mon.SoTiet)}");
        }

        Console.WriteLine("\nBài 5.2h - Danh sách môn theo hệ:");
        foreach (var group in danhSach.GroupBy(mon => mon.He))
        {
            Console.WriteLine($"Hệ {(group.Key == "" ? "Chưa khai báo" : group.Key)}:");
            foreach (MonHoc mon in group) Console.WriteLine($"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet} tiết");
        }

        Console.WriteLine("\nBài 5.2i - Danh sách môn theo số tiết, tăng dần:");
        foreach (var group in danhSach.GroupBy(mon => mon.SoTiet).OrderBy(group => group.Key))
        {
            Console.WriteLine($"Nhóm {group.Key} tiết:");
            foreach (MonHoc mon in group) Console.WriteLine($"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He}");
        }

        Console.WriteLine("\nBài 5.2j - Môn hệ KTV theo học phần:");
        var theoHocPhan = danhSach.Where(mon => mon.He == "KTV").OrderBy(mon => mon.MaMon).GroupBy(mon => mon.MaMon.Split('_')[0]);
        foreach (var group in theoHocPhan)
        {
            Console.WriteLine($"Học phần {group.Key}:");
            foreach (MonHoc mon in group) Console.WriteLine($"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet} tiết");
        }

        Console.WriteLine("\nBài 5.2k - Môn trên 40 tiết, phân nhóm theo hệ:");
        var trenBonMuoi = danhSach.Where(mon => mon.SoTiet > 40).GroupBy(mon => mon.He).Select(group => new { He = group.Key, CacMon = group.OrderBy(mon => mon.MaMon) });
        foreach (var group in trenBonMuoi)
        {
            Console.WriteLine($"Hệ {(group.He == "" ? "Chưa khai báo" : group.He)}:");
            foreach (MonHoc mon in group.CacMon) Console.WriteLine($"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet} tiết");
        }
    }

    static void Bai6_1()
    {
        List<He> danhSachHe = DuLieu.DS_He();
        Console.WriteLine("\nBài 6.1 - Danh sách hệ đào tạo:");
        Console.WriteLine($"{"Mã hệ",-8} | {"Tên hệ",-25}");
        foreach (He he in danhSachHe) Console.WriteLine($"{he.MaHe,-8} | {he.TenHe,-25}");
        Console.WriteLine($"Tổng số hệ: {danhSachHe.Count}");
    }

    static void Bai6_2()
    {
        List<MonHoc> danhSachMon = DuLieu.DS_Mon();
        List<He> danhSachHe = DuLieu.DS_He();

        Console.WriteLine("\nBài 6.2a - Môn học có hệ tương ứng:");
        var ketQuaA = from he in danhSachHe join mon in danhSachMon on he.MaHe equals mon.He select new { he.TenHe, mon.MaMon, mon.TenMon };
        foreach (var item in ketQuaA) Console.WriteLine($"{item.TenHe,-20} | {item.MaMon,-8} | {item.TenMon}");

        Console.WriteLine("\nBài 6.2b - Tất cả hệ và môn học tương ứng:");
        var ketQuaB = from he in danhSachHe
                      join mon in danhSachMon on he.MaHe equals mon.He into nhomMon
                      from mon in nhomMon.DefaultIfEmpty()
                      select new { he.TenHe, MaMon = mon?.MaMon ?? "(Không có)", TenMon = mon?.TenMon ?? "(Chưa có môn học)" };
        foreach (var item in ketQuaB) Console.WriteLine($"{item.TenHe,-20} | {item.MaMon,-12} | {item.TenMon}");

        var monKhongCoHe = from mon in danhSachMon
                           where !danhSachHe.Any(he => he.MaHe == mon.He)
                           select new { TenHe = "(Chưa khai báo hệ)", mon.MaMon, mon.TenMon };
        Console.WriteLine("\nBài 6.2c - Tất cả hệ và tất cả môn:");
        foreach (var item in ketQuaB.Concat(monKhongCoHe)) Console.WriteLine($"{item.TenHe,-20} | {item.MaMon,-12} | {item.TenMon}");

        Console.WriteLine("\nBài 6.2d - Hệ chưa có môn và môn chưa có hệ:");
        var heKhongCoMon = from he in danhSachHe
                           where !danhSachMon.Any(mon => mon.He == he.MaHe)
                           select new { he.TenHe, MaMon = "(Không có)", TenMon = "(Chưa có môn học)" };
        foreach (var item in heKhongCoMon.Concat(monKhongCoHe)) Console.WriteLine($"{item.TenHe,-20} | {item.MaMon,-12} | {item.TenMon}");

        Console.WriteLine("\nBài 6.2e - 5 môn có số tiết cao nhất:");
        var monKemHe = from mon in danhSachMon
                       join he in danhSachHe on mon.He equals he.MaHe into nhomHe
                       from he in nhomHe.DefaultIfEmpty()
                       select new { TenHe = he?.TenHe ?? "(Chưa khai báo hệ)", mon.MaMon, mon.TenMon, mon.SoTiet };
        foreach (var item in monKemHe.OrderByDescending(mon => mon.SoTiet).Take(5))
            Console.WriteLine($"{item.TenHe,-20} | {item.MaMon,-8} | {item.TenMon,-45} | {item.SoTiet} tiết");

        Console.WriteLine("\nBài 6.2f - Tổng số môn của mỗi hệ:");
        var ketQuaF = from he in danhSachHe
                      join mon in danhSachMon on he.MaHe equals mon.He into nhomMon
                      select new { he.MaHe, he.TenHe, TongSoMon = nhomMon.Count() };
        foreach (var item in ketQuaF) Console.WriteLine($"{item.MaHe,-5} | {item.TenHe,-20} | {item.TongSoMon} môn");

        int soLoaiSoTiet = danhSachMon.Select(mon => mon.SoTiet).Distinct().Count();
        Console.WriteLine($"\nBài 6.2g - Có {soLoaiSoTiet} loại số tiết khác nhau.");

        Console.WriteLine("\nBài 6.2h - Môn đầu tiên bắt đầu bằng Lập trình:");
        MonHoc? ketQuaH = danhSachMon.FirstOrDefault(mon => mon.TenMon.StartsWith("Lập trình"));
        if (ketQuaH != null)
            Console.WriteLine($"{ketQuaH.MaMon} | {ketQuaH.TenMon} | {ketQuaH.He} | {ketQuaH.SoTiet} tiết");
        else
            Console.WriteLine("Không tìm thấy môn phù hợp.");

        Console.WriteLine("\nBài 6.2i - Danh sách môn theo hệ có số thứ tự:");
        var ketQuaI = from he in danhSachHe
                      join mon in danhSachMon on he.MaHe equals mon.He into nhomMon
                      select new
                      {
                          he.MaHe,
                          he.TenHe,
                          CacMon = nhomMon.Select((mon, index) => new { STT = index + 1, mon.MaMon, mon.TenMon, mon.SoTiet })
                      };
        foreach (var group in ketQuaI)
        {
            Console.WriteLine($"\nHệ {group.MaHe} - {group.TenHe}:");
            if (!group.CacMon.Any()) Console.WriteLine("  Chưa có môn học.");
            foreach (var mon in group.CacMon)
                Console.WriteLine($"  {mon.STT,2}. {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet} tiết");
        }
    }
}
