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
    public partial class FrmPunishAdd : Form
    {
        SQLHelp DB;
        DataTable dt;
        public FrmPunishAdd()
        {
            InitializeComponent();
        }

        private void FrmPunishAdd_Load(object sender, EventArgs e)
        {
            DB = new SQLHelp();
            //加载下拉列表框
            LoadComboxData();
        }

        void LoadComboxData()
        {
            string sql = "select * from 惩罚类型";
            cboPunishKind.DisplayMember = "惩罚类型";
            cboPunishKind.ValueMember = "编号";

            dt = DB.FillDataTable(sql);
            cboPunishKind.DataSource = dt;

            this.cboPunishKind.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string no = this.txtNo.Text;
            string punishName = txtPunishName.Text;
            string kind = string.Empty;
            if (cboPunishKind.SelectedValue != null)
            {
                kind = cboPunishKind.SelectedValue.ToString();
            }
            string detail = this.txtDetail.Text;

            if (string.IsNullOrEmpty(no))
            {
                this.lblMsg.Text = "学号不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(punishName))
            {
                this.lblMsg.Text = "惩罚名称不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(kind))
            {
                this.lblMsg.Text = "请选择惩罚类型！";
                return;
            }
            if (string.IsNullOrEmpty(detail))
            {
                this.lblMsg.Text = "请填写惩罚详情原因！";
                return;
            }

            string sql = "insert into 惩罚记录(学号,惩罚名称,类型编号,详情原因) values(@p1,@p2,@p3,@p4)";
            DB.ExecuteNoQuery(sql, no, punishName, kind,detail);
            MessageBox.Show("新增惩罚记录成功");

            //清空界面数据
            this.txtNo.Clear();
            this.txtPunishName.Clear();
            cboPunishKind.SelectedIndex = -1;
            this.txtDetail.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
