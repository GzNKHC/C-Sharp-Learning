using System;

abstract class ThiSinh
{
    public string SBD { get; set; }
    public string HoTen { get; set; }
    public double Bai1 { get; set; }
    public double Bai2 { get; set; }
    public double Bai3 { get; set; }

    public ThiSinh(string sbd, string hoTen,
                   double bai1, double bai2, double bai3)
    {
        SBD = sbd;
        HoTen = hoTen;
        Bai1 = bai1;
        Bai2 = bai2;
        Bai3 = bai3;
    }

    public abstract double TinhTongDiem();

    public virtual void XuatThongTin()
    {
        Console.WriteLine($"SBD: {SBD}");
        Console.WriteLine($"Họ tên: {HoTen}");
        Console.WriteLine($"Bài 1: {Bai1}");
        Console.WriteLine($"Bài 2: {Bai2}");
        Console.WriteLine($"Bài 3: {Bai3}");
        Console.WriteLine($"Tổng điểm: {TinhTongDiem()}");
    }
}

class ThiSinhChuyen : ThiSinh
{
    public double TiengAnh { get; set; }

    public ThiSinhChuyen(string sbd, string hoTen,
                         double bai1, double bai2, double bai3,
                         double tiengAnh)
        : base(sbd, hoTen, bai1, bai2, bai3)
    {
        TiengAnh = tiengAnh;
    }

    public override double TinhTongDiem()
    {
        double tong = Bai1 + Bai2 + Bai3;

        if (TiengAnh >= 7 && TiengAnh <= 8)
            tong += 1;
        else if (TiengAnh >= 9 && TiengAnh <= 10)
            tong += 2;

        return tong;
    }

    public override void XuatThongTin()
    {
        Console.WriteLine("\n--- THÍ SINH CHUYÊN ---");
        base.XuatThongTin();
        Console.WriteLine($"Điểm tiếng Anh: {TiengAnh}");
    }
}

class ThiSinhSieuCup : ThiSinh
{
    public double CSDL { get; set; }

    public ThiSinhSieuCup(string sbd, string hoTen,
                          double bai1, double bai2, double bai3,
                          double csdl)
        : base(sbd, hoTen, bai1, bai2, bai3)
    {
        CSDL = csdl;
    }

    public override double TinhTongDiem()
    {
        return Bai1 + Bai2 + Bai3 + CSDL;
    }

    public override void XuatThongTin()
    {
        Console.WriteLine("\n--- THÍ SINH SIÊU CÚP ---");
        base.XuatThongTin();
        Console.WriteLine($"Điểm CSDL: {CSDL}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Nhập số lượng thí sinh: ");
        int n = int.Parse(Console.ReadLine());

        ThiSinh[] danhSach = new ThiSinh[n];

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"\n=== NHẬP THÍ SINH {i + 1} ===");
            Console.WriteLine("1. Thí sinh Chuyên");
            Console.WriteLine("2. Thí sinh Siêu cúp");
            Console.Write("Chọn loại thí sinh: ");

            int loai = int.Parse(Console.ReadLine());

            Console.Write("Số báo danh: ");
            string sbd = Console.ReadLine();

            Console.Write("Họ tên: ");
            string hoTen = Console.ReadLine();

            Console.Write("Điểm bài 1: ");
            double bai1 = double.Parse(Console.ReadLine());

            Console.Write("Điểm bài 2: ");
            double bai2 = double.Parse(Console.ReadLine());

            Console.Write("Điểm bài 3: ");
            double bai3 = double.Parse(Console.ReadLine());

            if (loai == 1)
            {
                Console.Write("Điểm tiếng Anh: ");
                double tiengAnh = double.Parse(Console.ReadLine());

                danhSach[i] = new ThiSinhChuyen(
                    sbd, hoTen, bai1, bai2, bai3, tiengAnh);
            }
            else if (loai == 2)
            {
                Console.Write("Điểm CSDL: ");
                double csdl = double.Parse(Console.ReadLine());

                danhSach[i] = new ThiSinhSieuCup(
                    sbd, hoTen, bai1, bai2, bai3, csdl);
            }
            else
            {
                Console.WriteLine("Loại thí sinh không hợp lệ");
                i--;
            }
        }

        Console.WriteLine("\n===== DANH SÁCH THÍ SINH=====");

        foreach (ThiSinh ts in danhSach)
        {
            ts.XuatThongTin();
        }
    }
}