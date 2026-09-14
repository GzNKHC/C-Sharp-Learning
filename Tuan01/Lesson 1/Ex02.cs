// Perimeter & Area of a cycle

Console.WriteLine("Enter radius: ");

double r = double.Parse(Console.ReadLine());

double area = Math.PI * r * r;
double perimeter = Math.PI * r * 2;

Console.WriteLine("Area of cycle: " + area);
Console.WriteLine("Perimeter of cycle: " + perimeter);