using System;

class PhanSo
{
    //Field
    private int tu;
    private int mau;

    //Property
    public int Tu
    {
        get { return tu; }
        set { tu = value; }
    }

    public int Mau
    {
        get { return mau; }
        set
        {
            if (value != 0)
                mau = value;
            else
                mau = 1;
        }
    }

    //Constructor mặc định
    public PhanSo()
    {
        tu = 0;
        mau = 1;
    }

    //Constructor có tham số
    public PhanSo(int tu, int mau)
    {
        this.tu = tu;

        if (mau != 0)
            this.mau = mau;
        else
            this.mau = 1;
    }

    //Constructor chỉ có tử
    public PhanSo(int tu)
    {
        this.tu = tu;
        this.mau = 1;
    }

    //Copy Constructor
    public PhanSo(PhanSo ps)
    {
        this.tu = ps.tu;
        this.mau = ps.mau;
    }

    //Override ToString
    public override string ToString()
    {
        return $"{tu}/{mau}";
    }

    //Toán tử 1 ngôi 

    public static PhanSo operator +(PhanSo a)
    {
        return new PhanSo(a.tu, a.mau);
    }

    public static PhanSo operator -(PhanSo a)
    {
        return new PhanSo(-a.tu, a.mau);
    }

    //Toán tử 2 ngôi
    public static PhanSo operator +(PhanSo a, PhanSo b)
    {
        return new PhanSo(
            a.tu * b.mau + b.tu * a.mau,
            a.mau * b.mau
        );
    }

    public static PhanSo operator -(PhanSo a, PhanSo b)
    {
        return new PhanSo(
            a.tu * b.mau - b.tu * a.mau,
            a.mau * b.mau
        );
    }

    public static PhanSo operator *(PhanSo a, PhanSo b)
    {
        return new PhanSo(
            a.tu * b.tu,
            a.mau * b.mau
        );
    }

    public static PhanSo operator /(PhanSo a, PhanSo b)
    {
        if (b.tu == 0)
            throw new DivideByZeroException("Khong the chia cho phan so 0!");

        return new PhanSo(
            a.tu * b.mau,
            a.mau * b.tu
        );
    }

    //Toán tử so sánh
    public static bool operator >(PhanSo a, PhanSo b)
    {
        return a.tu * b.mau > b.tu * a.mau;
    }

    public static bool operator <(PhanSo a, PhanSo b)
    {
        return a.tu * b.mau < b.tu * a.mau;
    }

    public static bool operator >=(PhanSo a, PhanSo b)
    {
        return a.tu * b.mau >= b.tu * a.mau;
    }

    public static bool operator <=(PhanSo a, PhanSo b)
    {
        return a.tu * b.mau <= b.tu * a.mau;
    }

    public static bool operator ==(PhanSo a, PhanSo b)
    {
        return a.tu * b.mau == b.tu * a.mau;
    }

    public static bool operator !=(PhanSo a, PhanSo b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is PhanSo ps)
            return this == ps;

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(tu, mau);
    }
}

class Program
{
    static void Main(string[] args)
    {
        PhanSo a = new PhanSo(1, 2);
        PhanSo b = new PhanSo(3, 4);

        Console.WriteLine($"a = {a}");
        Console.WriteLine($"b = {b}");

        Console.WriteLine($"a + b = {a + b}");
        Console.WriteLine($"a - b = {a - b}");
        Console.WriteLine($"a * b = {a * b}");
        Console.WriteLine($"a / b = {a / b}");

        Console.WriteLine($"+a = {+a}");
        Console.WriteLine($"-a = {-a}");

        Console.WriteLine($"a > b: {a > b}");
        Console.WriteLine($"a < b: {a < b}");
        Console.WriteLine($"a >= b: {a >= b}");
        Console.WriteLine($"a <= b: {a <= b}");
        Console.WriteLine($"a == b: {a == b}");
        Console.WriteLine($"a != b: {a != b}");
    }
}