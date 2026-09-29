using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BaiThucHanhLINQ;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Bai5_1();
        Bai5_2();
    }

    static void Bai5_1()
    {
        List<MonHoc> danhSach = DuLieu.DS_Mon();

        // Câu a
        var queryA = from mon in danhSach
                    where mon.TenMon.StartsWith("Lập trình")
                    select mon.TenMon;

        var methodA = danhSach
            .Where(mon => mon.TenMon.StartsWith("Lập trình"))
            .Select(mon => mon.TenMon);

        Console.WriteLine("Bài 5.1a - Query Syntax:");
        foreach (string tenMon in queryA)
        {
            Console.WriteLine(tenMon);
        }

        Console.WriteLine("\nBài 5.1a - Method Syntax:");
        foreach (string tenMon in methodA)
        {
            Console.WriteLine(tenMon);
        }

        // Câu b
        var queryB = from mon in danhSach
                    where mon.He == "CD"
                    orderby mon.SoTiet descending, mon.MaMon ascending
                    select mon;

        var methodB = danhSach
            .Where(mon => mon.He == "CD")
            .OrderByDescending(mon => mon.SoTiet)
            .ThenBy(mon => mon.MaMon);

        Console.WriteLine("\nBài 5.1b - Query Syntax:");
        foreach (MonHoc mon in queryB)
        {
            Console.WriteLine(
                $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
            );
        }

        Console.WriteLine("\nBài 5.1b - Method Syntax:");
        foreach (MonHoc mon in methodB)
        {
            Console.WriteLine(
                $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
            );
        }

        // Câu c
        var queryC = from mon in danhSach
                    where mon.TenMon.Contains(
                        "web", StringComparison.OrdinalIgnoreCase
                    )
                    select new { mon.TenMon, mon.He };

        var methodC = danhSach
            .Where(mon => mon.TenMon.Contains(
                "web", StringComparison.OrdinalIgnoreCase
            ))
            .Select(mon => new { mon.TenMon, mon.He });

        Console.WriteLine("\nBài 5.1c - Query Syntax:");
        foreach (var mon in queryC)
        {
            Console.WriteLine($"{mon.TenMon,-45} | {mon.He}");
        }

        Console.WriteLine("\nBài 5.1c - Method Syntax:");
        foreach (var mon in methodC)
        {
            Console.WriteLine($"{mon.TenMon,-45} | {mon.He}");
        }

        // Câu d
        var queryD = from mon in danhSach
                    where mon.He == "KTV"
                    orderby mon.MaMon ascending
                    select mon;

        var methodD = danhSach
            .Where(mon => mon.He == "KTV")
            .OrderBy(mon => mon.MaMon);

        Console.WriteLine("\nBài 5.1d - Query Syntax:");
        foreach (MonHoc mon in queryD)
        {
            Console.WriteLine(
                $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
            );
        }

        Console.WriteLine("\nBài 5.1d - Method Syntax:");
        foreach (MonHoc mon in methodD)
        {
            Console.WriteLine(
                $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
            );
        }
    }
    static void Bai5_2()
    {
        List<MonHoc> danhSach = DuLieu.DS_Mon();

        // Câu a
        Console.WriteLine("Bài 5.2a - Tổng số môn:");

        int tongSoMon = danhSach.Count();
        Console.WriteLine($"Tổng số môn: {tongSoMon}");

        // Câu b
        Console.WriteLine("\nBài 5.2b - Số môn bắt đầu bằng Lập trình:");

        int soMonLapTrinh = danhSach.Count(
            mon => mon.TenMon.StartsWith("Lập trình")
        );

        Console.WriteLine($"Số môn: {soMonLapTrinh}");

        // Câu c
        Console.WriteLine("\nBài 5.2c - Tổng số tiết hệ KTV:");

        int tongTietKTV = danhSach
            .Where(mon => mon.He == "KTV")
            .Sum(mon => (int)mon.SoTiet);

        Console.WriteLine($"Tổng số tiết: {tongTietKTV}");

        // Câu d
        Console.WriteLine("\nBài 5.2d - Tổng số môn của mỗi hệ:");

        var ketQuaD = danhSach
            .GroupBy(mon => mon.He)
            .Select(nhom => new
            {
                He = nhom.Key,
                TongSoMon = nhom.Count()
            });

        foreach (var item in ketQuaD)
        {
            string tenHe = item.He == "" ? "Chưa khai báo" : item.He;
            Console.WriteLine($"Hệ {tenHe}: {item.TongSoMon} môn");
        }

        // Câu e
        Console.WriteLine("\nBài 5.2e - Số môn theo số tiết, giảm dần:");

        var ketQuaE = danhSach
            .GroupBy(mon => mon.SoTiet)
            .Select(nhom => new
            {
                SoTiet = nhom.Key,
                TongSoMon = nhom.Count()
            })
            .OrderByDescending(item => item.SoTiet);

        foreach (var item in ketQuaE)
        {
            Console.WriteLine(
                $"{item.SoTiet} tiết: {item.TongSoMon} môn"
            );
        }

        // Câu f
        Console.WriteLine("\nBài 5.2f - Môn có số tiết cao nhất:");

        int tietCaoNhat = danhSach.Max(mon => (int)mon.SoTiet);

        var ketQuaF = danhSach.Where(
            mon => mon.SoTiet == tietCaoNhat
        );

        foreach (MonHoc mon in ketQuaF)
        {
            Console.WriteLine(
                $"{mon.MaMon} | {mon.TenMon} | {mon.He} | {mon.SoTiet} tiết"
            );
        }

        // Câu g
        Console.WriteLine("\nBài 5.2g - Thống kê theo hệ:");

        var ketQuaG = danhSach
            .GroupBy(mon => mon.He)
            .Select(nhom => new
            {
                He = nhom.Key,
                TongSoMon = nhom.Count(),
                TongSoTiet = nhom.Sum(mon => (int)mon.SoTiet),
                TietCaoNhat = nhom.Max(mon => (int)mon.SoTiet),
                TietThapNhat = nhom.Min(mon => (int)mon.SoTiet)
            });

        foreach (var item in ketQuaG)
        {
            string tenHe = item.He == "" ? "Chưa khai báo" : item.He;

            Console.WriteLine($"Hệ: {tenHe}");
            Console.WriteLine($"  Tổng số môn: {item.TongSoMon}");
            Console.WriteLine($"  Tổng số tiết: {item.TongSoTiet}");
            Console.WriteLine($"  Số tiết cao nhất: {item.TietCaoNhat}");
            Console.WriteLine($"  Số tiết thấp nhất: {item.TietThapNhat}");
        }

        // Câu h
        Console.WriteLine("\nBài 5.2h - Danh sách môn theo hệ:");

        var ketQuaH = danhSach.GroupBy(mon => mon.He);

        foreach (var nhom in ketQuaH)
        {
            string tenHe = nhom.Key == "" ? "Chưa khai báo" : nhom.Key;
            Console.WriteLine($"Hệ: {tenHe}");

            foreach (MonHoc mon in nhom)
            {
                Console.WriteLine(
                    $"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet} tiết"
                );
            }
        }

        // Câu i
        Console.WriteLine("\nBài 5.2i - Danh sách môn theo số tiết, tăng dần:");

        var ketQuaI = danhSach
            .GroupBy(mon => mon.SoTiet)
            .OrderBy(nhom => nhom.Key);

        foreach (var nhom in ketQuaI)
        {
            Console.WriteLine($"Nhóm {nhom.Key} tiết:");

            foreach (MonHoc mon in nhom)
            {
                Console.WriteLine(
                    $"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He}"
                );
            }
        }

        // Câu j
        // Sắp xếp theo mã môn
        Console.WriteLine("\nBài 5.2j - Môn hệ KTV theo học phần:");

        var ketQuaJ = danhSach
            .Where(mon => mon.He == "KTV")
            .OrderBy(mon => mon.MaMon)
            .GroupBy(mon => mon.MaMon.Split('_')[0]);

        foreach (var nhom in ketQuaJ)
        {
            Console.WriteLine($"Học phần {nhom.Key}:");

            foreach (MonHoc mon in nhom)
            {
                Console.WriteLine(
                    $"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet} tiết"
                );
            }
        }

        // Câu k
        // Sắp xếp mã môn trong từng nhóm
        Console.WriteLine("\nBài 5.2k - Môn trên 40 tiết, phân nhóm theo hệ:");

        var ketQuaK = danhSach
            .Where(mon => mon.SoTiet > 40)
            .GroupBy(mon => mon.He)
            .Select(nhom => new
            {
                He = nhom.Key,
                CacMon = nhom.OrderBy(mon => mon.MaMon)
            });

        foreach (var nhom in ketQuaK)
        {
            string tenHe = nhom.He == "" ? "Chưa khai báo" : nhom.He;
            Console.WriteLine($"Hệ: {tenHe}");

            foreach (MonHoc mon in nhom.CacMon)
            {
                Console.WriteLine(
                    $"  {mon.MaMon,-8} | {mon.TenMon,-45} | {mon.SoTiet} tiết"
                );
            }
        }
    }
}