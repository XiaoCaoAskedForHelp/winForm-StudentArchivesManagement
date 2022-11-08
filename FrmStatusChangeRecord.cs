using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 学生档案管理系统.CommonLib;

namespace 学生档案管理系统
{
    public partial class FrmStutasChangeRecord : Form
    {
        SQLHelp DB;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        public FrmStutasChangeRecord()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }
        private void FrmStutasChangeRecord_Load(object sender, EventArgs e)
        {
            if (GlobalParameter.identity == "学生")
            {
                btnDelete.Visible = false;
            }
            DB = new SQLHelp();
            LoadComboxData();
            Query();
        }

        void LoadComboxData()
        {
            string sql = "select null as 院系名称 union select distinct 院系名称 from 院系";
            cboDepartment.DisplayMember = "院系名称";
            cboDepartment.ValueMember = "院系名称";

            dt1 = DB.FillDataTable(sql);
            cboDepartment.DataSource = dt1;
        }

        void loadProfession(string department)
        {
            string sql = "select null as 编号,null,'请选择' as 专业名称 union select * from 院系 where 院系名称=@p1";
            cboProfession.DisplayMember = "专业名称";
            cboProfession.ValueMember = "编号";

            dt1 = DB.FillDataTable(sql, department);
            cboProfession.DataSource = dt1;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        void Query()
        {
            string studentNo = "%" + txtNo.Text + "%";
            string name = "%" + txtName.Text + "%";
            string department = string.Empty;
            string profession = string.Empty;
            if (cboDepartment.SelectedValue != null)
            {
                department = "%" + cboDepartment.SelectedValue.ToString() + "%";
            }
            if (cboProfession.SelectedValue != null)
            {
                profession = "%" + cboProfession.SelectedValue.ToString() + "%";
            }
            string sql = "select 学籍变动记录.*,学生信息.姓名,院系.院系名称 as 原院系名称,院系.专业名称 as 原专业名称,班级.班级名称 as 原班级名称 from 学籍变动记录,学生信息,院系,班级 " +
                "where 学籍变动记录.学号=学生信息.学号 and 学籍变动记录.原院系编号=院系.编号 and 学籍变动记录.原班级编号=班级.班级编号 " +
                "and 学籍变动记录.学号 like @p1 and 学生信息.姓名 like @p2 and 院系.院系名称 like @p3 and 院系.编号 like @p4";
            dt = DB.FillDataTable(sql, studentNo, name, department, profession);

            dataGridView1.DataSource = dt;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index < 0)
            {
                return;
            }
            string studentNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            //删除数据
            if (MessageBox.Show("确认要删除此学生学籍变动信息吗？", "删除确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "delete from 学籍变动记录 where 学号=@p1";
                DB.ExecuteNoQuery(sql, studentNo);

                MessageBox.Show("删除学籍变动信息成功");
                Query();
            }
            
        }

        private void cboDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDepartment.SelectedValue != null)
            {
                string department = this.cboDepartment.SelectedValue.ToString();
                loadProfession(department);
            }
        }
    }
}
