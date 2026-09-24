using System;

class SinhVien : IComparable<SinhVien>
{
    // Field
    private string hoTen;
    private double diemTB;

    // Constructor
    public SinhVien(string hoTen, double diemTB)
    {
        this.hoTen = hoTen;
        this.diemTB = diemTB;
    }

    // Property
    public string HoTen
    {
        get { return hoTen; }
        set { hoTen = value; }
    }

    public double DiemTB
    {
        get { return diemTB; }
        set { diemTB = value; }
    }

    // Implement Interface
    public int CompareTo(SinhVien other)
    {
        return diemTB.CompareTo(other.diemTB);
    }

    // Method
    public void XuatTT()
    {
        Console.WriteLine($"Họ tên: {hoTen}, Điểm TB: {diemTB}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        SinhVien[] ds =
        {
            new SinhVien("Nguyễn A", 8.5),
            new SinhVien("Trần Văn B", 6.5),
            new SinhVien("Lê Văn C", 9.0),
            new SinhVien("Phạm Văn D", 7.5)
        };

        Console.WriteLine("=== DANH SÁCH BAN ĐẦU ===");
        foreach (SinhVien sv in ds)
        {
            sv.XuatTT();
        }

        Array.Sort(ds);

        Console.WriteLine("\n=== SAU KHI SẮP XẾP ===");
        foreach (SinhVien sv in ds)
        {
            sv.XuatTT();
        }
    }
}