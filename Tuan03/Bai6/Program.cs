using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BaiThucHanhLINQ;

class Program 
{
    static void Bai6_1()
    {
        List<He> danhSachHe = DuLieu.DS_He();

        Console.WriteLine("Bài 6.1 - Danh sách hệ đào tạo:");
        Console.WriteLine($"{"Mã hệ",-8} | {"Tên hệ",-25}");

        foreach (He he in danhSachHe)
        {
            Console.WriteLine($"{he.MaHe,-8} | {he.TenHe,-25}");
        }

        Console.WriteLine($"\nTổng số hệ: {danhSachHe.Count}");
    }

    static void Bai6_2()
{
    List<MonHoc> danhSachMon = DuLieu.DS_Mon();
    List<He> danhSachHe = DuLieu.DS_He();

    //a
    Console.WriteLine("\nBài 6.2a - Môn học có hệ tương ứng:");

    var ketQuaA = from he in danhSachHe
                 join mon in danhSachMon
                     on he.MaHe equals mon.He
                 select new
                 {
                     he.TenHe,
                     mon.MaMon,
                     mon.TenMon
                 };

    foreach (var item in ketQuaA)
    {
        Console.WriteLine(
            $"{item.TenHe,-20} | {item.MaMon,-8} | {item.TenMon}"
        );
    }

    //b
    Console.WriteLine("\nBài 6.2b - Tất cả hệ và môn học tương ứng:");

    var ketQuaB = from he in danhSachHe
                 join mon in danhSachMon
                     on he.MaHe equals mon.He into nhomMon
                 from mon in nhomMon.DefaultIfEmpty()
                 select new
                 {
                     he.TenHe,
                     MaMon = mon?.MaMon ?? "(Không có)",
                     TenMon = mon?.TenMon ?? "(Chưa có môn học)"
                 };

    foreach (var item in ketQuaB)
    {
        Console.WriteLine(
            $"{item.TenHe,-20} | {item.MaMon,-12} | {item.TenMon}"
        );
    }

    //c
    Console.WriteLine("\nBài 6.2c - Tất cả hệ và tất cả môn:");

    var monKhongCoHe = from mon in danhSachMon
                       where !danhSachHe.Any(
                           he => he.MaHe == mon.He
                       )
                       select new
                       {
                           TenHe = "(Chưa khai báo hệ)",
                           mon.MaMon,
                           mon.TenMon
                       };

    var ketQuaC = ketQuaB.Concat(monKhongCoHe);

    foreach (var item in ketQuaC)
    {
        Console.WriteLine(
            $"{item.TenHe,-20} | {item.MaMon,-12} | {item.TenMon}"
        );
    }

    //d
    Console.WriteLine("\nBài 6.2d - Hệ chưa có môn và môn chưa có hệ:");

    var heKhongCoMon = from he in danhSachHe
                       where !danhSachMon.Any(
                           mon => mon.He == he.MaHe
                       )
                       select new
                       {
                           he.TenHe,
                           MaMon = "(Không có)",
                           TenMon = "(Chưa có môn học)"
                       };

    var ketQuaD = heKhongCoMon.Concat(monKhongCoHe);

    foreach (var item in ketQuaD)
    {
        Console.WriteLine(
            $"{item.TenHe,-20} | {item.MaMon,-12} | {item.TenMon}"
        );
    }

    //e
    Console.WriteLine("\nBài 6.2e - 5 môn có số tiết cao nhất:");

    var monKemHe = from mon in danhSachMon
                   join he in danhSachHe
                       on mon.He equals he.MaHe into nhomHe
                   from he in nhomHe.DefaultIfEmpty()
                   select new
                   {
                       TenHe = he?.TenHe ?? "(Chưa khai báo hệ)",
                       mon.MaMon,
                       mon.TenMon,
                       mon.SoTiet
                   };

    var ketQuaE = monKemHe
        .OrderByDescending(mon => mon.SoTiet)
        .Take(5);

    foreach (var item in ketQuaE)
    {
        Console.WriteLine(
            $"{item.TenHe,-20} | {item.MaMon,-8} | " +
            $"{item.TenMon,-45} | {item.SoTiet} tiết"
        );
    }

    //f
    Console.WriteLine("\nBài 6.2f - Tổng số môn của mỗi hệ:");

    var ketQuaF = from he in danhSachHe
                 join mon in danhSachMon
                     on he.MaHe equals mon.He into nhomMon
                 select new
                 {
                     he.MaHe,
                     he.TenHe,
                     TongSoMon = nhomMon.Count()
                 };

    foreach (var item in ketQuaF)
    {
        Console.WriteLine(
            $"{item.MaHe,-5} | {item.TenHe,-20} | {item.TongSoMon} môn"
        );
    }

    //g
    Console.WriteLine("\nBài 6.2g - Số loại số tiết khác nhau:");

    int soLoaiSoTiet = danhSachMon
        .Select(mon => mon.SoTiet)
        .Distinct()
        .Count();

    Console.WriteLine($"Có {soLoaiSoTiet} loại số tiết khác nhau.");

    //h
    Console.WriteLine("\nBài 6.2h - Môn đầu tiên bắt đầu bằng Lập trình:");

    var ketQuaH = danhSachMon.FirstOrDefault(
        mon => mon.TenMon.StartsWith("Lập trình")
    );

    if (ketQuaH != null)
    {
        Console.WriteLine(
            $"{ketQuaH.MaMon} | {ketQuaH.TenMon} | " +
            $"{ketQuaH.He} | {ketQuaH.SoTiet} tiết"
        );
    }
    else
    {
        Console.WriteLine("Không tìm thấy môn phù hợp.");
    }

    //i
    Console.WriteLine("\nBài 6.2i - Danh sách môn theo hệ có số thứ tự:");

    var ketQuaI = from he in danhSachHe
                 join mon in danhSachMon
                     on he.MaHe equals mon.He into nhomMon
                 select new
                 {
                     he.MaHe,
                     he.TenHe,
                     CacMon = nhomMon.Select((mon, index) => new
                     {
                         STT = index + 1,
                         mon.MaMon,
                         mon.TenMon,
                         mon.SoTiet
                     })
                 };

    foreach (var nhom in ketQuaI)
    {
        Console.WriteLine($"\nHệ {nhom.MaHe} - {nhom.TenHe}:");

        if (!nhom.CacMon.Any())
        {
            Console.WriteLine("  Chưa có môn học.");
        }

        foreach (var mon in nhom.CacMon)
        {
            Console.WriteLine(
                $"  {mon.STT,2}. {mon.MaMon,-8} | " +
                $"{mon.TenMon,-45} | {mon.SoTiet} tiết"
            );
        }
    }
}

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Bai6_1();
        Bai6_2();
    }
}
