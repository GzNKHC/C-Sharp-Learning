using System;

interface ISoSanh<T>
{
    int Compare(T a, T b);
}

class SoSanhSoNguyen : ISoSanh<int>
{
    public int Compare(int a, int b)
    {
        return a.CompareTo(b);
    }
}

class SapXep
{
    public static void Sort<T>(T[] arr, ISoSanh<T> comparer)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = i + 1; j < arr.Length; j++)
            {
                if (comparer.Compare(arr[i], arr[j]) > 0)
                {
                    T temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Nhập số lượng phần tử: ");
        int n = int.Parse(Console.ReadLine());

        int[] arr = new int[n];

        Console.WriteLine("Nhập các phần tử:");

        for (int i = 0; i < n; i++)
        {
            Console.Write($"arr[{i}] = ");
            arr[i] = int.Parse(Console.ReadLine());
        }

        Console.WriteLine("\nMảng trước khi sắp xếp:");
        foreach (int x in arr)
        {
            Console.Write(x + " ");
        }

        SoSanhSoNguyen comparer = new SoSanhSoNguyen();
        SapXep.Sort(arr, comparer);
        Console.WriteLine("\n\nMảng sau khi sắp xếp:");
        foreach (int x in arr)
        {
            Console.Write(x + " ");
        }
    }
}