using System;
using System.ComponentModel.DataAnnotations;

class NumberFinder
{
    public static void FindMinMax(double a, double b, double c, out double min, out double max)
    {
        //Find min
        min = a;
        if (b < min)
        {
            min = b;
        } else if (c < min)
        {
            min = c;
        }

        //Find max
        max = a;
        if (b > max)
        {
            max = b;
        } else if (c > max)
        {
            max = c;
        } 
        
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Nhập a:");
        double a = double.Parse(Console.ReadLine());

        Console.WriteLine("Nhập b:");
        double b = double.Parse(Console.ReadLine());

        Console.WriteLine("Nhập c:");
        double c = double.Parse(Console.ReadLine());

        NumberFinder.FindMinMax(a, b, c, out double min, out double max);

        Console.WriteLine($"""
        Min = {min}
        Max = {max}
        """);
    }
}