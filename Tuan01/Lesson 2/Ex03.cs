Console.WriteLine("Enter x: ");
double x = double.Parse(Console.ReadLine());

double fx = 1 + 2 * x + 3 * Math.Pow(x,2) - 4 * Math.Pow(x,3);

Console.WriteLine("f(x) = 1 + 2x + 3x^2 - 4x^3");
Console.WriteLine("f(" + x + ") =" + fx);