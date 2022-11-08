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
    public partial class FrmRewardList : Form
    {
        SQLHelp DB;
        DataTable dt;
        public FrmRewardList()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }
        private void FrmRewardList_Load(object sender, EventArgs e)
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
            string sql = "select null as 编号,'请选择' as 奖励类型 union select * from 奖励类型";
            cboKind.DisplayMember = "奖励类型";
            cboKind.ValueMember = "编号";

            dt = DB.FillDataTable(sql);
            cboKind.DataSource = dt;
            cboKind.SelectedIndex = 0;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        void Query()
        {
            string no = "%" + txtNo.Text + "%";
            string kind = "%";
            if (cboKind.SelectedValue != null)
            {
                kind = string.IsNullOrEmpty( cboKind.SelectedValue.ToString())?"%": cboKind.SelectedValue.ToString();
            }
            string sql = "select 奖励记录.*,奖励类型.奖励类型,学生信息.姓名 from 奖励记录,学生信息,奖励类型 " +
                "where 奖励记录.学号=学生信息.学号 and 奖励记录.类型编号=奖励类型.编号 and 奖励记录.学号 like @p1 and 奖励记录.类型编号 like @p2";
            dt = DB.FillDataTable(sql, no,kind);

            dataGridView1.DataSource = null;
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
            string No = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            //删除数据
            if (MessageBox.Show("确认要删除此奖励记录吗？", "删除确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "delete from 奖励记录 where 学号=@p1";
                DB.ExecuteNoQuery(sql, No);

                MessageBox.Show("删除此奖励记录成功");
                Query();
            }
            
        }
    }
}
