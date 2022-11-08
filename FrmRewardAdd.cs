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
    public partial class FrmRewardAdd : Form
    {
        SQLHelp DB;
        DataTable dt;
        public FrmRewardAdd()
        {
            InitializeComponent();
        }

        private void FrmRewardAdd_Load(object sender, EventArgs e)
        {
            DB = new SQLHelp();
            //加载下拉列表框
            LoadComboxData();
        }

        void LoadComboxData()
        {
            string sql = "select * from 奖励类型";
            cboRewardKind.DisplayMember = "奖励类型";
            cboRewardKind.ValueMember = "编号";

            dt = DB.FillDataTable(sql);
            cboRewardKind.DataSource = dt;

            this.cboRewardKind.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string no = this.txtNo.Text;
            string rewardName = txtRewardName.Text;
            string kind = string.Empty;
            if (cboRewardKind.SelectedValue != null)
            {
                kind = cboRewardKind.SelectedValue.ToString();
            }
            string detail = this.txtDetail.Text;

            if (string.IsNullOrEmpty(no))
            {
                this.lblMsg.Text = "学号不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(rewardName))
            {
                this.lblMsg.Text = "奖励名称不能为空！";
                return;
            }
            if (string.IsNullOrEmpty(kind))
            {
                this.lblMsg.Text = "请选择奖励类型！";
                return;
            }
            if (string.IsNullOrEmpty(detail))
            {
                this.lblMsg.Text = "请填写奖励详情原因！";
                return;
            }

            string sql = "insert into 奖励记录(学号,奖励名称,类型编号,详情原因) values(@p1,@p2,@p3,@p4)";
            DB.ExecuteNoQuery(sql, no, rewardName, kind,detail);
            MessageBox.Show("新增奖励记录成功");

            //清空界面数据
            this.txtNo.Clear();
            this.txtRewardName.Clear();
            cboRewardKind.SelectedIndex = -1;
            this.txtDetail.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
