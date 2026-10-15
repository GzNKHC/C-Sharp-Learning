namespace TH06a;

internal static class Ui
{
    public static Label L(string text, int x, int y, int w = 110) => new()
    {
        Text = text, Left = x, Top = y + 4, Width = w, AutoSize = false
    };

    public static TextBox T(string name, int x, int y, int w = 220) => new()
    {
        Name = name, Left = x, Top = y, Width = w
    };

    public static Button B(string text, int x, int y, EventHandler click)
    {
        var b = new Button { Text = text, Left = x, Top = y, Width = 90, Height = 32 };
        b.Click += click;
        return b;
    }

    private static T Also<T>(this T control, Action<T> action)
    {
        action(control);
        return control;
    }
}
