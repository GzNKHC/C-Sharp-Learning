using System;

class Point
{
    //Field
    private double x;
    private double y;

    //Property
    public double X
    {
        get { return x; }
        set { x = value; }
    }

    public double Y
    {
        get { return y; }
        set { y = value; }
    }

    //Default Constructor
    public Point()
    {
        x = 0;
        y = 0;
    }

    //Constructor có tham số
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    //Input
    public void Input()
    {
        Console.Write("Nhap x: ");
        x = double.Parse(Console.ReadLine());

        Console.Write("Nhap y: ");
        y = double.Parse(Console.ReadLine());
    }

    //Output
    public void Output()
    {
        Console.WriteLine($"({x}, {y})");
    }

    //Override ToString
    public override string ToString()
    {
        return $"({x}, {y})";
    }

    //Toán tử +
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }

    //Toán tử -
    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y);
    }

    //Toán tử lấy âm
    public static Point operator -(Point a)
    {
        return new Point(-a.x, -a.y);
    }

    //(a) Khoảng cách giữa 2 điểm

    //Cách 1: Phương thức thành viên
    public double KhoangCach(Point b)
    {
        return Math.Sqrt(
            Math.Pow(x - b.x, 2)
            + Math.Pow(y - b.y, 2)
        );
    }

    //Cách 2: Phương thức tĩnh
    public static double KhoangCach(Point a, Point b)
    {
        return Math.Sqrt(
            Math.Pow(a.x - b.x, 2)
            + Math.Pow(a.y - b.y, 2)
        );
    }
    //(b) Trung điểm của 2 điểm
    //Cách 1: Phương thức thành viên
    public Point TrungDiem(Point b)
    {
        return new Point(
            (x + b.x) / 2,
            (y + b.y) / 2
        );
    }

    //Cách 2: Phương thức tĩnh
    public static Point TrungDiem(Point a, Point b)
    {
        return new Point(
            (a.x + b.x) / 2,
            (a.y + b.y) / 2
        );
    }
}

class Program
{
    static void Main(string[] args)
    {
        Point A = new Point();
        Point B = new Point();

        Console.WriteLine("Nhap diem A:");
        A.Input();

        Console.WriteLine("\nNhap diem B:");
        B.Input();

        Console.WriteLine("\n--- Hai diem ---");
        Console.WriteLine($"A = {A}");
        Console.WriteLine($"B = {B}");
        Console.WriteLine("\n--- Phep toan ---");
        Console.WriteLine($"A + B = {A + B}");
        Console.WriteLine($"A - B = {A - B}");
        Console.WriteLine($"-A = {-A}");

        //Khoảng cách
        Console.WriteLine("\n--- Khoang cach ---");

        Console.WriteLine(
            $"Phuong thuc thanh vien: {A.KhoangCach(B)}"
        );

        Console.WriteLine(
            $"Phuong thuc tinh: {Point.KhoangCach(A, B)}"
        );

        //Trung điểm
        Console.WriteLine("\n--- Trung diem ---");

        Point I1 = A.TrungDiem(B);
        Console.WriteLine(
            $"Phuong thuc thanh vien: I = {I1}"
        );

        Point I2 = Point.TrungDiem(A, B);
        Console.WriteLine(
            $"Phuong thuc tinh: I = {I2}"
        );
    }
}