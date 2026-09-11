# Lab 01

## Lesson 3

---

### Exercise 1

#### Code

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter a, b, c, d, e, f: ");
        String[] input = Console.ReadLine().Split();

        int a = int.Parse(input[0]);
        int b = int.Parse(input[1]);
        int c = int.Parse(input[2]);
        int d = int.Parse(input[3]);
        int e = int.Parse(input[4]);
        int f = int.Parse(input[5]);


        double D = a*e - d*b;
        double Dx = b*f - e*c;
        double Dy = a*f - d*c;

        if (D == 0)
        {
            if (Dx == 0 && Dy == 0)
            {
                Console.WriteLine("The system has infinitely many solutions");
            } else
            {
                Console.WriteLine("The system has no solution");
            }
        } else
        {
            Console.WriteLine($"""
            The system has a unique solution
            x = {Dx/D}
            y = {Dy/D}
            """);
        }
    }
}
```

#### Sample Input

```text
7 -3 19 5 4 -2
```

#### Sample Output

```text
x = -1.627906976744186
y = -2.5348837209302326
```

---

### Exercise 2 - Water receipt

#### Code

```csharp
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
```

#### Sample Input

```text
100
120
3
```

#### Sample Output

```text
The total amount for 20 m^3 of water consumed this month is 142,140 VND.
```

---

### Exercise 3 - The day after

#### Code

```csharp
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
```

#### Sample Input

```text
31 1 2015
```

#### Sample Output

```text
The next day is 1/2/2015
```

---

### Exercise 4 - Classify triangle

#### Code

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a, b, c: ");
        string[] input = Console.ReadLine().Split();

        double a = double.Parse(input[0]);
        double b = double.Parse(input[1]);
        double c = double.Parse(input[2]);
        double[] sides = { a, b, c };
        Array.Sort(sides);

        a = sides[0];
        b = sides[1];
        c = sides[2];

        if (a <= 0 || a + b <= c)
        {
            Console.WriteLine("These sides do not form a triangle.");
        }
        else
        {
            bool isEquilateral = a == b && b == c;
            bool isIsosceles = a == b || b == c || a == c;
            bool isRight = a * a + b * b == c * c;

            if (isEquilateral)
            {
                Console.WriteLine("This is an equilateral triangle.");
            }
            else if (isRight && isIsosceles)
            {
                Console.WriteLine("This is a right isosceles triangle.");
            }
            else if (isRight)
            {
                Console.WriteLine("This is a right triangle.");
            }
            else if (isIsosceles)
            {
                Console.WriteLine("This is an isosceles triangle.");
            }
            else
            {
                Console.WriteLine("This is a scalene triangle.");
            }
        }
    }
}
```

#### Sample Input

```text
5 3 4
```

#### Sample Output

```text
This is a right triangle.
```

---

### Exercise 5 - Numerical properties

#### Code

```csharp
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
```

#### Sample Input

```text
1221
```

#### Sample Output

```text
1221 is a palindrome.
1221 does not have 3 equal digits.
1221 does not have 4 different digits.
```

---
### END