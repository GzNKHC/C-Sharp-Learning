using System;

public class DaySo
{    private int[] data;
    private int n;

    public DaySo()
    {
        n = 0;
        data = new int[0];
    }

    public DaySo(int size)
    {
        n = size;
        data = new int[n];
    }

    public DaySo(DaySo other)
    {
        this.n = other.n;
        this.data = new int[this.n];
        for (int i = 0; i < this.n; i++)
        {
            this.data[i] = other.data[i];
        }
    }

    public int this[int i]
    {
        get
        {
            if (i >= 0 && i < n)
                return data[i];
            throw new IndexOutOfRangeException("Chỉ số nằm ngoài giới hạn của mảng!");
        }
        set
        {
            if (i >= 0 && i < n)
                data[i] = value;
            else
                throw new IndexOutOfRangeException("Chỉ số nằm ngoài giới hạn của mảng!");
        }
    }

    public void Nhap()
    {
        Console.Write("Nhập số lượng phần tử của dãy (n): ");
        if (int.TryParse(Console.ReadLine(), out n) && n > 0)
        {
            data = new int[n];
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Nhập phần tử thứ {i}: ");
                data[i] = int.Parse(Console.ReadLine());
            }
        }
        else
        {
            Console.WriteLine("Số lượng phần tử không hợp lệ. Khởi tạo mảng rỗng.");
            n = 0;
            data = new int[0];
        }
    }

    public void Xuat()
    {
        if (n == 0)
        {
            Console.WriteLine("Dãy số rỗng.");
            return;
        }

        Console.Write("Các phần tử trong dãy: ");
        for (int i = 0; i < n; i++)
        {
            Console.Write(data[i] + " ");
        }
        Console.WriteLine();
    }

    public void TimSoChan()
    {
        Console.Write("Các số chẵn trong dãy: ");
        bool found = false;
        
        for (int i = 0; i < n; i++)
        {
            if (data[i] % 2 == 0)
            {
                Console.Write(data[i] + " ");
                found = true;
            }
        }
        
        if (!found) 
        {
            Console.Write("Không có số chẵn nào.");
        }
        Console.WriteLine();
    }
    public int Length => n;
}
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        DaySo ds = new DaySo();
        
        ds.Nhap();
        ds.Xuat();
        if (ds.Length > 0)
        {
            Console.WriteLine($"\n--- Thử nghiệm Indexer ---");
            Console.WriteLine($"Phần tử tại index 0 ban đầu: {ds[0]}");
            ds[0] = 999; // Ghi đè bằng indexer
            Console.WriteLine($"Phần tử tại index 0 sau khi sửa: {ds[0]}");
        }

        Console.WriteLine($"\n--- Thử nghiệm Tìm số chẵn ---");
        ds.TimSoChan();
        Console.ReadLine();
    }
}