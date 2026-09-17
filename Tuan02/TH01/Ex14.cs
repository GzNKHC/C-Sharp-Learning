using System;

class Staff
{
    public string FullName;
    public long GrossSalary;
    public int AbsentDays;
    private long netSalary;

    public void Input()
    {
        Console.WriteLine("Nhập họ tên nhân viên: ");
        FullName = Console.ReadLine();

        Console.WriteLine("Nhập lương: ");
        GrossSalary = long.Parse(Console.ReadLine());

        Console.WriteLine("Nhập số ngày nghỉ: ");
        AbsentDays= int.Parse(Console.ReadLine());
    }

    public void DisplaySalaryCalc()
    {
        netSalary = GrossSalary - 100000*(AbsentDays);
        Console.WriteLine($"Lương nhận được: {netSalary:N0}vnđ");
    }
}

class Program
{
    static void Main()
    {
        Staff s = new Staff();
        s.Input();
        s.DisplaySalaryCalc();
    }
}