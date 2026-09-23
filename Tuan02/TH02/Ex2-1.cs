using System;
using System.Collections;

class Point
{
    public double X;
    public double Y;

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public void Display()
    {
        Console.WriteLine($"({X}, {Y})");
    }
}

class ArrayPoint
{
    private ArrayList points = new ArrayList();

    public void Add(Point p)
    {
        points.Add(p);
    }

    public Point this[int i]
    {
        get
        {
            return (Point)points[i];
        }

        set
        {
            points[i] = value;
        }
    }
}

class Program
{
    static void Main()
    {
        ArrayPoint arr = new ArrayPoint();

        arr.Add(new Point(1, 2));
        arr.Add(new Point(3, 4));
        arr.Add(new Point(5, 6));

        arr[0].Display();
        arr[1].Display();
        arr[2].Display();
    }
}