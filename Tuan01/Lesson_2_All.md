# Lab 01

## Lesson 2

---

### Exercise 1 - Sum of [a,b]

#### Code

```csharp
Console.WriteLine("Enter a, b: ");

string[] input = Console.ReadLine().Split();

int a = int.Parse(input[0]);
int b = int.Parse(input[1]);

int result = 0;

int tmp = a;
while (tmp <= b)
{
    result += tmp;
    tmp++;
}

Console.WriteLine("Sum of [" + a + " -> " + b + "] is: " + result);
```

#### Sample Input

```text
3 5
```

#### Sample Output

```text
Sum of [3 -> 5] is: 12
```

---

### Exercise 2

#### Code

```csharp
Console.WriteLine("Enter a real number : ");
double a = double.Parse(Console.ReadLine());

Console.WriteLine(a + "^2 =" + Math.Pow(a,2) + " | " + a + "^5 =" + Math.Pow(a,5) + " | " + a + "^17 = " + Math.Pow(a,17));
```

#### Sample Input

```text
2
```

#### Sample Output

```text
2^2 =4 | 2^5 =32 | 2^17 = 131072
```

---

### Exercise 3

#### Code

```csharp
Console.WriteLine("Enter x: ");
double x = double.Parse(Console.ReadLine());

double fx = 1 + 2 * x + 3 * Math.Pow(x,2) - 4 * Math.Pow(x,3);

Console.WriteLine("f(x) = 1 + 2x + 3x^2 - 4x^3");
Console.WriteLine("f(" + x + ") =" + fx);
```

#### Sample Input

```text
3
```

#### Sample Output

```text
f(x) = 1 + 2x + 3x^2 - 4x^3
f(3) =-74
```

---

### Exercise 4

#### Code

```csharp
Console.WriteLine("Enter month: ");
int M = int.Parse(Console.ReadLine());

if (M < 4)
{
    Console.WriteLine("Month " + M + " belongs to Quarter 1");
} else if (M < 7)
{
    Console.WriteLine("Month " + M + " belongs to Quarter 2");
} else if (M < 10)
{
    Console.WriteLine("Month " + M + " belongs to Quarter 3");
} else
{
    Console.WriteLine("Month " + M + " belongs to Quarter 4");
}
```

#### Sample Input

```text
3
```

#### Sample Output

```text
Month 3 belongs to Quarter 1
```

---

### Exercise 5

#### Code

```csharp
Console.WriteLine("Enter n: ");
int n = int.Parse(Console.ReadLine());

int result = 1;

for (int i = 0; i < 278; i++)
{
    result = (result * n) % 100;
}

Console.WriteLine($"Last 2 digits of {n}^278 is: {result:D2}");
```

#### Sample Input

```text
2
```

#### Sample Output

```text
Last 2 digits of 2^278 is: 44
```

---

## End