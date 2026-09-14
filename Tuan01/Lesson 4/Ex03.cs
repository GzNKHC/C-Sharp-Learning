using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter day, month, year: ");
        string[] input = Console.ReadLine().Split();

        int day = int.Parse(input[0]);
        int month = int.Parse(input[1]);
        int year = int.Parse(input[2]);

        int daysInMonth;

        // Determine number of days in current month
        if (month == 1 || month == 3 || month == 5 ||
            month == 7 || month == 8 || month == 10 || month == 12)
        {
            daysInMonth = 31;
        }
        else if (month == 4 || month == 6 || month == 9 || month == 11)
        {
            daysInMonth = 30;
        }
        else
        {
            // February
            if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))
            {
                daysInMonth = 29;
            }
            else
            {
                daysInMonth = 28;
            }
        }
        day++;
        if (day > daysInMonth)
        {
            day = 1;
            month++;

            if (month > 12)
            {
                month = 1;
                year++;
            }
        }
        Console.WriteLine($"The next day is {day}/{month}/{year}.");
    }
}