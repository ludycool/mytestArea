using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.StudentService;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StudentService.StudentServiceClient client = new WindowsFormsApplication1.StudentService.StudentServiceClient("WSHttpBinding_IStudentService");
            IEnumerable<StudentInfo> x = client.GetStudentInfo(Int32.Parse(richTextBox1.Text));
            dataGridView1.DataSource = x;

        }
    }
}
