using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a, b, c: ");
        string[] input = Console.ReadLine().Split();

        double a = double.Parse(input[0]);
        double b = double.Parse(input[1]);
        double c = double.Parse(input[2]);
        double[] sides = { a, b, c };
        Array.Sort(sides);

        a = sides[0];
        b = sides[1];
        c = sides[2];

        if (a <= 0 || a + b <= c)
        {
            Console.WriteLine("These sides do not form a triangle.");
        }
        else
        {
            bool isEquilateral = a == b && b == c;
            bool isIsosceles = a == b || b == c || a == c;
            bool isRight = a * a + b * b == c * c;

            if (isEquilateral)
            {
                Console.WriteLine("This is an equilateral triangle.");
            }
            else if (isRight && isIsosceles)
            {
                Console.WriteLine("This is a right isosceles triangle.");
            }
            else if (isRight)
            {
                Console.WriteLine("This is a right triangle.");
            }
            else if (isIsosceles)
            {
                Console.WriteLine("This is an isosceles triangle.");
            }
            else
            {
                Console.WriteLine("This is a scalene triangle.");
            }
        }
    }
}