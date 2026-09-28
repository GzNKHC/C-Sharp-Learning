using System;
using System.Collections.Generic;
using System.Text;

namespace BaiThucHanhLINQ;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Bai4_1();
    }

    static void Bai4_1()
    {
        List<MonHoc> danhSach = DuLieu.DS_Mon();

        Console.WriteLine("Bài 4.1 - Danh sách môn học:");
        Console.WriteLine(
            $"{"Mã môn",-8} | {"Tên môn",-45} | {"Hệ",-5} | {"Số tiết",7}"
        );

        foreach (MonHoc mon in danhSach)
        {
            Console.WriteLine(
                $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
            );
        }

        Console.WriteLine($"\nTổng số môn: {danhSach.Count}");
    }
}