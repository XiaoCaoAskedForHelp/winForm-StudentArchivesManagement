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
    public partial class FrmClassUpdate : Form
    {
        SQLHelp DB;
        DataTable dt = new DataTable();
        public readonly int classNo;
        public FrmClassUpdate(int classNo)
        {
            InitializeComponent();
            this.classNo = classNo;
        }

        private void FrmClassUpdate_Load(object sender, EventArgs e)
        {
            DB = new SQLHelp();
            //记载下拉列表框
            LoadComboxData();

            string sql = "select * from 班级 where 班级编号=@p1";
            dt = DB.FillDataTable(sql, classNo);
            if (dt.Rows.Count > 0)
            {
                txtClassNo.Text = dt.Rows[0]["班级编号"].ToString();
                txtClassName.Text = dt.Rows[0]["班级名称"].ToString();
                int departmentNo = (int)dt.Rows[0]["院系编号"];
                if (dt.Rows[0]["班级描述"] != null)
                {
                    txtDescription.Text = dt.Rows[0]["班级描述"].ToString();
                }
                sql = "select * from 院系 where 编号=@p1";
                dt = DB.FillDataTable(sql, departmentNo);
                cboDepatment.SelectedValue = dt.Rows[0]["院系名称"].ToString();
                cboProfession.SelectedValue = departmentNo;
            }
            else
            {
                MessageBox.Show("未查询此班级的信息");
                this.Close();
            }
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

            string sql = "update 班级 set 班级名称=@p1,院系编号=@p2,班级描述=@p3 where 班级编号=@p4";
            DB.ExecuteNoQuery(sql,ClassName,profession,description,classNo);
            MessageBox.Show("修改班级信息成功");

            DialogResult = DialogResult.OK;
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
