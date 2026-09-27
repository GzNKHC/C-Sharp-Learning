//Bài 2.1 Truy vấn mảng số nguyên

using System;
using System.Linq;
using System.Text;

namespace BaiThucHanhLINQ;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Bai2_1();
    }

    static void Bai2_1()
    {
        int[] NumArray = { 50, 42, 16, 3, 9, 8, 12, 7, 24, 0 };
        Console.WriteLine("//Bài 2.1a - Các số chia hết cho cả 4 và 3:");
        //Query syntax
        var Query_kq =    from n in NumArray
                    where n % 4 == 0 && n % 3 == 0
                    select n;
        
        Console.WriteLine("Query syntax:");
        foreach (int n in Query_kq)
        {
            Console.Write($"{n} ");
        }
        Console.WriteLine();

        //Method syntax
        var Method_kq = NumArray.Where(
            n => n % 4 == 0 && n % 3 == 0
        );

        Console.WriteLine("Method syntax:");
        foreach (int n in Method_kq)
        {
            Console.Write($"{n} ");
        }
        Console.WriteLine();
    }
}
