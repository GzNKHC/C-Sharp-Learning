using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ThucHanh9
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EntityTitleAttribute : Attribute
    {
        public EntityTitleAttribute(string title) { Title = title; }
        public string Title { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InputAttribute : Attribute
    {
        public InputAttribute(string title) { Title = title; }
        public string Title { get; }
        public Type Lookup { get; set; }
        public string LookupLabel { get; set; }
        public double Minimum { get; set; } = double.NaN;
        public double Maximum { get; set; } = double.NaN;
    }

    internal sealed class Field
    {
        public Field(PropertyInfo property, ColumnAttribute column, InputAttribute input)
        {
            Property = property;
            Column = column;
            Input = input;
        }

        public PropertyInfo Property { get; }
        public ColumnAttribute Column { get; }
        public InputAttribute Input { get; }
        public string Name => Property.Name;
        public string Header => Input.Title;
        public bool IsKey => Column.IsPrimaryKey;
        public bool Optional => Column.CanBeNull;
        public Type Type => Nullable.GetUnderlyingType(Property.PropertyType) ?? Property.PropertyType;
        public bool IsDateTime => Column.DbType.StartsWith("datetime2", StringComparison.Ordinal);
        public int Size
        {
            get
            {
                if (Type != typeof(string))
                    return 0;
                int start = Column.DbType.IndexOf('(') + 1;
                int length = Column.DbType.IndexOf(')') - start;
                return int.Parse(Column.DbType.Substring(start, length), CultureInfo.InvariantCulture);
            }
        }

        public object Get(object row) => Property.GetValue(row);
        public void Set(object row, object value) => Property.SetValue(row, value);

        public string Format(object value)
        {
            if (value is DateTime date)
                return date.ToString(IsDateTime ? "dd/MM/yyyy HH:mm:ss" : "dd/MM/yyyy", CultureInfo.CurrentCulture);
            if (value is decimal number)
                return number.ToString("0.##", CultureInfo.CurrentCulture);
            return Convert.ToString(value, CultureInfo.CurrentCulture) ?? "";
        }

        public object Parse(object value)
        {
            if (value is DateTime date)
                return IsDateTime ? date.AddTicks(-(date.Ticks % TimeSpan.TicksPerSecond)) : date.Date;

            string text = Convert.ToString(value, CultureInfo.CurrentCulture)?.Trim() ?? "";
            if (text.Length == 0 && Optional)
                return null;
            if (text.Length == 0)
                throw new InvalidOperationException("Chưa nhập " + Header.ToLower() + ".");

            if (Type == typeof(string))
            {
                if (text.Length > Size)
                    throw new InvalidOperationException(Header + " dài tối đa " + Size + " ký tự.");
                if (Input.Lookup == null && (Name == "GT" || Name == "GioiTinh") && text != "Nam" && text != "Nữ")
                    throw new InvalidOperationException("Hãy chọn Nam hoặc Nữ.");
                return text;
            }
            if (Type == typeof(int) && int.TryParse(text, NumberStyles.Integer, CultureInfo.CurrentCulture, out int integer))
            {
                CheckRange(integer);
                return integer;
            }
            if (Type == typeof(decimal) && decimal.TryParse(text,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture, out decimal number))
            {
                CheckRange(number);
                if (decimal.Round(number, 2) != number)
                    throw new InvalidOperationException(Header + " có tối đa hai chữ số thập phân.");
                return number;
            }
            throw new InvalidOperationException(Header + " không đúng định dạng.");
        }

        private void CheckRange(decimal value)
        {
            if (!double.IsNaN(Input.Minimum) && value < (decimal)Input.Minimum)
                throw new InvalidOperationException(Header + " phải từ " + Input.Minimum + " trở lên.");
            if (!double.IsNaN(Input.Maximum) && value > (decimal)Input.Maximum)
                throw new InvalidOperationException(Header + " không được vượt quá " + Input.Maximum + ".");
        }
    }

    internal static class TableInfo
    {
        private static readonly Dictionary<Type, Field[]> Cache = new Dictionary<Type, Field[]>();

        public static string Table(Type type)
        {
            string name = type.GetCustomAttribute<TableAttribute>().Name;
            return string.Join(".", name.Split('.').Select(Quote));
        }

        public static string Title(Type type) => type.GetCustomAttribute<EntityTitleAttribute>().Title;
        public static string Quote(string name) => "[" + name.Replace("]", "]]") + "]";

        public static Field[] Fields(Type type)
        {
            if (!Cache.TryGetValue(type, out Field[] columns))
            {
                columns = type.GetProperties().Select(property => new
                {
                    Property = property,
                    Column = property.GetCustomAttribute<ColumnAttribute>(),
                    Input = property.GetCustomAttribute<InputAttribute>()
                }).Where(item => item.Column != null)
                  .Select(item => new Field(item.Property, item.Column, item.Input)).ToArray();
                Cache.Add(type, columns);
            }
            return columns;
        }

        public static Field Find(Type type, string name) => Fields(type).Single(field => field.Name == name);

        public static Expression<Func<T, bool>> ByKey<T>(T value)
        {
            var row = Expression.Parameter(typeof(T), "row");
            Expression test = null;
            foreach (Field key in Fields(typeof(T)).Where(field => field.IsKey))
            {
                Expression match = Expression.Equal(Expression.Property(row, key.Property),
                    Expression.Constant(key.Get(value), key.Property.PropertyType));
                test = test == null ? match : Expression.AndAlso(test, match);
            }
            return Expression.Lambda<Func<T, bool>>(test, row);
        }

        public static string KeyText(object row)
        {
            return string.Join(" / ", Fields(row.GetType()).Where(field => field.IsKey).Select(field => field.Format(field.Get(row))));
        }

        public static void Copy<T>(T from, T to) where T : class
        {
            foreach (Field field in Fields(typeof(T)).Where(field => !field.IsKey))
                field.Set(to, field.Get(from));
        }
    }
}
