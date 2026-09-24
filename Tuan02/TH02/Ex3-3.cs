using System;

class Program
{
    public delegate int SoSanh<T>(T a, T b);

    public static void Sort<T>(T[] arr, SoSanh<T> compare)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = i + 1; j < arr.Length; j++)
            {
                if (compare(arr[i], arr[j]) > 0)
                {
                    T temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
        }
    }

    public static int SoSanhTang(int a, int b)
    {
        return a.CompareTo(b);
    }

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
        Console.WriteLine("\nMảng ban đầu:");
        foreach (int x in arr)
        {
            Console.Write(x + " ");
        }

        Sort(arr, SoSanhTang);
        Console.WriteLine("\n\nMảng sau khi sắp xếp tăng dần:");
        foreach (int x in arr)
        {
            Console.Write(x + " ");
        }
    }
}