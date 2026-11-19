using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal sealed class RecordField
    {
        public string Name { get; }
        public string Caption { get; }
        public Control Editor { get; }
        public string LookupTable { get; }
        public string LookupCaption { get; }
        public string AllCaption { get; }
        private readonly SqlDbType type;
        private readonly decimal maximum;
        private readonly bool required;

        public RecordField(string name, string caption, int length, bool required = true)
            : this(name, caption, Ui.TextBox(length), SqlDbType.NVarChar, 0, required)
        {
        }

        private RecordField(string name, string caption, Control editor, SqlDbType type,
            decimal maximum = 0, bool required = true)
        {
            Name = name;
            Caption = caption;
            Editor = editor;
            this.type = type;
            this.maximum = maximum;
            this.required = required;
        }

        public RecordField(string name, string caption, string lookupTable, string lookupCaption,
            string allCaption = null)
            : this(name, caption, Ui.Combo(), SqlDbType.NVarChar)
        {
            LookupTable = lookupTable;
            LookupCaption = lookupCaption;
            AllCaption = allCaption;
        }

        public static RecordField Date(string name, string caption)
        {
            return new RecordField(name, caption, Ui.DatePicker(), SqlDbType.Date);
        }

        public static RecordField Numeric(string name, string caption, decimal maximum, bool integer = false)
        {
            return new RecordField(name, caption, Ui.TextBox(20),
                integer ? SqlDbType.Int : SqlDbType.Decimal, maximum);
        }

        public string Input
        {
            get
            {
                if (Editor is ComboBox combo)
                    return Ui.Selected(combo);
                return Editor.Text.Trim();
            }
        }

        public object Read()
        {
            if (Editor is DateTimePicker date)
            {
                if (date.Value.Date > DateTime.Today)
                    throw new InvalidOperationException(Caption + " không thể sau hôm nay.");
                return date.Value.Date;
            }

            string value = Input;
            if (required && value.Length == 0)
                throw new InvalidOperationException("Vui lòng nhập hoặc chọn " + Caption.ToLower() + ".");

            if (type == SqlDbType.Int)
            {
                if (int.TryParse(value, out int number) && number >= 0 && number <= maximum)
                    return number;
                throw new InvalidOperationException(Caption + " phải là số nguyên không âm.");
            }

            if (type == SqlDbType.Decimal)
            {
                if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                    CultureInfo.CurrentCulture, out decimal number) &&
                    number >= 0 && number <= maximum && decimal.Round(number, 2) == number)
                    return number;
                throw new InvalidOperationException(Caption + " phải từ 0 đến " + maximum +
                    " và có tối đa hai chữ số thập phân.");
            }

            return value;
        }

        public void Show(object value)
        {
            if (Editor is ComboBox combo)
                combo.SelectedValue = value ?? "";
            else if (Editor is DateTimePicker date)
                date.Value = value is DateTime day ? day : DateTime.Today;
            else
                Editor.Text = Db.Value(value);
        }

        public bool Matches(object value)
        {
            string input = Input;
            if (input.Length == 0)
                return true;
            if (LookupTable != null)
                return string.Equals(Db.Value(value), input, StringComparison.CurrentCultureIgnoreCase);
            if (type == SqlDbType.Int || type == SqlDbType.Decimal)
                return Convert.ToDecimal(value) == Convert.ToDecimal(Read());
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(Db.Value(value), input,
                CompareOptions.IgnoreCase) >= 0;
        }
    }
}
