using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter a, b, c, d, e, f: ");
        String[] input = Console.ReadLine().Split();

        int a = int.Parse(input[0]);
        int b = int.Parse(input[1]);
        int c = int.Parse(input[2]);
        int d = int.Parse(input[3]);
        int e = int.Parse(input[4]);
        int f = int.Parse(input[5]);


        double D = a*e - d*b;
        double Dx = b*f - e*c;
        double Dy = a*f - d*c;

        if (D == 0)
        {
            if (Dx == 0 && Dy == 0)
            {
                Console.WriteLine("The system has infinitely many solutions");
            } else
            {
                Console.WriteLine("The system has no solution");
            }
        } else
        {
            Console.WriteLine($"""
            The system has a unique solution
            x = {Dx/D}
            y = {Dy/D}
            """);
        }
    }
}