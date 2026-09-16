using System;

class PrimeChecker
{
    public bool IsPrime(int n)
    {
        if (n < 2)
        {
            return false;
        }

        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0)
            {
                return false;
            }
        }

        return true;
    }
}

class Program
{
    static void Main()
    {
        int n;
        Console.WriteLine("Nhập số nguyên n: ");
        n = int.Parse(Console.ReadLine());

        PrimeChecker pc = new PrimeChecker();
        bool isPrime = pc.IsPrime(n);

        if (isPrime)
        {
            Console.WriteLine($"n = {n} là số nguyên tố.");
        }
        else
        {
            Console.WriteLine($"n = {n} không phải số nguyên tố.");
        }
    }
}