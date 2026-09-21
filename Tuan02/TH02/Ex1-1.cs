using System;

class SinhVien
{
    //Field:
    private string HoTen;
    private int NamSinh;

    //Constructor
    public SinhVien(string HoTen, int NamSinh)
    {
        this.HoTen = HoTen;
        this.NamSinh = NamSinh;
    }

    //Property:
    public string GetHoTen
    {
        get { return HoTen; }
        set { HoTen = value;}
    }
    public int GetNamSinh
    {
        get { return NamSinh; }
        set { NamSinh = value; }
    }

    //Method:
    public int TinhTuoi()
    {
        int NamHienTai = DateTime.Now.Year;
        return NamHienTai - NamSinh;
    }

    public void XuatTT()
    {
        Console.WriteLine($"""
        Họ tên: {HoTen}
        Năm sinh: {NamSinh}
        Tuổi: {TinhTuoi()} 
        """);
    }
}
class Program()
{
    static void Main(String[] args)
    {
        Console.Write("Nhập họ tên sinh viên: ");
        string HoTen = Console.ReadLine();

        Console.Write("Nhập năm sinh: ");
        int NamSinh = int.Parse(Console.ReadLine());

        SinhVien sv = new SinhVien(HoTen, NamSinh);
        Console.WriteLine("Thông tin sinh viên:");
        sv.XuatTT();
    }
}