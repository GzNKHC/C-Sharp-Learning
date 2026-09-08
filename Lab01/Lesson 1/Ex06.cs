// a^n

using System.IO.Pipelines;

Console.WriteLine("Enter a");
double a = double.Parse(Console.ReadLine());

Console.WriteLine("Enter n");
double n = double.Parse(Console.ReadLine());

double result = Math.Pow(a,n) ; 

Console.WriteLine("a^n = " + result);