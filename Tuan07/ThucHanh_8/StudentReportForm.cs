using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh8
{
    internal sealed class StudentForm : EditForm
    {
        private readonly ComboBox faculty = Ui.Combo();
        private readonly ComboBox classFilter = Ui.Combo();
        private readonly ComboBox classInput = Ui.Combo();
        private readonly DateTimePicker birthDate = Ui.DatePicker();
        private readonly TableLayoutPanel filterArea = Ui.Fields();
        private readonly Button find;
        private string filteredClass = "";
        private string filteredFaculty = "";
        private bool loadingChoices;

        public StudentForm() : base("Danh sách sinh viên", "Mã sinh viên", "Họ tên sinh viên")
        {
            Ui.Field(filterArea, 0, "Khoa", faculty);
            Ui.Field(filterArea, 1, "Lớp", classFilter);
            var searchBar = Ui.Actions();
            find = Ui.Button(searchBar, "Tìm kiếm", (sender, args) => Ui.Run(this, () =>
            {
                filteredClass = Ui.Selected(classFilter);
                filteredFaculty = Ui.Selected(faculty);
                Reload();
            }));
            Ui.Field(filterArea, 2, "", searchBar);
            Ui.Field(Fields, 2, "Ngày sinh", birthDate);
            Ui.Field(Fields, 3, "Lớp sinh viên", classInput);
            Ui.Arrange(this, Grid, Toolbar, filterArea, Fields);
            faculty.SelectedValueChanged += (sender, args) =>
            {
                if (!loadingChoices && faculty.SelectedValue is string)
                    Ui.Run(this, LoadClassesForFaculty);
            };
        }

        protected override string CodeName => "MaSinhVien";
        protected override string NameColumn => "HoTen";

        protected override void InitializeChoices()
        {
            Ui.Bind(classInput, Db.ReadWithReader("SELECT MaLop, TenLop FROM dbo.Lop ORDER BY TenLop"),
                "MaLop", "TenLop");
            loadingChoices = true;
            try
            {
                Ui.Bind(faculty, Db.ReadWithReader("SELECT MaKhoa, TenKhoa FROM dbo.Khoa ORDER BY TenKhoa"),
                    "MaKhoa", "TenKhoa", "Tất cả khoa");
            }
            finally
            {
                loadingChoices = false;
            }
            LoadClassesForFaculty();
        }

        private void LoadClassesForFaculty()
        {
            string code = Ui.Selected(faculty);
            DataTable classes = Db.ReadWithReader(@"SELECT MaLop, TenLop FROM dbo.Lop
                WHERE @ma = N'' OR MaKhoa = @ma ORDER BY TenLop", Db.Text("@ma", code));
            // Ô trống tương ứng với tất cả lớp khi chưa chọn khoa.
            Ui.Bind(classFilter, classes, "MaLop", "TenLop", code.Length == 0 ? "Tất cả lớp" : null);
            if (code.Length > 0 && classes.Rows.Count > 0)
                classFilter.SelectedIndex = 0;
        }

        protected override DataTable ReadRows()
        {
            return StudentReportData.ReadStudents(filteredClass, filteredFaculty);
        }

        protected override void ShowPrintPreview()
        {
            var students = Grid.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Visible)
                .Select(row => ((DataRowView)row.DataBoundItem).Row);
            using (DataTable report = StudentReportData.ForReport(students))
            {
                if (report.Rows.Count == 0)
                    throw new InvalidOperationException("Không có sinh viên để in theo bộ lọc hiện tại.");
                using (var form = new StudentReportForm("Xem in danh sách sinh viên", false, report))
                    form.ShowDialog(this);
            }
        }

        protected override void FormatOtherColumns()
        {
            Ui.Header(Grid, "NgaySinh", "Ngày sinh", "dd/MM/yyyy");
            Ui.Header(Grid, "TenLop", "Tên lớp");
            Grid.Columns["MaLop"].Visible = false;
        }

        protected override void ShowOtherFields(DataRow row)
        {
            birthDate.Value = row == null ? DateTime.Today : (DateTime)row["NgaySinh"];
            classInput.SelectedValue = row == null ? Ui.Selected(classFilter) : Db.Value(row["MaLop"]);
        }

        protected override void EnableOtherFields(bool editing)
        {
            birthDate.Enabled = classInput.Enabled = editing;
            find.Enabled = faculty.Enabled = classFilter.Enabled = !editing;
        }

        protected override void Store(string code, string name, bool insert)
        {
            string classCode = Ui.Selected(classInput);
            if (classCode.Length == 0)
                throw new InvalidOperationException("Hãy chọn lớp cho sinh viên.");
            if (birthDate.Value.Date > DateTime.Today)
                throw new InvalidOperationException("Ngày sinh không thể sau hôm nay.");

            Db.InTransaction((connection, transaction) =>
            {
                if (!Db.Exists(connection, transaction, "SELECT 1 FROM dbo.Lop WHERE MaLop = @ma",
                    Db.Text("@ma", classCode)))
                    throw new InvalidOperationException("Lớp đã chọn không tồn tại.");
                if (insert && Db.Exists(connection, transaction, "SELECT 1 FROM dbo.SinhVien WHERE MaSinhVien = @ma",
                    Db.Text("@ma", code)))
                    throw new InvalidOperationException("Mã sinh viên đã tồn tại.");

                string sql = insert
                    ? "INSERT dbo.SinhVien (MaSinhVien, HoTen, NgaySinh, MaLop) VALUES (@ma, @ten, @ngay, @lop)"
                    : "UPDATE dbo.SinhVien SET HoTen = @ten, NgaySinh = @ngay, MaLop = @lop WHERE MaSinhVien = @ma";
                if (Db.Execute(connection, transaction, sql, Db.Text("@ma", code), Db.Text("@ten", name, 100),
                    Db.Number("@ngay", SqlDbType.Date, birthDate.Value.Date), Db.Text("@lop", classCode)) != 1)
                    throw new InvalidOperationException("Sinh viên này không còn trong cơ sở dữ liệu.");
            });
        }

        protected override void Remove(string code)
        {
            Db.InTransaction((connection, transaction) =>
            {
                if (Db.Exists(connection, transaction, "SELECT 1 FROM dbo.Diem WHERE MaSinhVien = @ma",
                    Db.Text("@ma", code)))
                    throw new InvalidOperationException("Sinh viên đã có điểm, không thể xóa.");
                if (Db.Execute(connection, transaction, "DELETE dbo.SinhVien WHERE MaSinhVien = @ma",
                    Db.Text("@ma", code)) != 1)
                    throw new InvalidOperationException("Sinh viên này không còn trong cơ sở dữ liệu.");
            });
        }
    }
}
