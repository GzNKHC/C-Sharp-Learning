static void Bai3_1()
{
    int[] mangSo = { 50, 42, 12, 3, 9, 8, 1, 50, 3, 42, 85 };

    Console.WriteLine("Bài 3.1a - Đếm số phần tử:");

    int tongSo = mangSo.Count();
    int soChan = mangSo.Count(n => n % 2 == 0);
    int soLe = mangSo.Count(n => n % 2 != 0);

    Console.WriteLine($"Tổng số phần tử: {tongSo}");
    Console.WriteLine($"Số phần tử chẵn: {soChan}");
    Console.WriteLine($"Số phần tử lẻ: {soLe}");

    Console.WriteLine("\nBài 3.1b - Tổng, lớn nhất và nhỏ nhất:");

    Console.WriteLine($"Tổng các giá trị: {mangSo.Sum()}");
    Console.WriteLine($"Giá trị lớn nhất: {mangSo.Max()}");
    Console.WriteLine($"Giá trị nhỏ nhất: {mangSo.Min()}");

    Console.WriteLine("\nBài 3.1c - Đếm số giá trị khác nhau:");

    int soGiaTriKhacNhau = mangSo.Distinct().Count();

    Console.WriteLine($"Số giá trị khác nhau: {soGiaTriKhacNhau}");
    
    //Query Syntax
    var nhomQuery = from n in mangSo
                    group n by n % 5;

    Console.WriteLine("\nBài 3.1d - Query Syntax:");
    foreach (var nhom in nhomQuery)
    {
        Console.Write($"Dư {nhom.Key}: ");

        foreach (int n in nhom)
        {
            Console.Write($"{n} ");
        }

        Console.WriteLine();
    }
    //Method Syntax     
    var nhomMethod = mangSo.GroupBy(n => n % 5);

    Console.WriteLine("\nBài 3.1d - Method Syntax:");
    foreach (var nhom in nhomMethod)
    {
        Console.Write($"Dư {nhom.Key}: ");

        foreach (int n in nhom)
        {
            Console.Write($"{n} ");
        }

        Console.WriteLine();
    }
}