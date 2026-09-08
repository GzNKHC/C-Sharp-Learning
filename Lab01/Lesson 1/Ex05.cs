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