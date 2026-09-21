using System;

class DonThuc
{
    //Field
    private double a;
    private int n;

    //Property
    public double A
    {
        get { return a; }
        set { a = value; }
    }

    public int N
    {
        get { return n; }
        set
        {
            if (value >= 0)
                n = value;
            else
                n = 0;
        }
    }

    //Constructor mặc định
    public DonThuc()
    {
        a = 0;
        n = 0;
    }

    //Constructor có tham số
    public DonThuc(double a, int n)
    {
        this.a = a;

        if (n >= 0)
            this.n = n;
        else
            this.n = 0;
    }

    //Giá trị đơn thức tại x
    public double TinhGiaTri(double x)
    {
        return a * Math.Pow(x, n);
    }

    //Đạo hàm
    public DonThuc DaoHam()
    {
        if (n == 0)
            return new DonThuc(0, 0);

        return new DonThuc(a * n, n - 1);
    }

    //Xuất đơn thức
    public override string ToString()
    {
        if (n == 0)
            return $"{a}";

        if (n == 1)
            return $"{a}x";

        return $"{a}x^{n}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Nhap he so a: ");
        double a = double.Parse(Console.ReadLine());

        Console.Write("Nhap so mu n: ");
        int n = int.Parse(Console.ReadLine());

        DonThuc p = new DonThuc(a, n);

        Console.Write("Nhap x: ");
        double x = double.Parse(Console.ReadLine());

        Console.WriteLine($"\nP(x) = {p}");
        Console.WriteLine($"P({x}) = {p.TinhGiaTri(x)}");

        DonThuc q = p.DaoHam();

        Console.WriteLine($"P'(x) = {q}");
    }
}