static void Bai5_1()
{
    List<MonHoc> danhSach = DuLieu.DS_Mon();

    // Câu a
    var queryA = from mon in danhSach
                 where mon.TenMon.StartsWith("Lập trình")
                 select mon.TenMon;

    var methodA = danhSach
        .Where(mon => mon.TenMon.StartsWith("Lập trình"))
        .Select(mon => mon.TenMon);

    Console.WriteLine("Bài 5.1a - Query Syntax:");
    foreach (string tenMon in queryA)
    {
        Console.WriteLine(tenMon);
    }

    Console.WriteLine("\nBài 5.1a - Method Syntax:");
    foreach (string tenMon in methodA)
    {
        Console.WriteLine(tenMon);
    }

    // Câu b
    var queryB = from mon in danhSach
                 where mon.He == "CD"
                 orderby mon.SoTiet descending, mon.MaMon ascending
                 select mon;

    var methodB = danhSach
        .Where(mon => mon.He == "CD")
        .OrderByDescending(mon => mon.SoTiet)
        .ThenBy(mon => mon.MaMon);

    Console.WriteLine("\nBài 5.1b - Query Syntax:");
    foreach (MonHoc mon in queryB)
    {
        Console.WriteLine(
            $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
        );
    }

    Console.WriteLine("\nBài 5.1b - Method Syntax:");
    foreach (MonHoc mon in methodB)
    {
        Console.WriteLine(
            $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
        );
    }

    // Câu c
    var queryC = from mon in danhSach
                 where mon.TenMon.Contains(
                     "web", StringComparison.OrdinalIgnoreCase
                 )
                 select new { mon.TenMon, mon.He };

    var methodC = danhSach
        .Where(mon => mon.TenMon.Contains(
            "web", StringComparison.OrdinalIgnoreCase
        ))
        .Select(mon => new { mon.TenMon, mon.He });

    Console.WriteLine("\nBài 5.1c - Query Syntax:");
    foreach (var mon in queryC)
    {
        Console.WriteLine($"{mon.TenMon,-45} | {mon.He}");
    }

    Console.WriteLine("\nBài 5.1c - Method Syntax:");
    foreach (var mon in methodC)
    {
        Console.WriteLine($"{mon.TenMon,-45} | {mon.He}");
    }

    // Câu d
    var queryD = from mon in danhSach
                 where mon.He == "KTV"
                 orderby mon.MaMon ascending
                 select mon;

    var methodD = danhSach
        .Where(mon => mon.He == "KTV")
        .OrderBy(mon => mon.MaMon);

    Console.WriteLine("\nBài 5.1d - Query Syntax:");
    foreach (MonHoc mon in queryD)
    {
        Console.WriteLine(
            $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
        );
    }

    Console.WriteLine("\nBài 5.1d - Method Syntax:");
    foreach (MonHoc mon in methodD)
    {
        Console.WriteLine(
            $"{mon.MaMon,-8} | {mon.TenMon,-45} | {mon.He,-5} | {mon.SoTiet,7}"
        );
    }
}