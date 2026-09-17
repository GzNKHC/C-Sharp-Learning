using System;

class Program
{
    static bool StrSymmetryCheck(string str)
    {
        int left = 0;
        int right = str.Length - 1;
        while (left < right)
        {
            if(str[left] != str[right])
            {
                return false;
            }
            left++;
            right--;
        }
        return true;
    }

    static void Main()
    {
        Console.WriteLine("Nhập một chuỗi: ");
        String str = Console.ReadLine();

        if (StrSymmetryCheck(str))
        {
            Console.WriteLine("Chuỗi đối xứng.");
        } else
        {
            Console.WriteLine("Chuỗi không đối xứng.");
        }
    }
}