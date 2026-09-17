using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Nhập một chuỗi: ");
        string str = Console.ReadLine();

        Console.WriteLine($"""
        Chuyển sang ký tự thường: {str.ToLower()}
        Chuyển sang ký tự hoa: {str.ToUpper()}
        Số từ trong chuỗi: {str.Length}
        """);
    }
}