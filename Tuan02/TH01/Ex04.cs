using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Nhập số nguyên x: ");
        int x = int.Parse(Console.ReadLine());
        Console.WriteLine("Nhập số nguyên y: ");
        int y = int.Parse(Console.ReadLine());
        //Nếu x và y không phải số nguyên thì terminal in ra lỗi vì đã khai báo x và y là kiểu int
        
        double result = Math.Pow(x,y);
        Console.WriteLine($"Kết quả {x} mũ {y} là: {result}"); 
    }
}