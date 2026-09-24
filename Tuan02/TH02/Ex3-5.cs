using System;

abstract class NhanVien
{
    public string MaNV { get; set; }
    public string HoTen { get; set; }

    public NhanVien(string maNV, string hoTen)
    {
        MaNV = maNV;
        HoTen = hoTen;
    }

    //abstract
    public abstract double TinhLuong();

    public virtual void XuatThongTin()
    {
        Console.WriteLine($"Mã nhân viên: {MaNV}");
        Console.WriteLine($"Họ tên: {HoTen}");
        Console.WriteLine($"Lương: {TinhLuong():N0} VNĐ");
    }
}

//NV KD
class NhanVienKinhDoanh : NhanVien
{
    public double LuongCoBan { get; set; }
    public int SoHopDong { get; set; }

    public NhanVienKinhDoanh(
        string maNV,
        string hoTen,
        double luongCoBan,
        int soHopDong
    ) : base(maNV, hoTen)
    {
        LuongCoBan = luongCoBan;
        SoHopDong = soHopDong;
    }

    public override double TinhLuong()
    {
        return LuongCoBan + SoHopDong * 500000;
    }

    public override void XuatThongTin()
    {
        Console.WriteLine("\n--- NHÂN VIÊN KINH DOANH ---");
        base.XuatThongTin();
        Console.WriteLine($"Lương cơ bản: {LuongCoBan:N0} VNĐ");
        Console.WriteLine($"Số hợp đồng: {SoHopDong}");
    }
}

//NV SX
class NhanVienSanXuat : NhanVien
{
    public int SoSanPham { get; set; }

    public NhanVienSanXuat(
        string maNV,
        string hoTen,
        int soSanPham
    ) : base(maNV, hoTen)
    {
        SoSanPham = soSanPham;
    }

    public override double TinhLuong()
    {
        double luong = SoSanPham * 1000;

        if (SoSanPham > 3000)
        {
            luong = luong + luong * 0.05;
        }

        return luong;
    }

    public override void XuatThongTin()
    {
        Console.WriteLine("\n--- NHÂN VIÊN SẢN XUẤT ---");
        base.XuatThongTin();
        Console.WriteLine($"Số sản phẩm: {SoSanPham}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Nhập số nhân viên: ");
        int n = int.Parse(Console.ReadLine());

        NhanVien[] ds = new NhanVien[n];

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"\n=== NHẬP NHÂN VIÊN {i + 1} ===");
            Console.WriteLine("1. Nhân viên kinh doanh");
            Console.WriteLine("2. Nhân viên sản xuất");
            Console.Write("Chọn loại nhân viên: ");

            int loai = int.Parse(Console.ReadLine());

            Console.Write("Mã nhân viên: ");
            string maNV = Console.ReadLine();

            Console.Write("Họ tên: ");
            string hoTen = Console.ReadLine();

            if (loai == 1)
            {
                Console.Write("Lương cơ bản: ");
                double luongCoBan = double.Parse(Console.ReadLine());

                Console.Write("Số hợp đồng ký được: ");
                int soHopDong = int.Parse(Console.ReadLine());

                ds[i] = new NhanVienKinhDoanh(
                    maNV,
                    hoTen,
                    luongCoBan,
                    soHopDong
                );
            }
            else if (loai == 2)
            {
                Console.Write("Số sản phẩm: ");
                int soSanPham = int.Parse(Console.ReadLine());

                ds[i] = new NhanVienSanXuat(
                    maNV,
                    hoTen,
                    soSanPham
                );
            }
            else
            {
                Console.WriteLine("Loại nhân viên không hợp lệ!");
                i--;
            }
        }

        Console.WriteLine("\n\n===== DANH SÁCH NHÂN VIÊN =====");

        foreach (NhanVien nv in ds)
        {
            nv.XuatThongTin();
        }
    }
}