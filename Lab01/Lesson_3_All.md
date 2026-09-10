# Lab 01

## Lesson 3

---

### Exercise 1 - Find max min in array

#### Code

```csharp
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
```

#### Sample Input

```text
1 5 6 2 8
```

#### Sample Output

```text
Max: 8
Min: 1
```

---

### Exercise 2 - Function calc

#### Code

```csharp
using System;

class funtionCalc
{
    static double f1(double x)
    {
        if(x <= 0)
        {
            return 0;
        } else if (x <= 1)
        {
            return x;
        }     else
        {
            return Math.Pow(x,4);
        }
    }

    static double f2(double x)
    {
        if (x <=2)
        {
            return Math.Pow(x,2) + 4*x + 5;
        } else
        {
            return 1/(Math.Pow(x,2) + 4*x + 5);
        }
    }

    static void Main()
    {
        Console.WriteLine("Enter x: ");
        double x = double.Parse(Console.ReadLine());

        Console.WriteLine($"""
        f1({x}) = {f1(x)}
        f2({x}) = {f2(x)}
        """);
    }
}
```

#### Sample Input

```text
1.2
```

#### Sample Output

```text
f1(1.2) = 2.0736
f2(1.2) = 11.24
```

---

### Exercise 3 - Solve the quadratic equation 

#### Code

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter a: ");
        double a = double.Parse(Console.ReadLine());

        Console.WriteLine("Enter b: ");
        double b = double.Parse(Console.ReadLine());

        Console.WriteLine("Enter c: ");
        double c = double.Parse(Console.ReadLine());


        //check
        if(a == 0)
        {
                Console.WriteLine("Not a equadratic equation");
        } else
        {
            double delta = b * b - 4*a*c;
            if(delta < 0)
            {
                Console.WriteLine("No real solution");
            } else if (delta == 0)
            {
                Console.WriteLine($"The equation has a double root: x = {(-(double)b / (2 * a)):F2}");
            } else
            {
                Console.WriteLine($"""
                The equation has 2 distinct roots:
                x1 = {((double)(-b + Math.Sqrt(delta)) / (2 * a)):F2}
                x2 = {((double)(-b - Math.Sqrt(delta)) / (2 * a)):F2}
                """);
            }
        }
    }
}
```

#### Sample Input

```text
1 
5 
6
```

#### Sample Output

```text
The equation has 2 distinct roots:
x1 = -2.00
x2 = -3.00
```

---


### Exercise 4 - Read month in English

#### Code

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter month (1 - 12): ");
        int M = int.Parse(Console.ReadLine());

        switch (M)
        {
            case 1:
            Console.WriteLine($"Month {M} is: January");
            break;

            case 2:
            Console.WriteLine($"Month {M} is: February");
            break;

            case 3:
            Console.WriteLine($"Month {M} is: March");
            break;

            case 4:
            Console.WriteLine($"Month {M} is: April");
            break;

            case 5:
            Console.WriteLine($"Month {M} is: May");
            break;

            case 6:
            Console.WriteLine($"Month {M} is: June");
            break;

            case 7:
            Console.WriteLine($"Month {M} is: July");
            break;

            case 8:
            Console.WriteLine($"Month {M} is: August");
            break;

            case 9:
            Console.WriteLine($"Month {M} is: September");
            break;

            case 10:
            Console.WriteLine($"Month {M} is: October");
            break;

            case 11:
            Console.WriteLine($"Month {M} is: November");
            break;

            case 12:
            Console.WriteLine($"Month {M} is: Devember");
            break;
        }
    }
}
```

#### Sample Input

```text
5
```

#### Sample Output

```text
Month 5 is: May
```

---

### Exercise 5

#### Code

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter month: ");
        int M = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter year: ");
        int Y = int.Parse(Console.ReadLine());

        if(M==1 || M==3 || M==5 || M==7 || M==8 || M==10 || M==12)
        {
            Console.WriteLine($"Month {M} in {Y} has: 31 days");
        } else if(M == 2)
        {
            if (Y % 400 == 0)
            {
                Console.WriteLine($"Month {M} in {Y} has: 29 days");
            } else
            {
                Console.WriteLine($"Month {M} in {Y} has: 28 days");
            }
        } else
        {
            Console.WriteLine($"Month {M} in {Y} has: 30 days");
        }
    }
}
```

#### Sample Input

```text
1
2015
```

#### Sample Output

```text
Month 2 in 2000 has: 29 days
```

---

### END