using System;

class NumberSwaper
{
    public static void HoanVi(ref double a, ref double b)
    {
        double tmp = a;
        a = b;
        b = tmp;
    } 
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Nhập a: ");
        double a = double.Parse(Console.ReadLine());
        Console.WriteLine("Nhập b: ");
        double b = double.Parse(Console.ReadLine());

        NumberSwaper.HoanVi(ref a, ref b);

        Console.WriteLine("Đã hoán vị a và b.");
        Console.WriteLine($"a = {a}; b = {b}");
    }
}