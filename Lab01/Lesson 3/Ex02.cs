using System;

class funtionCalc
{
    static double f1(double x)
    {
        if(x <= 0)
        {
            return 0;
        } else if (x <= 1)
        {
            return x;
        }     else
        {
            return Math.Pow(x,4);
        }
    }

    static double f2(double x)
    {
        if (x <=2)
        {
            return Math.Pow(x,2) + 4*x + 5;
        } else
        {
            return 1/(Math.Pow(x,2) + 4*x + 5);
        }
    }

    static void Main()
    {
        Console.WriteLine("Enter x: ");
        double x = double.Parse(Console.ReadLine());

        Console.WriteLine($"""
        f1({x}) = {f1(x)}
        f2({x}) = {f2(x)}
        """);
    }
}