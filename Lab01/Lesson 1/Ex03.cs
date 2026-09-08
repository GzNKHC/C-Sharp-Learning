// seconds converter

Console.WriteLine("Enter hour: ");
double h = double.Parse(Console.ReadLine());

Console.WriteLine("Enter minute: ");
double m = double.Parse(Console.ReadLine());

Console.WriteLine("Enter second: ");
double s = double.Parse(Console.ReadLine());

double result = h*3600 + m*60 + s;

Console.WriteLine("Sum of " + h + ":" + m + ":" + s + " is: " + result + "s");