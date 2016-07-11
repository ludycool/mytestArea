using PetaPoco;
using SharpLite.mode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpLite
{
    public partial class MainForm : Form
    {

        Database m_instance = new Database("SQLiteDemo");
        public MainForm()
        {
            InitializeComponent();
            User us = new User();
            us.Name = "哈哈";
            us.Typess = "sdsds";
            us.IsUse = 1;
            m_instance.Insert("User", "Id", us);
            List<User> ll = m_instance.Fetch<User>("select * from User", null);




        }
    }
}
