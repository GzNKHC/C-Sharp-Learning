# Lab 01

## Lesson 1

---

### Exercise 1 — Student Information

#### Code

```csharp
Console.WriteLine("""
*******************************
* Truong: Dai Hoc Sai Gon     *
* Khoa: CNTT                  *
* Ho ten: Nguyen Khai Hoan Ca *
*******************************
""");
```

#### Output

```text
*******************************
* Truong: Dai Hoc Sai Gon     *
* Khoa: CNTT                  *
* Ho ten: Nguyen Khai Hoan Ca *
*******************************
```

---

### Exercise 2 — Perimeter & Area of a Circle

#### Code

```csharp
// Perimeter & Area of a circle

Console.WriteLine("Enter radius: ");
double r = double.Parse(Console.ReadLine());

double area = Math.PI * r * r;
double perimeter = Math.PI * r * 2;

Console.WriteLine("Area of circle: " + area);
Console.WriteLine("Perimeter of circle: " + perimeter);
```

#### Sample Input

```text
5
```

#### Sample Output

```text
Area of circle: 78.53981633974483
Perimeter of circle: 31.41592653589793
```

---

### Exercise 3 — Convert Time to Seconds

#### Code

```csharp
// Seconds converter

Console.WriteLine("Enter hour: ");
double h = double.Parse(Console.ReadLine());

Console.WriteLine("Enter minute: ");
double m = double.Parse(Console.ReadLine());

Console.WriteLine("Enter second: ");
double s = double.Parse(Console.ReadLine());

double result = h * 3600 + m * 60 + s;

Console.WriteLine("Sum of " + h + ":" + m + ":" + s + " is: " + result + "s");
```

#### Sample Input

```text
1
2
3
```

#### Sample Output

```text
Sum of 1:2:3 is: 3723s
```

---

### Exercise 4 — Convert Seconds to Time

#### Code

```csharp
// Time converter (h:m:s)

Console.Write("Enter seconds: ");
int s = int.Parse(Console.ReadLine());

int h = s / 3600;
int m = (s % 3600) / 60;
int sec = s % 60;

Console.WriteLine($"{s}s is: {h}:{m}:{sec}");
```

#### Sample Input

```text
3665
```

#### Sample Output

```text
3665s is: 1:1:5
```

---

### Exercise 5 — Area of a Triangle

#### Code

```csharp
// Area of triangle

Console.WriteLine("Enter length of a");
double a = double.Parse(Console.ReadLine());

Console.WriteLine("Enter length of b");
double b = double.Parse(Console.ReadLine());

Console.WriteLine("Enter length of c");
double c = double.Parse(Console.ReadLine());

double p = (a + b + c) / 2;
double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

Console.WriteLine($"Area of triangle: {S:F2}");
```

#### Sample Input

```text
3
4
5
```

#### Sample Output

```text
Area of triangle: 6.00
```

---

### Exercise 6 — Calculate a^n

#### Code

```csharp
// a^n

Console.WriteLine("Enter a");
double a = double.Parse(Console.ReadLine());

Console.WriteLine("Enter n");
double n = double.Parse(Console.ReadLine());

double result = Math.Pow(a, n);

Console.WriteLine("a^n = " + result);
```

#### Sample Input

```text
2
5
```

#### Sample Output

```text
a^n = 32
```

---

### Exercise 7 — Common Syntax Errors

| No. | Common Error | Incorrect Example | Correction |
|---:|---|---|---|
| 1 | Missing `;` | `int a = 10` | `int a = 10;` |
| 2 | Variable not declared | `Console.WriteLine(a);` | Declare `a` before using it |
| 3 | Variable declared twice | `int a = 1; int a = 2;` | Declare it only once |
| 4 | Incorrect data type | `int a = "10";` | `int a = 10;` |
| 5 | Missing bracket | `Console.WriteLine("Hi";` | Add `)` |
| 6 | Incorrect capitalization | `console.WriteLine()` | `Console.WriteLine()` |

---

## End
