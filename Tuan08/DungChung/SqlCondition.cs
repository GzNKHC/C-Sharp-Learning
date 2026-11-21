using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ThucHanh9
{
    internal sealed class SqlCondition
    {
        private readonly SqlCommand command;
        private readonly ParameterExpression row;

        private SqlCondition(SqlCommand command, ParameterExpression row)
        {
            this.command = command;
            this.row = row;
        }

        public static string Build(SqlCommand command, LambdaExpression criteria)
        {
            return new SqlCondition(command, criteria.Parameters[0]).Visit(criteria.Body);
        }

        public static string Parameter(SqlCommand command, object value, Field field)
        {
            string name = "@p" + command.Parameters.Count;
            SqlParameter parameter;
            if (field == null)
                parameter = command.Parameters.AddWithValue(name, value ?? DBNull.Value);
            else if (field.Type == typeof(string))
            {
                int size = Math.Max(field.Size, (value as string)?.Length ?? 0);
                parameter = command.Parameters.Add(name, SqlDbType.NVarChar, size > 4000 ? -1 : size);
            }
            else if (field.Type == typeof(int))
                parameter = command.Parameters.Add(name, SqlDbType.Int);
            else if (field.Type == typeof(decimal))
                parameter = command.Parameters.Add(name, SqlDbType.Decimal);
            else
                parameter = command.Parameters.Add(name, field.IsDateTime ? SqlDbType.DateTime2 : SqlDbType.Date);
            parameter.Value = value ?? DBNull.Value;
            return name;
        }

        private Field Column(Expression node)
        {
            if (node is UnaryExpression unary && node.NodeType == ExpressionType.Convert)
                node = unary.Operand;
            if (node is MemberExpression member && member.Expression == row)
                return TableInfo.Find(row.Type, member.Member.Name);
            return null;
        }

        private static object Value(Expression node)
        {
            return Expression.Lambda<Func<object>>(Expression.Convert(node, typeof(object))).Compile().Invoke();
        }

        private string Visit(Expression node)
        {
            if (node is BinaryExpression pair)
            {
                if (node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse)
                {
                    string junction = node.NodeType == ExpressionType.AndAlso ? " AND " : " OR ";
                    return "(" + Visit(pair.Left) + junction + Visit(pair.Right) + ")";
                }

                Field left = Column(pair.Left);
                Field right = Column(pair.Right);
                if (left != null || right != null)
                {
                    object leftValue = left == null ? Value(pair.Left) : null;
                    object rightValue = right == null ? Value(pair.Right) : null;
                    if ((left == null && leftValue == null) || (right == null && rightValue == null))
                    {
                        if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual)
                            return TableInfo.Quote((left ?? right).Name) +
                                (node.NodeType == ExpressionType.Equal ? " IS NULL" : " IS NOT NULL");
                        return "(1 = 0)";
                    }
                    string lhs = left == null ? Parameter(command, leftValue, right) : TableInfo.Quote(left.Name);
                    string rhs = right == null ? Parameter(command, rightValue, left) : TableInfo.Quote(right.Name);
                    return lhs + " " + Operator(node.NodeType) + " " + rhs;
                }
            }
            if (node is MethodCallExpression call && call.Method.Name == nameof(string.Contains) &&
                call.Object != null && call.Arguments.Count == 1 && Column(call.Object) is Field field)
            {
                return "CHARINDEX(" + Parameter(command, Value(call.Arguments[0]), null) + ", " +
                    TableInfo.Quote(field.Name) + ") > 0";
            }
            return (bool)Value(node) ? "(1 = 1)" : "(1 = 0)";
        }

        private static string Operator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal: return "=";
                case ExpressionType.NotEqual: return "<>";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.LessThanOrEqual: return "<=";
                default: throw new NotSupportedException("Điều kiện tìm kiếm không được hỗ trợ.");
            }
        }
    }
}
