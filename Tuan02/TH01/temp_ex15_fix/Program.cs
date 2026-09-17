using System;
using System.Collections;

class NumList
{
    private ArrayList ls = new ArrayList();

    public void ArrayInput()
    {
        Console.WriteLine("Nhập số lượng phần tử của mảng: ");
        int n = int.Parse(Console.ReadLine());

        Console.WriteLine("Nhập một mảng số: ");
        ls.Clear();

        for (int i = 0; i < n; i++)
        {
            Console.Write($"Nhập phần tử thứ {i + 1}: ");
            int value = int.Parse(Console.ReadLine());
            ls.Add(value);
        }
    }

    public void FindMinMax()
    {
        if (ls.Count == 0)
        {
            Console.WriteLine("Mảng rỗng.");
            return;
        }

        int min = (int)ls[0];
        int max = (int)ls[0];

        for (int i = 1; i < ls.Count; i++)
        {
            int value = (int)ls[i];
            if (value < min) min = value;
            if (value > max) max = value;
        }

        Console.WriteLine($"""
        Min = {min}
        Max = {max}
        """);
    }

    public void DisplayArray()
    {
        if (ls.Count == 0)
        {
            Console.WriteLine("Mảng rỗng.");
            return;
        }

        Console.Write("Mảng của bạn là: ");
        foreach (int value in ls)
        {
            Console.Write(value + " ");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        NumList obj = new NumList();
        obj.ArrayInput();
        obj.DisplayArray();
        obj.FindMinMax();
    }
}