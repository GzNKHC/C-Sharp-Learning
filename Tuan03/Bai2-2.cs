//Bài 2.2 Truy vấn mảng chuỗi

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace BaiThucHanhLINQ;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Bai2_2();
    }

    static void Bai2_2()
    {
        string[] mangChuoi = { "đầu", "lòng", "hai", "ả", "tố", "nga",
            "Thúy", "Kiều", "là", "chị", "em", "là", "Thúy", "Vân" };
        Console.WriteLine("Bài 2.2a - Liệt kê các phần tử có 4 ký tự và sắp xếp tăng dần theo ký tự đầu tiên:");
        //Query Syntax
        IEnumerable<string> Query_kq =  from w in mangChuoi
                        where w.Length == 4
                        orderby w[0] ascending
                        select w;
        
        Console.WriteLine("Query Syntax:");
        foreach (string w in Query_kq)
        {
            Console.WriteLine($"{w}");
        }
        Console.WriteLine();

        //Method Syntax
        IEnumerable<string> Method_kq = mangChuoi
            .Where(w => w.Length == 4)
            .OrderBy(w => w[0]);
        
        Console.WriteLine("Method Syntax:");
        foreach (string w in Method_kq)
        {
            Console.WriteLine($"{w}");
        }
        Console.WriteLine();
        //------------------------

        Console.WriteLine("Bài 2.2b - Biến đổi mỗi phần tử thành dạng: <chữ thường> - <CHỮ HOA>:");
        //Query Syntax
        Query_kq =  from w in mangChuoi
                    select $"{w.ToLower()} - {w.ToUpper()}";
        
        Console.WriteLine("Query Syntax:");
        foreach (string w in Query_kq)
        {
            Console.WriteLine(w);
        }

        //Method Syntax
        Method_kq = mangChuoi.Select(
            w => $"{w.ToLower()} - {w.ToUpper()}"
        );
            
        
        Console.WriteLine("\nMethod Syntax:");
        foreach (string w in Method_kq)
        {
            Console.WriteLine($"{w}");
        }
        //------------------------

        Console.WriteLine("\nBài 2.2c - Liệt kê các phần tử có chứa ký tự u:");
        //Query Syntax
        Query_kq =  from w in mangChuoi
                    where w.Contains("u")
                    select w;
        
        Console.WriteLine("Query Syntax:");
        foreach (string w in Query_kq)
        {
            Console.WriteLine(w);
        }

        //Method Syntax
        Method_kq = mangChuoi.Where(w => w.Contains("u"));
            
        
        Console.WriteLine("\nMethod Syntax:");
        foreach (string w in Method_kq)
        {
            Console.WriteLine($"{w}");
        }
        //------------------------

        Console.WriteLine("\nBài 2.1d Liệt kê các từ “Thúy Kiều Thúy Vân” bằng cách chọn các phần tử bắt đầu bằng chữ in hoa:");
        //Query Syntax
        Query_kq =  from w in mangChuoi
                    where char.IsUpper(w[0])
                    select w;
        
        Console.WriteLine("Query Syntax:");
        foreach (string w in Query_kq)
        {
            Console.WriteLine(w);
        }

        //Method Syntax
        Method_kq = mangChuoi.Where(w => char.IsUpper(w[0]));
        
        Console.WriteLine("\nMethod Syntax:");
        foreach (string w in Method_kq)
        {
            Console.WriteLine($"{w}");
        }
        //------------------------
    }
}