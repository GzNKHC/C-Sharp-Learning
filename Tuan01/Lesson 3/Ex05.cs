using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter month: ");
        int M = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter year: ");
        int Y = int.Parse(Console.ReadLine());

        if(M==1 || M==3 || M==5 || M==7 || M==8 || M==10 || M==12)
        {
            Console.WriteLine($"Month {M} in {Y} has: 31 days");
        } else if(M == 2)
        {
            if (Y % 400 == 0)
            {
                Console.WriteLine($"Month {M} in {Y} has: 29 days");
            } else
            {
                Console.WriteLine($"Month {M} in {Y} has: 28 days");
            }
        } else
        {
            Console.WriteLine($"Month {M} in {Y} has: 30 days");
        }
    }
}