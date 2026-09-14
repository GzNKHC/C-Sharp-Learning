using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter a: ");
        double a = double.Parse(Console.ReadLine());

        Console.WriteLine("Enter b: ");
        double b = double.Parse(Console.ReadLine());

        Console.WriteLine("Enter c: ");
        double c = double.Parse(Console.ReadLine());


        //check
        if(a == 0)
        {
                Console.WriteLine("Not a equadratic equation");
        } else
        {
            double delta = b * b - 4*a*c;
            if(delta < 0)
            {
                Console.WriteLine("No real solution");
            } else if (delta == 0)
            {
                Console.WriteLine($"The equation has a double root: x = {(-(double)b / (2 * a)):F2}");
            } else
            {
                Console.WriteLine($"""
                The equation has 2 distinct roots:
                x1 = {((double)(-b + Math.Sqrt(delta)) / (2 * a)):F2}
                x2 = {((double)(-b - Math.Sqrt(delta)) / (2 * a)):F2}
                """);
            }
        }
    }
}