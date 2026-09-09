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