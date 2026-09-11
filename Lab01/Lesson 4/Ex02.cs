// Water receipt

using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter last month's water meter reading (m^3): ");
        double oldIndex = double.Parse(Console.ReadLine());

        Console.Write("Enter this month's water meter reading (m^3): ");
        double newIndex = double.Parse(Console.ReadLine());

        Console.Write("Enter number of people: ");
        int people = int.Parse(Console.ReadLine());

        double waterUsed = newIndex - oldIndex;

        double L1 = 4 * people;
        double L2 = 2 * people;

        double price;

        if (waterUsed <= L1)
        {
            price = waterUsed * 4400;
        }
        else if (waterUsed <= L1 + L2)
        {
            price = L1 * 4400
                  + (waterUsed - L1) * 8300;
        }
        else
        {
            price = L1 * 4400
                  + L2 * 8300
                  + (waterUsed - L1 - L2) * 10500;
        }

        double total = price * 1.15;

        Console.WriteLine(
            $"The total amount for {waterUsed} m^3 of water consumed this month is {total:N0} VND."
        );
    }
}