// Find min max in an array

using System;

class findMinMaxArray
{
    static void bubbleSort(int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = 0; j < arr.Length - 1 - i; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    int tmp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = tmp;
                } 
            }
        }
    }    

    static void Main()
    {
        Console.WriteLine("Enter a, b, c, d, e: ");

        String[] input = Console.ReadLine().Split();
        int[] a = new int[5];
        
        for(int i = 0; i < 5; i++)
        {
            a[i] = int.Parse(input[i]);
        }

        bubbleSort(a);

        Console.WriteLine($"""
        Max: {a[4]}
        Min: {a[0]}
        """);
    }
}