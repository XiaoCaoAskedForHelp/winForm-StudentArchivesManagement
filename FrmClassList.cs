using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 学生档案管理系统.CommonLib;

namespace 学生档案管理系统
{
    public partial class FrmClassList : Form
    {
        SQLHelp DB;
        DataTable dt;
        public FrmClassList()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }
        private void FrmClassList_Load(object sender, EventArgs e)
        {
            if (GlobalParameter.identity == "学生")
            {
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
            }
            DB = new SQLHelp();
            LoadComboxData();
            Query();
        }

        void LoadComboxData()
        {
            string sql = "select distinct 院系名称 from 院系";
            cboDepatment.DisplayMember = "院系名称";
            cboDepatment.ValueMember = "院系名称";

            dt = DB.FillDataTable(sql);
            cboDepatment.DataSource = dt;
            cboDepatment.SelectedIndex = -1;
        }

        void loadProfession(string department)
        {
            if (!string.IsNullOrEmpty(department))
            {
                string sql = "select null as 编号,null,'请选择' as 专业名称 union select * from 院系 where 院系名称=@p1";
                cboProfession.DisplayMember = "专业名称";
                cboProfession.ValueMember = "编号";

                dt = DB.FillDataTable(sql, department);
                cboProfession.DataSource = dt;
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        void Query()
        {
            string className = "%" + txtName.Text + "%";
            string department = "%";
            string profession = "%";
            if (cboDepatment.SelectedValue != null)
            {
                department = "%" + cboDepatment.SelectedValue.ToString() + "%";
            }
            if (cboProfession.SelectedValue != null)
            {
                profession = "%" + cboProfession.SelectedValue.ToString() + "%";
            }
            string sql = "select 班级.*,院系.院系名称,院系.专业名称 from 班级,院系 " +
                "where 班级.院系编号=院系.编号 and 班级.班级名称 like @p1 and 院系.院系名称 like @p2 and 班级.院系编号 like @p3";
            dt = DB.FillDataTable(sql, className, department,profession);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index < 0)
            {
                return;
            }
            int classNo = (int)dataGridView1.CurrentRow.Cells[0].Value;

            //打开修改页面
            FrmClassUpdate form = new FrmClassUpdate(classNo);
            DialogResult dr = form.ShowDialog();
            if(dr == DialogResult.OK)
            {
                Query();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index < 0)
            {
                return;
            }
            int classNo = (int)dataGridView1.CurrentRow.Cells[0].Value;

            //删除数据
            if (MessageBox.Show("确认要删除此班级信息吗？", "删除确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "delete from 班级 where 班级编号=@p1";
                DB.ExecuteNoQuery(sql, classNo);

                MessageBox.Show("删除班级信息成功");
                Query();
            }
            
        }

        private void cboDepatment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDepatment.SelectedValue != null)
            {
                string department = this.cboDepatment.SelectedValue.ToString();
                loadProfession(department);
            }
        }
    }
}
