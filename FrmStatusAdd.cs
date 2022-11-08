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
    public partial class FrmStatusAdd : Form
    {
        SQLHelp DB;
        DataTable dt;
        public FrmStatusAdd()
        {
            InitializeComponent();
        }

        private void FrmStatusAdd_Load(object sender, EventArgs e)
        {
            DB = new SQLHelp();
            //记载下拉列表框
            LoadComboxData();
        }

        void LoadComboxData()
        {
            string sql = "select distinct 院系名称 from 院系";
            cboDepartment.DisplayMember = "院系名称";
            cboDepartment.ValueMember = "院系名称";

            dt = DB.FillDataTable(sql);
            cboDepartment.DataSource = dt;
        }

        void loadProfession(string department)
        {
            string sql = "select * from 院系 where 院系名称=@p1";
            cboProfession.DisplayMember = "专业名称";
            cboProfession.ValueMember = "编号";

            dt = DB.FillDataTable(sql, department);
            cboProfession.DataSource = dt;
        }

        void loadClass(string departmentNo)
        {
            string sql = "select * from 班级 where 院系编号=@p1";
            dt = DB.FillDataTable(sql, departmentNo);
            if (dt.Rows.Count > 0)
            {
                cboClass.DisplayMember = "班级名称";
                cboClass.ValueMember = "班级编号";
                cboClass.DataSource = dt;
            }
            else
            {
                cboClass.DataSource = null;
                MessageBox.Show("此专业还未建立班级");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string StudentNo = this.txtNo.Text;
            string departmentNo = string.Empty;
            if (this.cboProfession.SelectedValue != null)
            {
                departmentNo = this.cboProfession.SelectedValue.ToString();
            }
            string classNo = string.Empty;
            if (this.cboProfession.SelectedValue != null)
            {
                classNo = this.cboClass.SelectedValue.ToString();
            }
            DateTime enrollTime = this.dtpEnroll.Value;
            string eduLength = this.cboEduLength.Text;
            string grade = this.cboGrade.Text;
            string degree=cboDegree.Text;
            string status = this.cboStatus.Text;

            string note = string.Empty;
            if (this.txtNote.Text != null)
            {
                note = this.txtNote.Text;
            }

            if (string.IsNullOrEmpty(StudentNo))
            {
                this.txtNo.BackColor = Color.Cyan;
                this.txtNo.Focus();
                this.lblMsg.Text = "请输入学号！";
                return;
            }
            if (string.IsNullOrEmpty(classNo))
            {
                this.lblMsg.Text = "请选择班级！";
                return;
            }
            if (string.IsNullOrEmpty(departmentNo))
            {
                this.lblMsg.Text = "请选择院系专业！";
                return;
            }
            if (string.IsNullOrEmpty(eduLength))
            {
                this.lblMsg.Text = "请选择学制！";
                return;
            }
            if (string.IsNullOrEmpty(grade))
            {
                this.lblMsg.Text = "请选择年级！";
                return;
            }
            if (string.IsNullOrEmpty(degree))
            {
                this.lblMsg.Text = "请选择学历！";
                return;
            }
            if (string.IsNullOrEmpty(status))
            {
                this.lblMsg.Text = "请选择学籍状态！";
                return;
            }

            string sql = "insert into 学籍信息(学号,院系编号,班级编号,入学年份,学制,年级,学历,学籍状态,备注) " +
                "values(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)";
            
            DB.ExecuteNoQuery(sql, StudentNo, departmentNo, classNo, enrollTime, eduLength, grade,degree, status, note);
            MessageBox.Show("新增学籍信息成功");

            //清空界面数据
            this.txtNo.Clear();
            this.cboDepartment.SelectedIndex = 0;
            this.cboProfession.SelectedIndex = 0;
            this.cboClass.SelectedIndex = 0;
            this.dtpEnroll.Value = DateTime.Now;
            this.cboEduLength.SelectedIndex = -1;
            this.cboGrade.SelectedIndex = -1;
            this.cboStatus.SelectedIndex = -1;
            this.cboDegree.SelectedIndex = -1;
            this.txtNote.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDepartment.SelectedValue != null)
            {
                string department = this.cboDepartment.SelectedValue.ToString();
                loadProfession(department);
            }
        }

        private void cboProfession_SelectedIndexChanged(object sender, EventArgs e)
        {
            string departmentNo = string.Empty;
            if (this.cboProfession.SelectedValue != null)
            {
                departmentNo = this.cboProfession.SelectedValue.ToString();
            }
            loadClass(departmentNo);
        }
    }
}
