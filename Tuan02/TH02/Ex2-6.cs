using System;

public class PhanSo
{
    public int TuSo { get; set; }
    public int MauSo { get; set; }

    public PhanSo()
    {
        TuSo = 0;
        MauSo = 1;
    }

    public PhanSo(int tu, int mau)
    {
        TuSo = tu;
        MauSo = mau != 0 ? mau : 1;
    }

    public void Nhap()
    {
        Console.Write("  + Nhập tử số: ");
        TuSo = int.Parse(Console.ReadLine());
        do
        {
            Console.Write("  + Nhập mẫu số (khác 0): ");
            MauSo = int.Parse(Console.ReadLine());
            if (MauSo == 0)
                Console.WriteLine(" Mẫu số phải khác 0. Vui lòng nhập lại!");
        } while (MauSo == 0);
    }

    public void Xuat()
    {
        if (MauSo == 1 || TuSo == 0)
            Console.Write($"{TuSo}");
        else
            Console.Write($"{TuSo}/{MauSo}");
    }
    private int UCLN(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b > 0)
        {
            int tmp = a % b;
            a = b;
            b = tmp;
        }
        return a;
    }

    public void RutGon()
    {
        int ucln = UCLN(TuSo, MauSo);
        TuSo /= ucln;
        MauSo /= ucln;

        if (MauSo < 0)
        {
            TuSo = -TuSo;
            MauSo = -MauSo;
        }
    }

    public PhanSo Cong(PhanSo psKhac)
    {
        PhanSo ketQua = new PhanSo();
        ketQua.TuSo = this.TuSo * psKhac.MauSo + psKhac.TuSo * this.MauSo;
        ketQua.MauSo = this.MauSo * psKhac.MauSo;
        ketQua.RutGon();
        return ketQua;
    }
}

public class DayPhanSo
{
    private PhanSo[] danhSach;
    private int n;

    public DayPhanSo()
    {
        n = 0;
        danhSach = new PhanSo[0];
    }

    public void NhapDay()
    {
        Console.Write("Nhập số lượng phân số (n): ");
        if (int.TryParse(Console.ReadLine(), out n) && n > 0)
        {
            danhSach = new PhanSo[n];
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Nhập phân số thứ {i + 1}:");
                danhSach[i] = new PhanSo();
                danhSach[i].Nhap();
            }
        }
        else
        {
            Console.WriteLine("Số lượng không hợp lệ.");
            n = 0;
            danhSach = new PhanSo[0];
        }
    }

    public void XuatDay()
    {
        if (n == 0)
        {
            Console.WriteLine("Danh sách rỗng.");
            return;
        }

        for (int i = 0; i < n; i++)
        {
            danhSach[i].Xuat();
            if (i < n - 1) Console.Write(" ; ");
        }
        Console.WriteLine();
    }

    public PhanSo TinhTong()
    {
        PhanSo tong = new PhanSo(0, 1); 

        for (int i = 0; i < n; i++)
        {
            tong = tong.Cong(danhSach[i]); 
        }
        return tong;
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        DayPhanSo dayPS = new DayPhanSo();

        Console.WriteLine("--- NHẬP DANH SÁCH PHÂN SỐ ---");
        dayPS.NhapDay();

        Console.WriteLine("\n--- DANH SÁCH PHÂN SỐ VỪA NHẬP ---");
        dayPS.XuatDay();

        Console.WriteLine("\n--- KẾT QUẢ ---");
        PhanSo tong = dayPS.TinhTong();
        Console.Write("Tổng của n phân số trên là: ");
        tong.Xuat();
        
        Console.WriteLine();
        Console.ReadLine();
    }
}