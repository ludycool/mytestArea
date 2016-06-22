using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WCFServiceGeneratedByConfig;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Action action = new Action(Bind);
            action.BeginInvoke(new AsyncCallback((iar) =>
            {
                Action actionEnd = (Action)iar.AsyncState;
                actionEnd.EndInvoke(iar);

            }), action);
        }

        private void Bind()
        {
            StudentServiceClient client = new StudentServiceClient();
            IEnumerable<StudentInfo> x = client.GetStudentInfo(Int32.Parse(textBox1.Text));
            dataGridView1.Invoke((Action)(() => { dataGridView1.DataSource = x; }));
        }
    }
}
