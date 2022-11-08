using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 学生档案管理系统
{
    public partial class FrmStudentAdd : Form
    {
        SQLHelp DB;
        DataTable dt = new DataTable("学生信息");
        public FrmStudentAdd()
        {
            InitializeComponent();
        }

        private void FrmAddReader_Load(object sender, EventArgs e)
        {
            DB = new SQLHelp();
            LoadComboxData();
        }

        void LoadComboxData()
        {
            string sql = "select distinct 省份 from 地区";
            cboProvince.DisplayMember = "省份";
            cboProvince.ValueMember = "省份";
            cboProvince1.DisplayMember = "省份";
            cboProvince1.ValueMember = "省份";

            dt = DB.FillDataTable(sql);
            cboProvince.DataSource = dt;
            dt = DB.FillDataTable(sql);
            cboProvince1.DataSource = dt;

            this.cboProvince.SelectedIndex = 0;
            this.cboProvince1.SelectedIndex = 0;

            loadCity(dt.Rows[0]["省份"].ToString());
            loadCity1(dt.Rows[0]["省份"].ToString());

            this.cboGender.SelectedIndex =0;
        }

        void loadCity(string province)
        {
            string sql = "select * from 地区 where 省份=@p1";
            cboCity.DisplayMember = "城市";
            cboCity.ValueMember = "城市";

            dt = DB.FillDataTable(sql,province);
            cboCity.DataSource = dt;
            this.cboCity.SelectedIndex = 0;
        }

        void loadCity1(string province)
        {
            string sql = "select * from 地区 where 省份=@p1";
            cboCity1.DisplayMember = "城市";
            cboCity1.ValueMember = "城市";

            dt = DB.FillDataTable(sql, province);
            cboCity1.DataSource = dt;
            this.cboCity1.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string account = this.txtNo.Text;
            string name = this.txtName.Text;
            string password = this.txtPwd.Text;
            string gender = this.cboGender.Text;
            DateTime birthday = this.dtpBirthday.Value;
            string origin = this.cboProvince.Text +" "+ this.cboCity.Text;
            string source = this.cboProvince1.Text +" "+ this.cboCity1.Text;
            //照片
            byte[] imgBytesIn = null;
            try
            {
                string file = this.pictureBox1.ImageLocation;
                if (File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imgBytesIn = br.ReadBytes((int)fs.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (string.IsNullOrEmpty(account))
            {
                this.txtNo.BackColor = Color.Cyan;
                this.txtNo.Focus();
                this.lblMsg.Text = "账号不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                this.txtName.BackColor = Color.Cyan;
                this.txtName.Focus();
                this.lblMsg.Text = "姓名不能为空！";
                return;
            }
            if (String.IsNullOrEmpty(password))
            {
                this.txtPwd.BackColor = Color.Cyan;
                this.txtPwd.Focus();
                this.lblMsg.Text = "密码不能为空！";
                return;
            }
            if (imgBytesIn == null)
            {
                this.lblMsg.Text = "请选择本人照片！";
                return;
            }
            string sql = "insert into 学生信息(学号,姓名,密码,性别,出生年月,籍贯,生源所在地,照片) values(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";
            DB.ExecuteNoQuery(sql, account, name, password,gender,birthday,origin,source,imgBytesIn);
            MessageBox.Show("新增学生信息成功");

            //清空界面数据
            this.txtNo.Clear();
            this.txtName.Clear();
            this.txtPwd.Clear();
            cboGender.SelectedIndex = 0;
            dtpBirthday.Value = DateTime.Now;
            cboProvince.SelectedIndex = 0;
            cboProvince1.SelectedIndex = 0;
            cboCity1.SelectedIndex = 0;
            cboCity.SelectedIndex = 0;
            this.pictureBox1.Image = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string province = cboProvince.Text;
            loadCity(province);
        }
        private void cboProvince1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string province = cboProvince1.Text;
            loadCity1(province);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //OpenFileDialog打开窗口选择文件
            OpenFileDialog path = new OpenFileDialog();
            //出界面的方法
            path.ShowDialog();
            //在界面点击确定的时候出的一个文件地址。
            var file = path.FileName;
            this.pictureBox1.ImageLocation = file;
        }
    }
}
