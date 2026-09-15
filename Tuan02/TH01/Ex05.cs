using System;

class Program
{
    static void DisplayMenu()
    {
        Console.WriteLine("""
        ===================================
        1. Nhập giá hai trị số thực cho x, y
        2. Tính x^y
        3. Tính căn bậc 2 của x và y
        4. Thoát
        ===================================
        """);
    }
    static void Main()
    {
        double x = 0; 
        double y = 0;
        int Ch;

        do
        {
        DisplayMenu();

        Console.WriteLine("Nhập lựa chọn của bạn từ 1 đến 4: ");
        Ch = int.Parse(Console.ReadLine());


        if (Ch == 1)
        {
            Console.WriteLine("Nhập x: ");
            x = double.Parse(Console.ReadLine());
            Console.WriteLine("Nhập y: ");
            y = double.Parse(Console.ReadLine());        
        } 
        else if (Ch == 2)
        {
            double result = Math.Pow(x,y);
            Console.WriteLine($"x^y = {result}");        
        } 
        else if (Ch == 3)
        {
            Console.WriteLine($"""
            Căn bậc 2 của x: {Math.Sqrt(x)}
            Căn bậc 2 của y: {Math.Sqrt(y1)}
            """);
        } 
        else if (Ch == 4)
        {
            Console.WriteLine("Thoát chương trình.");
        } 
        else
        {
            Console.WriteLine("Lựa chọn không hợp lệ, vui lòng nhập lại. ");         
        }

        } while (Ch != 4);
    }
}