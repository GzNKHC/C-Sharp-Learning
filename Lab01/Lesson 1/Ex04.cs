// Time converter (h:m:s)

//OLD VERSION
/*
Console.WriteLine("Enter second: ");
double s = double.Parse(Console.ReadLine());

double tmp = s;
double h = 0;
double m = 0;

while (tmp >= 3600)
{
    h++;
    tmp -= 3600;
}

while (tmp >= 60)
{
    m++;
    tmp -= 60; 
}

Console.WriteLine(s + "s is: " + h + ":" + m + ":" + tmp + ":");
*/

//NEW VERSION
Console.Write("Enter seconds: ");
int s = int.Parse(Console.ReadLine());

int h = s / 3600;
int m = (s % 3600) / 60;
int sec = s % 60;

Console.WriteLine($"{s}s is: {h}:{m}:{sec}");