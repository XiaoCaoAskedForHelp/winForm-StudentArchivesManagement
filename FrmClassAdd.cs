using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 学生档案管理系统
{
    public partial class FrmClassAdd : Form
    {
        SQLHelp DB;
        DataTable dt;
        public FrmClassAdd()
        {
            InitializeComponent();
        }

        private void FrmClassAdd_Load(object sender, EventArgs e)
        {
            DB = new SQLHelp();
            //加载下拉列表框
            LoadComboxData();
        }

        void LoadComboxData()
        {
            string sql = "select distinct 院系名称 from 院系";
            cboDepatment.DisplayMember = "院系名称";
            cboDepatment.ValueMember = "院系名称";

            dt = DB.FillDataTable(sql);
            cboDepatment.DataSource = dt;

            this.cboDepatment.SelectedIndex = 0;

            loadProfession(dt.Rows[0]["院系名称"].ToString());
        }

        void loadProfession(string department)
        {
            string sql = "select * from 院系 where 院系名称=@p1";
            cboProfession.DisplayMember = "专业名称";
            cboProfession.ValueMember = "编号";

            dt = DB.FillDataTable(sql, department);
            cboProfession.DataSource = dt;
            this.cboProfession.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ClassName = this.txtClassName.Text;
            string profession = string.Empty;
            if (this.cboProfession.SelectedValue != null)
            {
                profession = this.cboProfession.SelectedValue.ToString();
            }
            string description = this.txtDescription.Text;

            if (string.IsNullOrEmpty(ClassName))
            {
                this.txtClassName.BackColor = Color.Cyan;
                this.txtClassName.Focus();
                this.lblMsg.Text = "班级名称不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(profession))
            {
                this.cboDepatment.BackColor = Color.Cyan;
                this.cboDepatment.Focus();
                this.lblMsg.Text = "请选择院系！";
                return;
            }

            string sql = "insert into 班级(院系编号,班级名称,班级描述) values(@p1,@p2,@p3)";
            DB.ExecuteNoQuery(sql, profession, ClassName, description);
            MessageBox.Show("新增班级成功");

            //清空界面数据
            this.txtClassName.Clear();
            cboDepatment.SelectedIndex = 0;
            cboProfession.SelectedIndex = 0;
            this.txtDescription.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboDepatment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string department = this.cboDepatment.SelectedValue.ToString();
            loadProfession(department);
        }
    }
}
