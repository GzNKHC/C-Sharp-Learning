Console.WriteLine("Enter n: ");
int n = int.Parse(Console.ReadLine());

int result = 1;

for (int i = 0; i < 278; i++)
{
    result = (result * n) % 100;
}

Console.WriteLine($"Last 2 digits of {n}^278 is: {result:D2}");