using System;

class ConsoleMenu
{
    public delegate void ChooseHandler(int choice);

    public event ChooseHandler Choose;

    public void ShowMenu()
    {
        int choice;

        do
        {
            Console.WriteLine("\n=====MENU=====");
            Console.WriteLine("1. Nhập hệ số phương trình");
            Console.WriteLine("2. Giải phương trình");
            Console.WriteLine("0. Thoát chương trình");

            Console.Write("Thực hiện: ");
            choice = int.Parse(Console.ReadLine());

            if (choice != 0)
            {
                Choose?.Invoke(choice);
            }

        } while (choice != 0);

        Console.WriteLine("Thoát chương trình");
    }
}

class PTBac2Console : ConsoleMenu
{
    private double a;
    private double b;
    private double c;

    public void XuLyLuaChon(int choice)
    {
        switch (choice)
        {
            case 1:
                Nhap();
                break;

            case 2:
                Giai();
                break;

            default:
                Console.WriteLine("Chức năng không hợp lệ");
                break;
        }
    }

    private void Nhap()
    {
        Console.Write("Nhập a: ");
        a = double.Parse(Console.ReadLine());

        Console.Write("Nhập b: ");
        b = double.Parse(Console.ReadLine());

        Console.Write("Nhập c: ");
        c = double.Parse(Console.ReadLine());
    }

    private void Giai()
    {
        if (a == 0)
        {
            if (b == 0)
            {
                if (c == 0)
                    Console.WriteLine("Phương trình vô số nghiệm");
                else
                    Console.WriteLine("Phương trình vô nghiệm");
            }
            else
            {
                Console.WriteLine($"x = {-c / b}");
            }

            return;
        }

        double delta = b * b - 4 * a * c;

        if (delta < 0)
        {
            Console.WriteLine("Phương trình vô nghiệm");
        }
        else if (delta == 0)
        {
            double x = -b / (2 * a);

            Console.WriteLine($"Phương trình có nghiệm kép x = {x}");
        }
        else
        {
            double x1 = (-b + Math.Sqrt(delta)) / (2 * a);
            double x2 = (-b - Math.Sqrt(delta)) / (2 * a);

            Console.WriteLine($"x1 = {x1}");
            Console.WriteLine($"x2 = {x2}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        PTBac2Console app = new PTBac2Console();

        app.Choose += app.XuLyLuaChon;

        app.ShowMenu();
    }
}