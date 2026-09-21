using System;

class Person
{
    //Field
    private int id;
    private string name;
    private int yob;
    private int yod;

    //Property
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Yob
    {
        get { return yob; }
        set { yob = value; }
    }

    public int Yod
    {
        get { return yod; }
        set { yod = value; }
    }

    //Default Constructor
    public Person()
    {
        id = 0;
        name = "";
        yob = 0;
        yod = 0;
    }

    //Copy Constructor
    public Person(Person p)
    {
        id = p.id;
        name = p.name;
        yob = p.yob;
        yod = p.yod;
    }

    //Input
    public void Input()
    {
        Console.Write("Nhap ID: ");
        id = int.Parse(Console.ReadLine());

        Console.Write("Nhap ho ten: ");
        name = Console.ReadLine();

        Console.Write("Nhap nam sinh: ");
        yob = int.Parse(Console.ReadLine());

        Console.Write("Nhap nam mat (0 neu con song): ");
        yod = int.Parse(Console.ReadLine());
    }

    //Output
    public void Output()
    {
        Console.WriteLine($"""
        ID: {id}
        Ho ten: {name}
        Nam sinh: {yob}
        Nam mat: {yod}
        Con song: {IsLiving()}
        """);
    }

    //Method
    public bool IsLiving()
    {
        return yod == 0;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Person p1 = new Person();

        Console.WriteLine("Nhap thong tin:");
        p1.Input();

        Console.WriteLine("\nThong tin Person:");
        p1.Output();

        //Test Copy
        Person p2 = new Person(p1);

        Console.WriteLine("\nThong tin Person sao chep:");
        p2.Output();
    }
}