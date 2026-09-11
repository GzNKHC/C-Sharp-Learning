using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a 4-digit number: ");
        int n = int.Parse(Console.ReadLine());

        int a = n / 1000;
        int b = (n / 100) % 10;
        int c = (n / 10) % 10;
        int d = n % 10;

        // a. Check palindrome
        if (a == d && b == c)
        {
            Console.WriteLine($"{n} is a palindrome.");
        }
        else
        {
            Console.WriteLine($"{n} is not a palindrome.");
        }

        // Sort digits
        int[] digits = { a, b, c, d };
        Array.Sort(digits);

        // b. Check if there are 3 equal digits
        if ((digits[0] == digits[1] && digits[1] == digits[2]) ||
            (digits[1] == digits[2] && digits[2] == digits[3]))
        {
            Console.WriteLine($"{n} has 3 equal digits.");
        }
        else
        {
            Console.WriteLine($"{n} does not have 3 equal digits.");
        }

        // c. Check if all 4 digits are different
        if (digits[0] != digits[1] &&
            digits[1] != digits[2] &&
            digits[2] != digits[3])
        {
            Console.WriteLine($"{n} has 4 different digits.");
        }
        else
        {
            Console.WriteLine($"{n} does not have 4 different digits.");
        }
    }
}