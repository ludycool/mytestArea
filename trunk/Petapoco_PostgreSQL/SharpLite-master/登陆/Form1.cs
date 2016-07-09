using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 登陆
{
    public partial class Form1 : Form
    {
        bool bl = false;////////////

        public Form1()
        {
            InitializeComponent();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (names.Text.Trim() == "")
            {
                MessageBox.Show("用户名不能为空！","提示信息");
                names.Focus();
                return;
            }
            if (pwd.Text.Trim() == "")
            {
                MessageBox.Show("密码不能为空！", "提示信息");
                pwd.Focus();
                return;
            }
            int i =1;
            //SqlConnection con = new SqlConnection("server=.;database=Data;uid=sa;pwd=sa123");
            //using (con)
            //{
            //    con.Open();
            //    string sql = "select count(*) from tb where names='" + names.Text.Trim() + "' and pwd='" + pwd.Text.Trim() + "'";
            //    using (SqlCommand cmd = con.CreateCommand())
            //    {
            //        cmd.CommandText = sql;
            //        i = (int)cmd.ExecuteScalar();
            //    }
            //}
            if (i > 0)
            {
                bl = true;
                MessageBox.Show("登陆成功", "提示信息");
                MDIParent1.TxtUser = names.Text.Trim();
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！请重新登陆！", "提示信息");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!bl)
            {
                Application.Exit();
            }
        }
    }
}
