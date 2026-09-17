using System;

class Program
{
    static string StringReverse(string str)
    {
        char[] ch = str.ToCharArray();

        int left = 0;
        int right = ch.Length - 1;

        while(left < right)
        {
            char tmp = ch[left];
            ch[left] = ch[right];
            ch[right] = tmp;
            left++;
            right--;
        }
        return new string(ch);
    }

    static void Main()
    {
        Console.WriteLine("Nhập một chuỗi");
        string str = Console.ReadLine();

        Console.WriteLine(StringReverse(str));
    }
}