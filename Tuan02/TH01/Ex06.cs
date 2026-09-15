using System;

class Processing
{
    public int maxNumber(int a, int b, int c)
    {
        int max = a;
        if(b > max) { max = b; };
        if(c > max) { max = c; };
        
        return max;
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("Nhập 3 số nguyên: ");
        String[] input = Console.ReadLine().Split();

        int a = int.Parse(input[0]);
        int b = int.Parse(input[1]);
        int c = int.Parse(input[2]);

        Processing maxNum = new Processing();
        int max = maxNum.maxNumber(a, b, c);  

        Console.WriteLine($"Giá trị lớn nhất là: {max}");
    }
}