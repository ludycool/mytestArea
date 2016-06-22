using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WCFServiceHostInConsole;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StudentServiceClient client = new StudentServiceClient();

            IEnumerable<StudentInfo> x = client.GetStudentInfo(Int32.Parse(textBox1.Text));
            dataGridView1.DataSource = x;
        }
    }
}
