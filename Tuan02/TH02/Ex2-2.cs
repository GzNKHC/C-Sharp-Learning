using System;
using System.Collections.Generic;

public class Person
{
    public string Name { get; set; }
    public bool IsAlive { get; set; }

    public Person() 
    { 
        Name = "";
        IsAlive = true;
    }

    public Person(Person other)
    {
        this.Name = other.Name;
        this.IsAlive = other.IsAlive;
    }

    public void Input()
    {
        Console.Write("Nhập tên: ");
        Name = Console.ReadLine();
        Console.Write("Còn sống không? (true/false): ");
        IsAlive = bool.Parse(Console.ReadLine());
    }

    public void Output()
    {
        string status = IsAlive ? "Còn sống" : "Đã mất";
        Console.WriteLine($"- Tên: {Name} | Trạng thái: {status}");
    }
}

public class PersonList
{
    private List<Person> people;

    public PersonList()
    {
        people = new List<Person>();
    }

    public PersonList(PersonList other)
    {
        people = new List<Person>();
        foreach (Person p in other.people)
        {
            this.people.Add(new Person(p));
        }
    }

    public void Add(Person x)
    {
        people.Add(x);
    }
    public void Input()
    {
        Console.Write("Nhập số lượng nhân khẩu cần thêm: ");
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\nNhập thông tin người thứ {i + 1}:");
                Person p = new Person();
                p.Input();
                this.Add(p);
            }
        }
    }

    public void Output()
    {
        if (people.Count == 0)
        {
            Console.WriteLine("Danh sách trống.");
            return;
        }

        foreach (Person p in people)
        {
            p.Output();
        }
    }

    public PersonList LivingPeople()
    {
        PersonList livingList = new PersonList();
        foreach (Person p in people)
        {
            if (p.IsAlive)
            {
                livingList.Add(new Person(p));
            }
        }
        return livingList;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        PersonList list = new PersonList();
        
        Console.WriteLine("--- NHẬP DANH SÁCH NHÂN KHẨU ---");
        list.Input();

        Console.WriteLine("\n--- DANH SÁCH TOÀN BỘ NHÂN KHẨU ---");
        list.Output();

        Console.WriteLine("\n--- DANH SÁCH NHỮNG NGƯỜI CÒN SỐNG ---");
        PersonList living = list.LivingPeople();
        living.Output();
        
        Console.ReadLine();
    }
}