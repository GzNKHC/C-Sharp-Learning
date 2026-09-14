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