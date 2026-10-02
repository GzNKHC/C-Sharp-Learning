namespace Lab04c_BTVN
{
    public partial class fmMTBT : Form
    {
        double SoThuNhat = 0;
        string PhepTinh = "";
        bool NhapSoMoi = false;
        public fmMTBT()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void fmMTBT_Load(object sender, EventArgs e)
        {

        }

        private void btnSo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (txtInput.Text == "0" || NhapSoMoi)
            {
                txtInput.Text = btn.Text;
                NhapSoMoi = false;
            }
            else
            {
                txtInput.Text += btn.Text;
            }
        }

        private void buttonPlus_Click(object sender, EventArgs e)
        {
            SoThuNhat = double.Parse(txtInput.Text);
            PhepTinh = "+";
            NhapSoMoi = true;
        }

        private void buttonMinus_Click(object sender, EventArgs e)
        {
            SoThuNhat = double.Parse(txtInput.Text);
            PhepTinh = "-";
            NhapSoMoi = true;
        }

        private void buttonTime_Click(object sender, EventArgs e)
        {
            SoThuNhat = double.Parse(txtInput.Text);
            PhepTinh = "*";
            NhapSoMoi = true;
        }

        private void buttonDivide_Click(object sender, EventArgs e)
        {
            SoThuNhat = double.Parse(txtInput.Text);
            PhepTinh = "/";
            NhapSoMoi = true;
        }

        private void buttonEqual_Click(object sender, EventArgs e)
        {
            double soThuHai = double.Parse(txtInput.Text);
            double ketQua = 0;

            switch (PhepTinh)
            {
                case "+":
                    ketQua = SoThuNhat + soThuHai;
                    break;

                case "-":
                    ketQua = SoThuNhat - soThuHai;
                    break;

                case "*":
                    ketQua = SoThuNhat * soThuHai;
                    break;

                case "/":
                    if (soThuHai == 0)
                    {
                        MessageBox.Show("Không thể chia cho 0!");
                        return;
                    }

                    ketQua = SoThuNhat / soThuHai;
                    break;
            }

            txtInput.Text = ketQua.ToString();
        }

        private void buttonC_Click(object sender, EventArgs e)
        {
            txtInput.Text = "0";
            SoThuNhat = 0;
            PhepTinh = "";
            NhapSoMoi = false;
        }
    }
}
