namespace TH06b;

internal static class Ui
{
    public static Label L(string text, int x, int y, int w = 115) => new()
    {
        Text = text, Left = x, Top = y + 4, Width = w
    };

    public static TextBox T(string name, int x, int y, int w = 220) => new()
    {
        Name = name, Left = x, Top = y, Width = w
    };

    public static ComboBox C(string name, int x, int y, int w = 220) => new()
    {
        Name = name, Left = x, Top = y, Width = w, DropDownStyle = ComboBoxStyle.DropDownList
    };

    public static Button B(string text, int x, int y, EventHandler click, int w = 90)
    {
        var b = new Button { Text = text, Left = x, Top = y, Width = w, Height = 32 };
        b.Click += click;
        return b;
    }

    private static T Also<T>(this T control, Action<T> action)
    {
        action(control);
        return control;
    }
}
