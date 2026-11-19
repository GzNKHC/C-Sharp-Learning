using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh8
{
    internal static class PrintGrid
    {
        public static void Show(Form owner, DataGridView grid, string title, string summary = "")
        {
            var columns = grid.Columns.Cast<DataGridViewColumn>().Where(column => column.Visible)
                .OrderBy(column => column.DisplayIndex).ToArray();
            var rows = grid.Rows.Cast<DataGridViewRow>().Where(row => !row.IsNewRow).ToArray();

            if (columns.Length == 0 || rows.Length == 0)
                throw new InvalidOperationException("Không có dữ liệu để in.");

            using (var document = new PrintDocument { DocumentName = title })
            using (var preview = new PrintPreviewDialog { Document = document, Width = 1000, Height = 720 })
            using (var heading = new Font("Arial", 15F, FontStyle.Bold))
            using (var normal = new Font("Arial", 9F))
            using (var cell = new StringFormat { Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap, LineAlignment = StringAlignment.Center })
            {
                int rowIndex = 0;
                document.BeginPrint += (sender, args) => rowIndex = 0;
                document.PrintPage += (sender, page) =>
                {
                    Rectangle bounds = page.MarginBounds;
                    float top = bounds.Top;
                    page.Graphics.DrawString(title, heading, Brushes.Black, bounds.Left, top);
                    top += 32;
                    if (summary.Length != 0)
                    {
                        var area = new RectangleF(bounds.Left, top, bounds.Width, 56);
                        page.Graphics.DrawString(summary, normal, Brushes.Black, area);
                        top += 58;
                    }

                    float columnWidth = (float)bounds.Width / columns.Length;
                    const float rowHeight = 24;
                    for (int i = 0; i < columns.Length; i++)
                        DrawCell(page.Graphics, normal, cell, columns[i].HeaderText,
                            bounds.Left + i * columnWidth, top, columnWidth, rowHeight, true);

                    top += rowHeight;
                    while (rowIndex < rows.Length && top + rowHeight <= bounds.Bottom)
                    {
                        for (int i = 0; i < columns.Length; i++)
                            DrawCell(page.Graphics, normal, cell,
                                Db.Value(rows[rowIndex].Cells[columns[i].Index].FormattedValue),
                                bounds.Left + i * columnWidth, top, columnWidth, rowHeight, false);
                        rowIndex++;
                        top += rowHeight;
                    }
                    page.HasMorePages = rowIndex < rows.Length;
                };
                preview.ShowDialog(owner);
            }
        }

        private static void DrawCell(Graphics graphics, Font font, StringFormat format, string text,
            float left, float top, float width, float height, bool header)
        {
            if (header)
                graphics.FillRectangle(Brushes.Gainsboro, left, top, width, height);
            graphics.DrawRectangle(Pens.Gray, left, top, width, height);
            graphics.DrawString(text, font, Brushes.Black,
                new RectangleF(left + 4, top, width - 8, height), format);
        }
    }
}
