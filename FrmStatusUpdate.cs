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
    public partial class FrmStutasUpdate : Form
    {
        SQLHelp DB;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        public readonly string studentNo;
        string no, eduLength_old, grade_old, degree_old, status_old, department_old, profession_old,class_old;
        DateTime enroll_old;
        public FrmStutasUpdate(string studentNo)
        {
            InitializeComponent();
            this.studentNo = studentNo;
        }

        private void FrmStutasUpdate_Load(object sender, EventArgs e)
        {
            DB = new SQLHelp();
            //记载下拉列表框
            LoadComboxData();

            //查询学籍信息
            string sql = "select * from 学籍信息 where 学号=@p1";
            dt = DB.FillDataTable(sql, studentNo);
            if (dt.Rows.Count > 0)
            {
                no = dt.Rows[0]["学号"].ToString();
                enroll_old = (DateTime)dt.Rows[0]["入学年份"];
                eduLength_old = dt.Rows[0]["学制"].ToString();
                grade_old = dt.Rows[0]["年级"].ToString();
                degree_old = dt.Rows[0]["学历"].ToString();
                status_old = dt.Rows[0]["学籍状态"].ToString();
                class_old = dt.Rows[0]["班级编号"].ToString();
                txtNo.Text = no;
                dtpEnroll.Value = enroll_old;
                cboEduLength.Text = eduLength_old;
                cboGrade.Text = grade_old;
                cboDegree.Text = degree_old;
                cboStatus.Text = status_old;
                if (dt.Rows[0]["备注"] != null)
                {
                    txtNote.Text = dt.Rows[0]["备注"].ToString();
                }
                sql = "select * from 院系 where 编号=@p1";
                dt = DB.FillDataTable(sql, (int)dt.Rows[0]["院系编号"]);
                dt1 = dt;
                department_old = dt1.Rows[0]["院系名称"].ToString();
                profession_old = dt1.Rows[0]["编号"].ToString();
                cboDepartment.SelectedValue = dt1.Rows[0]["院系名称"];
                cboProfession.SelectedValue = dt1.Rows[0]["编号"];
                cboClass.SelectedValue = class_old;
            }
            else
            {
                MessageBox.Show("未查询对应的学籍信息");
                this.Close();
            }
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
            string degree = cboDegree.Text;
            string status = this.cboStatus.Text;
            string detail = txtReason.Text;

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
            if (string.IsNullOrEmpty(detail))
            {
                this.lblMsg.Text = "填写详情原因！";
                return;
            }
            try
            {
                DB.BeginTransaction();
                string sql = "update 学籍信息 set 院系编号=@p1,班级编号=@p2,入学年份=@p3,学制=@p4,年级=@p5,学历=@p6,学籍状态=@p7,备注=@p8 where 学号=@p9";
                DB.ExecuteNoQuery(sql, departmentNo, classNo, enrollTime, eduLength, grade, degree, status, note, StudentNo);
                sql = "insert into 学籍变动记录(学号,原院系编号,原班级编号,原年级,原学历,原学籍状态,详情原因)" +
                    "values(@p1,@p2,@p3,@p4,@p5,@p6,@p7)";
                DB.ExecuteNoQuery(sql, no, profession_old, class_old, grade_old, degree_old, status_old, detail);
                DB.Commit();
                MessageBox.Show("修改学籍信息成功");
            }
            catch(Exception ex)
            {
                DB.Rollback();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void cboDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string department = this.cboDepartment.SelectedValue.ToString();
            loadProfession(department);
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
