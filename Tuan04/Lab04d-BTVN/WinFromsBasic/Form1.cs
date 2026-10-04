namespace WinFromsBasic
{
    public partial class frmDangKyKS : Form
    {
        //Dùng để thống kê cuối ngày
        private int tongSoKhach = 0;
        private double tongTien = 0;

        public frmDangKyKS()
        {
            InitializeComponent();
        }

        private void frmDangKyKS_Load(object sender, EventArgs e)
        {
            //Trạng thái ban đầu theo yêu cầu đề bài
            btnThanhToan.Enabled = false;
            btnNhapMoi.Enabled = false;
            btnTongKet.Enabled = false;

            txtThanhTien.ReadOnly = true;
            txtSoLuotNguoi.ReadOnly = true;
            txtTongSoTien.ReadOnly = true;

            txtHoVaTen.Focus();
        }

        //Kiểm tra dữ liệu bắt buộc để bật nút Thanh toán
        private void KiemTraDuLieu()
        {
            bool soNgayHopLe = int.TryParse(txtSoNgayO.Text, out int soNgay) && soNgay > 0;
            bool daChonPhong = radPhongDon.Checked || radPhongDoi.Checked || radPhongBa.Checked;

            btnThanhToan.Enabled =
                !string.IsNullOrWhiteSpace(txtHoVaTen.Text) &&
                !string.IsNullOrWhiteSpace(txtDiaChi.Text) &&
                soNgayHopLe &&
                daChonPhong;
        }

        private void txtHoVaTen_TextChanged(object sender, EventArgs e)
        {
            KiemTraDuLieu();
        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {
            KiemTraDuLieu();
        }

        private void txtSoNgayO_TextChanged(object sender, EventArgs e)
        {
            KiemTraDuLieu();
        }

        private void radPhongDon_CheckedChanged(object sender, EventArgs e)
        {
            KiemTraDuLieu();
        }

        private void radPhongDoi_CheckedChanged(object sender, EventArgs e)
        {
            KiemTraDuLieu();
        }

        private void radPhongBa_CheckedChanged(object sender, EventArgs e)
        {
            KiemTraDuLieu();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoNgayO.Text, out int soNgay) || soNgay <= 0)
            {
                MessageBox.Show(
                    "Số ngày ở phải là số nguyên lớn hơn 0.",
                    "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtSoNgayO.Focus();
                return;
            }

            double tienPhong;

            if (radPhongDon.Checked)
                tienPhong = 300000 * soNgay;
            else if (radPhongDoi.Checked)
                tienPhong = 350000 * soNgay;
            else
                tienPhong = 400000 * soNgay;

            //Mỗi tiện nghi cộng thêm 10.000đ/lượt thuê
            double tienTienNghi = 0;
            if (chkTivi.Checked) tienTienNghi += 10000;
            if (chkInternet.Checked) tienTienNghi += 10000;
            if (chkMayNuocNong.Checked) tienTienNghi += 10000;

            //Dịch vụ: Karaoke 50.000đ/lượt, ăn sáng 15.000đ/ngày
            double tienDichVu = 0;
            if (chkKaraoke.Checked) tienDichVu += 50000;
            if (chkAnSang.Checked) tienDichVu += 15000 * soNgay;

            double thanhTien = tienPhong + tienTienNghi + tienDichVu;

            txtThanhTien.Text = thanhTien.ToString("N0") + " VNĐ";

            //Cập nhật thống kê cuối ngày
            tongSoKhach++;
            tongTien += thanhTien;

            //Tránh thanh toán trùng cùng một khách
            btnThanhToan.Enabled = false;
            btnNhapMoi.Enabled = true;
            btnTongKet.Enabled = true;
        }

        private void btnNhapMoi_Click(object sender, EventArgs e)
        {
            txtHoVaTen.Clear();
            txtDiaChi.Clear();
            txtSoNgayO.Clear();
            txtThanhTien.Clear();

            radPhongDon.Checked = false;
            radPhongDoi.Checked = false;
            radPhongBa.Checked = false;

            chkTivi.Checked = false;
            chkInternet.Checked = false;
            chkMayNuocNong.Checked = false;
            chkKaraoke.Checked = false;
            chkAnSang.Checked = false;

            btnThanhToan.Enabled = false;
            btnNhapMoi.Enabled = false;

            txtHoVaTen.Focus();
        }

        private void btnTongKet_Click(object sender, EventArgs e)
        {
            txtSoLuotNguoi.Text = tongSoKhach.ToString();
            txtTongSoTien.Text = tongTien.ToString("N0") + " VNĐ";

            //Sau khi tổng kết, bắt đầu lại thống kê cho lượt/ngày tiếp theo
            tongSoKhach = 0;
            tongTien = 0;
            btnTongKet.Enabled = false;
        }

        private void btbThoat_Click(object sender, EventArgs e)
        {
            DialogResult ketQua = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát khỏi chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
