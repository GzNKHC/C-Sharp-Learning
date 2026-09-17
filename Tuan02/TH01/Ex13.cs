using System;

class Student
{
    public string ID;
    public string Fullname;
    public string Address;
    public int Year;
    public void input()
    {
        Console.WriteLine("Nhập mã sinh viên: ");
        ID = Console.ReadLine();

        Console.WriteLine("Nhập họ tên sinh viên: ");
        Fullname = Console.ReadLine();

        Console.WriteLine("Nhập địa chỉ sinh viên: ");
        Address = Console.ReadLine();

        Console.WriteLine("Sinh viên năm thứ: ");
        Year = int.Parse(Console.ReadLine());
    }

    public void InfoDisplay()
    {
        Console.WriteLine($"""
        ====== THÔNG TIN SINH VIÊN ======
        Mã sinh viên: {ID}
        Họ tên: {Fullname}
        Địa chỉ: {Address}
        Sinh viên năm thứ {Year}
        =================================
        """);        
    }
}

class Program
{
    static void Main()
    {
        Student s = new Student();
        s.input();
        s.InfoDisplay();
    }
}