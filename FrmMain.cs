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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            FrmLogin login = new FrmLogin();

            if (login.ShowDialog() != DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                this.toolStripStatusLabel1.Text = "用户:" + GlobalParameter.name;
            }

            if (GlobalParameter.identity == "学生")
            {
                新增学生ToolStripMenuItem.Visible = false;
                新增班级ToolStripMenuItem.Visible = false;
                新增学籍信息ToolStripMenuItem.Visible = false;
                新增奖励ToolStripMenuItem.Visible = false;
                新增惩罚ToolStripMenuItem.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel2.Text = "系统当前时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        private void 新增学生ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStudentAdd form = new FrmStudentAdd();
            form.ShowDialog();
        }

        private void 学生列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStudentList form = new FrmStudentList();
            form.MdiParent = this;
            form.Show();
        }

        private void 新增班级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClassAdd form = new FrmClassAdd();
            form.ShowDialog();
        }

        private void 班级列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClassList form = new FrmClassList();
            form.MdiParent = this;
            form.Show();
        }

        private void 新增学籍信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStatusAdd form = new FrmStatusAdd();
            form.ShowDialog();
        }

        private void 学籍信息列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStutasList form = new FrmStutasList();
            form.MdiParent = this;
            form.Show();
        }

        private void 学籍变动记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStutasChangeRecord form = new FrmStutasChangeRecord();
            form.MdiParent = this;
            form.Show();
        }

        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("真的要退出系统么？", "学生档案管理系统", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void 新增奖励ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRewardAdd form = new FrmRewardAdd();
            form.ShowDialog();
        }

        private void 新增惩罚ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPunishAdd form = new FrmPunishAdd();
            form.ShowDialog();
        }

        private void 奖励记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRewardList form = new FrmRewardList();
            form.MdiParent = this;
            form.Show();
        }

        private void 惩罚记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPunishList form = new FrmPunishList();
            form.MdiParent = this;
            form.Show();
        }
    }
}
