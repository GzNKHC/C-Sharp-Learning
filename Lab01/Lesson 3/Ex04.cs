using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter month (1 - 12): ");
        int M = int.Parse(Console.ReadLine());

        switch (M)
        {
            case 1:
            Console.WriteLine($"Month {M} is: January");
            break;

            case 2:
            Console.WriteLine($"Month {M} is: February");
            break;

            case 3:
            Console.WriteLine($"Month {M} is: March");
            break;

            case 4:
            Console.WriteLine($"Month {M} is: April");
            break;

            case 5:
            Console.WriteLine($"Month {M} is: May");
            break;

            case 6:
            Console.WriteLine($"Month {M} is: June");
            break;

            case 7:
            Console.WriteLine($"Month {M} is: July");
            break;

            case 8:
            Console.WriteLine($"Month {M} is: August");
            break;

            case 9:
            Console.WriteLine($"Month {M} is: September");
            break;

            case 10:
            Console.WriteLine($"Month {M} is: October");
            break;

            case 11:
            Console.WriteLine($"Month {M} is: November");
            break;

            case 12:
            Console.WriteLine($"Month {M} is: Devember");
            break;
        }
    }
}