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
    public partial class FrmStudentList : Form
    {
        SQLHelp DB;
        DataTable dt = new DataTable("学生信息");
        public FrmStudentList()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }
        private void FrmStudentList_Load(object sender, EventArgs e)
        {
            if (GlobalParameter.identity == "学生")
            {
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
            }
            DB = new SQLHelp();
            Query();
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        void Query()
        {
            string stduentNo = "%" + txtNo.Text + "%";
            string name = "%" + txtName.Text + "%";
            string sql = "select * from 学生信息 where 学号 like @p1 and 姓名 like @p2";
            dt = DB.FillDataTable(sql, stduentNo, name);

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
            string stduentNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            //打开修改页面
            FrmStudentUpdate form = new FrmStudentUpdate(stduentNo);
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
            string stduentNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //删除数据
            if (MessageBox.Show("确认要删除此学生信息吗？", "删除确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string sql = "delete from 学生信息 where 学号=@p1";
                DB.ExecuteNoQuery(sql, stduentNo);

                MessageBox.Show("删除学生信息成功");
                Query();
            }
            
        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = this.dataGridView1.CurrentRow.Index;
            try
            {
                if (dt.Rows[index]["照片"] == null) return;
                byte[] bImg = (byte[])dt.Rows[index]["照片"];
                if (bImg.Length > 100)
                {
                    MemoryStream ms = new MemoryStream(bImg);
                    this.pictureBox1.Image = Image.FromStream(ms);
                }
                else
                {
                    this.pictureBox1.Image = null;
                }
            }
            catch
            {
            }
        }
    }
}
