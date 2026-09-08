# Lab01

## Lesson 1

### Exercise 1

```csharp
*******************************
* Truong: Dai Hoc Sai Gon     *
* Khoa: CNTT                  *
* Ho ten: Nguyen Khai Hoan Ca *
*******************************
""");
```

### Exercise 2

```csharp
// Perimeter & Area of a cycle

Console.WriteLine("Enter radius: ");

double r = double.Parse(Console.ReadLine());

double area = Math.PI * r * r;
double perimeter = Math.PI * r * 2;

Console.WriteLine("Area of cycle: " + area);
Console.WriteLine("Perimeter of cycle: " + perimeter);
```

### Exercise 3

```csharp
// seconds converter

Console.WriteLine("Enter hour: ");
double h = double.Parse(Console.ReadLine());

Console.WriteLine("Enter minute: ");
double m = double.Parse(Console.ReadLine());

Console.WriteLine("Enter second: ");
double s = double.Parse(Console.ReadLine());

double result = h*3600 + m*60 + s;

Console.WriteLine("Sum of " + h + ":" + m + ":" + s + " is: " + result + "s");
```

### Exercise 4

```csharp
// Time converter (h:m:s)

Console.Write("Enter seconds: ");
int s = int.Parse(Console.ReadLine());

int h = s / 3600;
int m = (s % 3600) / 60;
int sec = s % 60;

Console.WriteLine($"{s}s is: {h}:{m}:{sec}");
```

### Exercise 5

```csharp
// Area of triangle

Console.WriteLine("Enter length of a");
double a = double.Parse(Console.ReadLine());

Console.WriteLine("Enter length of b");
double b = double.Parse(Console.ReadLine());

Console.WriteLine("Enter length of c");
double c = double.Parse(Console.ReadLine());

double p = (a + b + c) / 2;
double S = Math.Sqrt( p*(p-a)*(p-b)*(p-c) );

Console.WriteLine($"Area of triangle:  {S:F2}");
```


### Exercise 6

```csharp
// a^n

using System.IO.Pipelines;

Console.WriteLine("Enter a");
double a = double.Parse(Console.ReadLine());

Console.WriteLine("Enter n");
double n = double.Parse(Console.ReadLine());

double result = Math.Pow(a,n) ; 

Console.WriteLine("a^n = " + result);
```

### Exercise 7
| STT | Lỗi thường gặp | Ví dụ sai | Cách sửa |
|---|---|---|---|
| 1 | Thiếu dấu `;` | `int a = 10` | `int a = 10;` |
| 2 | Biến chưa khai báo | `Console.WriteLine(a);` | Khai báo `a` trước |
| 3 | Khai báo biến 2 lần | `int a = 1; int a = 2;` | Chỉ khai báo một lần |
| 4 | Sai kiểu dữ liệu | `int a = "10";` | `int a = 10;` |
| 5 | Thiếu dấu ngoặc | `Console.WriteLine("Hi";` | Thêm `)` |
| 6 | Sai chữ hoa/thường | `console.WriteLine()` | `Console.WriteLine()` |

### End