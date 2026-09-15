using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Nhập số nguyên x: ");
        int x = int.Parse(Console.ReadLine());
        Console.WriteLine("Nhập số nguyên y: ");
        int y = int.Parse(Console.ReadLine());

        double result = Math.Pow(x,y);
        Console.WriteLine($"Kết quả {x} mũ {y} là: {result}"); 
    }
}