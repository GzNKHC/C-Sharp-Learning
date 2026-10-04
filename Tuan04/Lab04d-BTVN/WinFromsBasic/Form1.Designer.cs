namespace WinFromsBasic
{
    partial class frmDangKyKS
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            groupBox3 = new GroupBox();
            chkAnSang = new CheckBox();
            chkKaraoke = new CheckBox();
            groupBox2 = new GroupBox();
            chkMayNuocNong = new CheckBox();
            chkInternet = new CheckBox();
            chkTivi = new CheckBox();
            label2 = new Label();
            groupBox1 = new GroupBox();
            radPhongBa = new RadioButton();
            radPhongDoi = new RadioButton();
            radPhongDon = new RadioButton();
            txtSoNgayO = new TextBox();
            txtDiaChi = new TextBox();
            txtHoVaTen = new TextBox();
            label4 = new Label();
            label3 = new Label();
            LblTieuDe = new Label();
            panel2 = new Panel();
            grbTTTK = new GroupBox();
            lblTongSoTien = new Label();
            lblSoLuotNguoi = new Label();
            txtTongSoTien = new TextBox();
            txtSoLuotNguoi = new TextBox();
            btnNhapMoi = new Button();
            btbThoat = new Button();
            btnTongKet = new Button();
            btnThanhToan = new Button();
            lblThanhTien = new Label();
            txtThanhTien = new TextBox();
            panel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            grbTTTK.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(txtSoNgayO);
            panel1.Controls.Add(txtDiaChi);
            panel1.Controls.Add(txtHoVaTen);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Location = new Point(3, 88);
            panel1.Name = "panel1";
            panel1.Size = new Size(602, 356);
            panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(chkAnSang);
            groupBox3.Controls.Add(chkKaraoke);
            groupBox3.Location = new Point(415, 163);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(175, 182);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Dịch vụ";
            // 
            // chkAnSang
            // 
            chkAnSang.AutoSize = true;
            chkAnSang.Location = new Point(19, 92);
            chkAnSang.Name = "chkAnSang";
            chkAnSang.Size = new Size(103, 29);
            chkAnSang.TabIndex = 0;
            chkAnSang.Text = "Ăn sáng";
            chkAnSang.UseVisualStyleBackColor = true;
            // 
            // chkKaraoke
            // 
            chkKaraoke.AutoSize = true;
            chkKaraoke.Location = new Point(19, 46);
            chkKaraoke.Name = "chkKaraoke";
            chkKaraoke.Size = new Size(101, 29);
            chkKaraoke.TabIndex = 0;
            chkKaraoke.Text = "Karaoke";
            chkKaraoke.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkMayNuocNong);
            groupBox2.Controls.Add(chkInternet);
            groupBox2.Controls.Add(chkTivi);
            groupBox2.Location = new Point(213, 163);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(175, 182);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tiện nghi";
            // 
            // chkMayNuocNong
            // 
            chkMayNuocNong.AutoSize = true;
            chkMayNuocNong.Location = new Point(16, 122);
            chkMayNuocNong.Name = "chkMayNuocNong";
            chkMayNuocNong.Size = new Size(164, 29);
            chkMayNuocNong.TabIndex = 0;
            chkMayNuocNong.Text = "Máy nước nóng";
            chkMayNuocNong.UseVisualStyleBackColor = true;
            // 
            // chkInternet
            // 
            chkInternet.AutoSize = true;
            chkInternet.Location = new Point(16, 76);
            chkInternet.Name = "chkInternet";
            chkInternet.Size = new Size(99, 29);
            chkInternet.TabIndex = 0;
            chkInternet.Text = "Internet";
            chkInternet.UseVisualStyleBackColor = true;
            // 
            // chkTivi
            // 
            chkTivi.AutoSize = true;
            chkTivi.Location = new Point(16, 30);
            chkTivi.Name = "chkTivi";
            chkTivi.Size = new Size(64, 29);
            chkTivi.TabIndex = 0;
            chkTivi.Text = "Tivi";
            chkTivi.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 21);
            label2.Name = "label2";
            label2.Size = new Size(93, 25);
            label2.TabIndex = 0;
            label2.Text = "Họ và tên:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radPhongBa);
            groupBox1.Controls.Add(radPhongDoi);
            groupBox1.Controls.Add(radPhongDon);
            groupBox1.Location = new Point(9, 163);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(175, 182);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Loại phòng";
            // 
            // radPhongBa
            // 
            radPhongBa.AutoSize = true;
            radPhongBa.Location = new Point(13, 123);
            radPhongBa.Name = "radPhongBa";
            radPhongBa.Size = new Size(114, 29);
            radPhongBa.TabIndex = 0;
            radPhongBa.TabStop = true;
            radPhongBa.Text = "Phòng ba";
            radPhongBa.UseVisualStyleBackColor = true;
            radPhongBa.CheckedChanged += radPhongBa_CheckedChanged;
            // 
            // radPhongDoi
            // 
            radPhongDoi.AutoSize = true;
            radPhongDoi.Location = new Point(13, 77);
            radPhongDoi.Name = "radPhongDoi";
            radPhongDoi.Size = new Size(120, 29);
            radPhongDoi.TabIndex = 0;
            radPhongDoi.TabStop = true;
            radPhongDoi.Text = "Phòng đôi";
            radPhongDoi.UseVisualStyleBackColor = true;
            radPhongDoi.CheckedChanged += radPhongDoi_CheckedChanged;
            // 
            // radPhongDon
            // 
            radPhongDon.AutoSize = true;
            radPhongDon.Location = new Point(13, 30);
            radPhongDon.Name = "radPhongDon";
            radPhongDon.Size = new Size(126, 29);
            radPhongDon.TabIndex = 0;
            radPhongDon.TabStop = true;
            radPhongDon.Text = "Phòng đơn";
            radPhongDon.UseVisualStyleBackColor = true;
            radPhongDon.CheckedChanged += radPhongDon_CheckedChanged;
            // 
            // txtSoNgayO
            // 
            txtSoNgayO.Location = new Point(121, 116);
            txtSoNgayO.Name = "txtSoNgayO";
            txtSoNgayO.Size = new Size(131, 31);
            txtSoNgayO.TabIndex = 1;
            txtSoNgayO.TextChanged += txtSoNgayO_TextChanged;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Location = new Point(121, 68);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.Size = new Size(435, 31);
            txtDiaChi.TabIndex = 1;
            txtDiaChi.TextChanged += txtDiaChi_TextChanged;
            // 
            // txtHoVaTen
            // 
            txtHoVaTen.Location = new Point(121, 21);
            txtHoVaTen.Name = "txtHoVaTen";
            txtHoVaTen.Size = new Size(302, 31);
            txtHoVaTen.TabIndex = 1;
            txtHoVaTen.TextChanged += txtHoVaTen_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(18, 116);
            label4.Name = "label4";
            label4.Size = new Size(97, 25);
            label4.TabIndex = 0;
            label4.Text = "Số ngày ở:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(46, 68);
            label3.Name = "label3";
            label3.Size = new Size(69, 25);
            label3.TabIndex = 0;
            label3.Text = "Địa chỉ:";
            // 
            // LblTieuDe
            // 
            LblTieuDe.AutoSize = true;
            LblTieuDe.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblTieuDe.ForeColor = Color.Coral;
            LblTieuDe.Location = new Point(218, 27);
            LblTieuDe.Name = "LblTieuDe";
            LblTieuDe.Size = new Size(586, 38);
            LblTieuDe.TabIndex = 2;
            LblTieuDe.Text = "KHÁCH SẠN THANH THANH - TRẢ PHÒNG";
            // 
            // panel2
            // 
            panel2.Controls.Add(grbTTTK);
            panel2.Controls.Add(btnNhapMoi);
            panel2.Controls.Add(btbThoat);
            panel2.Controls.Add(btnTongKet);
            panel2.Controls.Add(btnThanhToan);
            panel2.Controls.Add(lblThanhTien);
            panel2.Controls.Add(txtThanhTien);
            panel2.Location = new Point(611, 88);
            panel2.Name = "panel2";
            panel2.Size = new Size(432, 356);
            panel2.TabIndex = 3;
            // 
            // grbTTTK
            // 
            grbTTTK.Controls.Add(lblTongSoTien);
            grbTTTK.Controls.Add(lblSoLuotNguoi);
            grbTTTK.Controls.Add(txtTongSoTien);
            grbTTTK.Controls.Add(txtSoLuotNguoi);
            grbTTTK.Location = new Point(24, 163);
            grbTTTK.Name = "grbTTTK";
            grbTTTK.Size = new Size(398, 134);
            grbTTTK.TabIndex = 2;
            grbTTTK.TabStop = false;
            grbTTTK.Text = "Thông tin tổng kết";
            // 
            // lblTongSoTien
            // 
            lblTongSoTien.AutoSize = true;
            lblTongSoTien.Location = new Point(16, 76);
            lblTongSoTien.Name = "lblTongSoTien";
            lblTongSoTien.Size = new Size(115, 25);
            lblTongSoTien.TabIndex = 0;
            lblTongSoTien.Text = "Tổng số tiền:";
            // 
            // lblSoLuotNguoi
            // 
            lblSoLuotNguoi.AutoSize = true;
            lblSoLuotNguoi.Location = new Point(16, 34);
            lblSoLuotNguoi.Name = "lblSoLuotNguoi";
            lblSoLuotNguoi.Size = new Size(122, 25);
            lblSoLuotNguoi.TabIndex = 0;
            lblSoLuotNguoi.Text = "Số lượt người";
            // 
            // txtTongSoTien
            // 
            txtTongSoTien.Location = new Point(144, 77);
            txtTongSoTien.Name = "txtTongSoTien";
            txtTongSoTien.ReadOnly = true;
            txtTongSoTien.Size = new Size(248, 31);
            txtTongSoTien.TabIndex = 1;
            // 
            // txtSoLuotNguoi
            // 
            txtSoLuotNguoi.Location = new Point(144, 34);
            txtSoLuotNguoi.Name = "txtSoLuotNguoi";
            txtSoLuotNguoi.ReadOnly = true;
            txtSoLuotNguoi.Size = new Size(248, 31);
            txtSoLuotNguoi.TabIndex = 1;
            // 
            // btnNhapMoi
            // 
            btnNhapMoi.Location = new Point(132, 12);
            btnNhapMoi.Name = "btnNhapMoi";
            btnNhapMoi.Size = new Size(112, 34);
            btnNhapMoi.TabIndex = 0;
            btnNhapMoi.Text = "Nhập mới";
            btnNhapMoi.UseVisualStyleBackColor = true;
            btnNhapMoi.Click += btnNhapMoi_Click;
            // 
            // btbThoat
            // 
            btbThoat.Location = new Point(14, 308);
            btbThoat.Name = "btbThoat";
            btbThoat.Size = new Size(112, 34);
            btbThoat.TabIndex = 0;
            btbThoat.Text = "Thoát";
            btbThoat.UseVisualStyleBackColor = true;
            btbThoat.Click += btbThoat_Click;
            // 
            // btnTongKet
            // 
            btnTongKet.Location = new Point(14, 114);
            btnTongKet.Name = "btnTongKet";
            btnTongKet.Size = new Size(112, 34);
            btnTongKet.TabIndex = 0;
            btnTongKet.Text = "Tổng kết";
            btnTongKet.UseVisualStyleBackColor = true;
            btnTongKet.Click += btnTongKet_Click;
            // 
            // btnThanhToan
            // 
            btnThanhToan.Location = new Point(14, 12);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(112, 34);
            btnThanhToan.TabIndex = 0;
            btnThanhToan.Text = "Thanh toán";
            btnThanhToan.UseVisualStyleBackColor = true;
            btnThanhToan.Click += btnThanhToan_Click;
            // 
            // lblThanhTien
            // 
            lblThanhTien.AutoSize = true;
            lblThanhTien.Location = new Point(14, 68);
            lblThanhTien.Name = "lblThanhTien";
            lblThanhTien.Size = new Size(98, 25);
            lblThanhTien.TabIndex = 0;
            lblThanhTien.Text = "Thành tiền:";
            // 
            // txtThanhTien
            // 
            txtThanhTien.Location = new Point(118, 68);
            txtThanhTien.Name = "txtThanhTien";
            txtThanhTien.ReadOnly = true;
            txtThanhTien.Size = new Size(302, 31);
            txtThanhTien.TabIndex = 1;
            // 
            // frmDangKyKS
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1045, 442);
            Controls.Add(panel2);
            Controls.Add(LblTieuDe);
            Controls.Add(panel1);
            Name = "frmDangKyKS";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmDangKyKS";
            Load += frmDangKyKS_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            grbTTTK.ResumeLayout(false);
            grbTTTK.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label LblTieuDe;
        private Label label2;
        private Label label3;
        private TextBox txtSoNgayO;
        private TextBox txtDiaChi;
        private TextBox txtHoVaTen;
        private Label label4;
        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private CheckBox chkMayNuocNong;
        private CheckBox chkInternet;
        private CheckBox chkTivi;
        private RadioButton radPhongBa;
        private RadioButton radPhongDoi;
        private RadioButton radPhongDon;
        private CheckBox chkAnSang;
        private CheckBox chkKaraoke;
        private Panel panel2;
        private Button btnThanhToan;
        private Button btnNhapMoi;
        private Label lblThanhTien;
        private TextBox txtThanhTien;
        private GroupBox grbTTTK;
        private Label lblTongSoTien;
        private Label lblSoLuotNguoi;
        private TextBox txtTongSoTien;
        private TextBox txtSoLuotNguoi;
        private Button btbThoat;
        private Button btnTongKet;
    }
}
