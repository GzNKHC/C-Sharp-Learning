using System;
using System.Data;
using System.Data.SqlClient;

namespace ThucHanh7
{
    internal sealed class GridField
    {
        public string Name { get; }
        public string Caption { get; }
        public SqlDbType Type { get; }
        public int MaxLength { get; }
        public bool PrimaryKey { get; }
        public string LookupTable { get; }
        public string LookupCaption { get; }
        public string Format { get; }

        public GridField(string name, string caption, SqlDbType type, int maxLength = 0,
            bool primaryKey = false, string lookupTable = null, string lookupCaption = null,
            string format = null)
        {
            Name = name;
            Caption = caption;
            Type = type;
            MaxLength = maxLength;
            PrimaryKey = primaryKey;
            LookupTable = lookupTable;
            LookupCaption = lookupCaption;
            Format = format;
        }

        public object Check(DataRow row)
        {
            if (row.IsNull(Name))
                throw new InvalidOperationException("Thiếu " + Caption.ToLower() + ".");

            if (Type == SqlDbType.NVarChar)
            {
                string value = Db.Value(row[Name]).Trim();
                if (value.Length == 0 || value.Length > MaxLength)
                    throw new InvalidOperationException(Caption + " phải có từ 1 đến " + MaxLength + " ký tự.");
                return value;
            }

            if (Type == SqlDbType.Date)
            {
                var date = ((DateTime)row[Name]).Date;
                if (date.Year < 1753 || date > DateTime.Today)
                    throw new InvalidOperationException("Ngày sinh phải hợp lệ và không sau hôm nay.");
                return date;
            }

            var score = (decimal)row[Name];
            if (score < 0 || score > 10 || decimal.Round(score, 2) != score)
                throw new InvalidOperationException("Điểm phải từ 0 đến 10 và có tối đa hai chữ số thập phân.");
            return score;
        }

        public SqlParameter Parameter(string name, object value)
        {
            return Type == SqlDbType.NVarChar
                ? Db.Text(name, Db.Value(value), MaxLength)
                : Db.Number(name, Type, value);
        }
    }
}
