using System;

public class DaThuc
{
    private int n;
    private double[] heSo;
    public DaThuc()
    {
        n = 0;
        heSo = new double[1];
        heSo[0] = 0;
    }

    public DaThuc(int bac)
    {
        n = bac;
        heSo = new double[n + 1]; 
    }

    public DaThuc(DaThuc other)
    {
        this.n = other.n;
        this.heSo = new double[this.n + 1];
        for (int i = 0; i <= this.n; i++)
        {
            this.heSo[i] = other.heSo[i];
        }
    }

    public double this[int i]
    {
        get
        {
            if (i >= 0 && i <= n)
                return heSo[i];
            throw new IndexOutOfRangeException("Chỉ số không hợp lệ (vượt quá bậc đa thức)!");
        }
        set
        {
            if (i >= 0 && i <= n)
                heSo[i] = value;
            else
                throw new IndexOutOfRangeException("Chỉ số không hợp lệ (vượt quá bậc đa thức)!");
        }
    }

    public void Nhap()
    {
        Console.Write("Nhập bậc của đa thức (n): ");
        if (int.TryParse(Console.ReadLine(), out n) && n >= 0)
        {
            heSo = new double[n + 1];
            for (int i = 0; i <= n; i++)
            {
                Console.Write($"Nhập hệ số a[{i}]: ");
                heSo[i] = double.Parse(Console.ReadLine());
            }
        }
        else
        {
            Console.WriteLine("Bậc đa thức không hợp lệ. Khởi tạo đa thức bậc 0.");
            n = 0;
            heSo = new double[1] { 0 };
        }
    }

    public void Xuat()
    {
        Console.Write("P(x) = ");
        bool isFirst = true;
        
        for (int i = 0; i <= n; i++)
        {
            if (heSo[i] != 0 || (n == 0 && i == 0)) 
            {
                if (!isFirst && heSo[i] > 0)
                    Console.Write(" + ");
                else if (!isFirst && heSo[i] < 0)
                    Console.Write(" - ");
                else if (isFirst && heSo[i] < 0)
                    Console.Write("-");

                double absHeSo = Math.Abs(heSo[i]);

                // Xử lý in biến x và số mũ
                if (i == 0)
                    Console.Write($"{absHeSo}");
                else if (i == 1)
                    Console.Write($"{absHeSo}*x");
                else
                    Console.Write($"{absHeSo}*x^{i}");

                isFirst = false;
            }
        }
        
        if (isFirst) Console.Write("0"); 
        Console.WriteLine();
    }

    public double TinhGiaTri(double x)
    {
        double ketQua = 0;
        for (int i = 0; i <= n; i++)
        {
            ketQua += heSo[i] * Math.Pow(x, i);
        }
        return ketQua;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        DaThuc p = new DaThuc();
        
        Console.WriteLine("--- NHẬP ĐA THỨC ---");
        p.Nhap();
        
        Console.WriteLine("\n--- XUẤT ĐA THỨC ---");
        p.Xuat();

        Console.WriteLine("\n--- THỬ NGHIỆM INDEXER ---");
        try
        {
            Console.WriteLine($"Đơn thức bậc 0 (a0) hiện tại là: {p[0]}");
            p[0] = 99;
            Console.WriteLine($"Đa thức sau khi thay đổi a0 thành 99:");
            p.Xuat();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\n--- TÍNH GIÁ TRỊ ĐA THỨC ---");
        Console.Write("Nhập giá trị x: ");
        if (double.TryParse(Console.ReadLine(), out double x))
        {
            double kQ = p.TinhGiaTri(x);
            Console.WriteLine($"Giá trị của đa thức tại x = {x} là: P({x}) = {kQ}");
        }

        Console.ReadLine();
    }
}