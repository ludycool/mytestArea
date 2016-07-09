using Carbinet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 登陆
{
    public partial class Frm_user : Form
    {
        public Frm_user()
        {
            InitializeComponent();
            dataGridView1.DataSource = CsharpSQLiteHelper.ExecuteTable("select * from 货架表", null);
        }

        private void toolAdd9_Click(object sender, EventArgs e)
        {
            if (tb_hjID.Text == "" || tb_hjcengshu.Text == "" || tb_hjweizhi.Text == "")
            {
                MessageBox.Show("完善信息！", "警告：");
                return;
            }
            int i = CsharpSQLiteHelper.ExecuteNonQuery("insert into 货架表 values('{0}','{1}','{2}','{3}')",
                new object[] { tb_hjID.Text, tb_hjcengshu.Text, tb_hjweizhi.Text, DateTime.Now.ToString() });
            dataGridView1.DataSource = CsharpSQLiteHelper.ExecuteTable("select * from 货架表", null);
            MessageBox.Show("提交成功！", "提示：");
        }
    }
}
