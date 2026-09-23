using System;

public class Mang2Chieu
{
    private int[,] data;
    private int n; //dòng
    private int m; //cột

    public Mang2Chieu()
    {
        n = 0;
        m = 0;
        data = new int[0, 0];
    }

    public Mang2Chieu(int rows, int cols)
    {
        n = rows;
        m = cols;
        data = new int[n, m];
    }

    public Mang2Chieu(Mang2Chieu other)
    {
        this.n = other.n;
        this.m = other.m;
        this.data = new int[this.n, this.m];
        
        for (int i = 0; i < this.n; i++)
        {
            for (int j = 0; j < this.m; j++)
            {
                this.data[i, j] = other.data[i, j];
            }
        }
    }

    public int this[int i, int j]
    {
        get
        {
            if (i >= 0 && i < n && j >= 0 && j < m)
                return data[i, j];
            throw new IndexOutOfRangeException("Chỉ số (i, j) nằm ngoài giới hạn của mảng!");
        }
        set
        {
            if (i >= 0 && i < n && j >= 0 && j < m)
                data[i, j] = value;
            else
                throw new IndexOutOfRangeException("Chỉ số (i, j) nằm ngoài giới hạn của mảng!");
        }
    }

    public void Nhap()
    {
        Console.Write("Nhập số dòng (n): ");
        if (!int.TryParse(Console.ReadLine(), out n) || n <= 0) n = 0;
        
        Console.Write("Nhập số cột (m): ");
        if (!int.TryParse(Console.ReadLine(), out m) || m <= 0) m = 0;

        if (n > 0 && m > 0)
        {
            data = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write($"Nhập phần tử [{i},{j}]: ");
                    data[i, j] = int.Parse(Console.ReadLine());
                }
            }
        }
        else
        {
            Console.WriteLine("Kích thước mảng không hợp lệ. Khởi tạo mảng rỗng.");
            data = new int[0, 0];
        }
    }

    public void Xuat()
    {
        if (n == 0 || m == 0)
        {
            Console.WriteLine("Mảng rỗng.");
            return;
        }

        Console.WriteLine("Các phần tử trong mảng 2 chiều:");
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Console.Write(data[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    private bool KiemTraNguyenTo(int so)
    {
        if (so < 2) return false;
        for (int i = 2; i <= Math.Sqrt(so); i++)
        {
            if (so % i == 0) return false;
        }
        return true;
    }
    public void TimSoNguyenTo()
    {
        Console.Write("Các số nguyên tố trong mảng: ");
        bool hasPrime = false;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (KiemTraNguyenTo(data[i, j]))
                {
                    Console.Write(data[i, j] + " ");
                    hasPrime = true;
                }
            }
        }

        if (!hasPrime)
        {
            Console.Write("Không có số nguyên tố nào.");
        }
        Console.WriteLine();
    }
    
    public int Rows => n;
    public int Cols => m;
}
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Mang2Chieu mang = new Mang2Chieu();
        Console.WriteLine("--- NHẬP MẢNG 2 CHIỀU ---");
        mang.Nhap();
        
        Console.WriteLine("\n--- XUẤT MẢNG 2 CHIỀU ---");
        mang.Xuat();

        if (mang.Rows > 0 && mang.Cols > 0)
        {
            Console.WriteLine($"\n--- Thử nghiệm Indexer tại (0, 0) ---");
            Console.WriteLine($"Phần tử [0,0] ban đầu: {mang[0, 0]}");
            mang[0, 0] = 999; // Ghi đè bằng indexer
            Console.WriteLine($"Phần tử [0,0] sau khi sửa: {mang[0, 0]}");
        }

        Console.WriteLine($"\n--- Thử nghiệm Tìm số nguyên tố ---");
        mang.TimSoNguyenTo();
        
        Console.ReadLine();
    }
}