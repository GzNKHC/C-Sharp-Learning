static void Bai3_2()
{
    string[] monAn =
    {
        "Bún bò Huế", "Hủ tiếu heo", "Bánh canh", "Bánh mì",
        "Nước Cà phê", "Mì quảng", "Cơm tấm", "Nước Chanh dây",
        "Mì xào", "Bún riêu", "Bánh cuốn", "Mì gói",
        "Bún chả", "Hủ tiếu Nam vang"
    };

    int nganNhat = monAn.Min(mon => mon.Length);
    int daiNhat = monAn.Max(mon => mon.Length);

    //Query Syntax
    var monNganQuery = from mon in monAn
                       where mon.Length == nganNhat
                       select mon;

    var monDaiQuery = from mon in monAn
                      where mon.Length == daiNhat
                      select mon;

    Console.WriteLine("Bài 3.2a - Query Syntax:");

    Console.WriteLine($"Tên ngắn nhất ({nganNhat} ký tự):");
    foreach (string mon in monNganQuery)
    {
        Console.WriteLine(mon);
    }

    Console.WriteLine($"Tên dài nhất ({daiNhat} ký tự):");
    foreach (string mon in monDaiQuery)
    {
        Console.WriteLine(mon);
    }

    //Method Syntax
    var monNganMethod = monAn.Where(mon => mon.Length == nganNhat);
    var monDaiMethod = monAn.Where(mon => mon.Length == daiNhat);

    Console.WriteLine("\nBài 3.2a - Method Syntax:");

    Console.WriteLine($"Tên ngắn nhất ({nganNhat} ký tự):");
    foreach (string mon in monNganMethod)
    {
        Console.WriteLine(mon);
    }

    Console.WriteLine($"Tên dài nhất ({daiNhat} ký tự):");
    foreach (string mon in monDaiMethod)
    {
        Console.WriteLine(mon);
    }

    //Query Syntax
    var nhomQuery = from mon in monAn
                    group mon by mon.Split(' ')[0];

    Console.WriteLine("\nBài 3.2b - Query Syntax:");

    foreach (var nhom in nhomQuery)
    {
        Console.WriteLine($"Nhóm {nhom.Key}:");

        foreach (string mon in nhom)
        {
            Console.WriteLine($"  {mon}");
        }
    }

    //Method Syntax
    var nhomMethod = monAn.GroupBy(mon => mon.Split(' ')[0]);

    Console.WriteLine("\nBài 3.2b - Method Syntax:");

    foreach (var nhom in nhomMethod)
    {
        Console.WriteLine($"Nhóm {nhom.Key}:");

        foreach (string mon in nhom)
        {
            Console.WriteLine($"  {mon}");
        }
    }

    //Query Syntax and Count()
    int soMonBanhQuery = (from mon in monAn
                          where mon.Split(' ')[0] == "Bánh"
                          select mon).Count();

    //Method Syntax
    int soMonBanhMethod = monAn.Count(
        mon => mon.Split(' ')[0] == "Bánh"
    );

    Console.WriteLine("\nBài 3.2c - Số món có từ đầu tiên là Bánh:");
    Console.WriteLine($"Query Syntax + Count(): {soMonBanhQuery}");
    Console.WriteLine($"Method Syntax: {soMonBanhMethod}");
}