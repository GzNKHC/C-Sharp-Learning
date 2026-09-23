using System;
using System.Collections.Generic;

public class NhanVien
{
    public string HoTen { get; set; }
    public double MucLuong { get; set; }
    public int SoNgayVang { get; set; }

    public NhanVien()
    {
        HoTen = "";
        MucLuong = 0;
        SoNgayVang = 0;
    }

    public void Nhap()
    {
        Console.Write(" - Nhập họ tên: ");
        HoTen = Console.ReadLine();
        
        Console.Write(" - Nhập mức lương (VNĐ): ");
        MucLuong = double.Parse(Console.ReadLine());
        
        Console.Write(" - Nhập số ngày vắng: ");
        SoNgayVang = int.Parse(Console.ReadLine());
    }

    public void Xuat()
    {
        Console.WriteLine($"- Họ tên: {HoTen} | Mức lương: {MucLuong:N0}đ | Vắng: {SoNgayVang} ngày | Lương thực lãnh: {TinhLuongThucLanh():N0}đ");
    }

    public double TinhLuongThucLanh()
    {
        double luong = MucLuong - (SoNgayVang * 100000);
        return luong > 0 ? luong : 0; 
    }
}

public class PhongBan
{
    private List<NhanVien> danhSachNhanVien;

    public PhongBan()
    {
        danhSachNhanVien = new List<NhanVien>();
    }

    public void Nhap()
    {
        Console.Write("Nhập số lượng nhân viên trong phòng ban (n): ");
        if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\nNhập thông tin nhân viên thứ {i + 1}:");
                NhanVien nv = new NhanVien();
                nv.Nhap();
                danhSachNhanVien.Add(nv);
            }
        }
    }

    public void Xuat()
    {
        if (danhSachNhanVien.Count == 0)
        {
            Console.WriteLine("Phòng ban chưa có nhân viên.");
            return;
        }

        foreach (NhanVien nv in danhSachNhanVien)
        {
            nv.Xuat();
        }
    }

    public double TinhTongLuongPhongBan()
    {
        double tongLuong = 0;
        foreach (NhanVien nv in danhSachNhanVien)
        {
            tongLuong += nv.TinhLuongThucLanh();
        }
        return tongLuong;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        PhongBan pb = new PhongBan();
        
        Console.WriteLine("--- NHẬP THÔNG TIN PHÒNG BAN ---");
        pb.Nhap();

        Console.WriteLine("\n--- DANH SÁCH NHÂN VIÊN ---");
        pb.Xuat();

        Console.WriteLine("\n--- KẾT QUẢ ---");
        double tong = pb.TinhTongLuongPhongBan();
        Console.WriteLine($"Tổng lương của phòng ban là: {tong:N0} VNĐ");

        Console.ReadLine();
    }
}