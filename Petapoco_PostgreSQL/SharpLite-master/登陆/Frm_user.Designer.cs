namespace 登陆
{
    partial class Frm_user
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_user));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolAdd9 = new System.Windows.Forms.ToolStripButton();
            this.toolDelete = new System.Windows.Forms.ToolStripButton();
            this.toolrefesh = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox2 = new System.Windows.Forms.ToolStripComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.tb_hjcengshu = new System.Windows.Forms.TextBox();
            this.tb_hjweizhi = new System.Windows.Forms.TextBox();
            this.tb_hjID = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(6, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 154);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "货物表：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(7, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(472, 126);
            this.dataGridView1.TabIndex = 8;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAdd9,
            this.toolDelete,
            this.toolrefesh,
            this.toolStripLabel1,
            this.toolStripComboBox2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(498, 25);
            this.toolStrip1.TabIndex = 315;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolAdd9
            // 
            this.toolAdd9.Image = ((System.Drawing.Image)(resources.GetObject("toolAdd9.Image")));
            this.toolAdd9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAdd9.Name = "toolAdd9";
            this.toolAdd9.Size = new System.Drawing.Size(51, 22);
            this.toolAdd9.Tag = "3";
            this.toolAdd9.Text = "添加";
            this.toolAdd9.Click += new System.EventHandler(this.toolAdd9_Click);
            // 
            // toolDelete
            // 
            this.toolDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolDelete.Image")));
            this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDelete.Name = "toolDelete";
            this.toolDelete.Size = new System.Drawing.Size(51, 22);
            this.toolDelete.Tag = "5";
            this.toolDelete.Text = "删除";
            // 
            // toolrefesh
            // 
            this.toolrefesh.Image = ((System.Drawing.Image)(resources.GetObject("toolrefesh.Image")));
            this.toolrefesh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolrefesh.Name = "toolrefesh";
            this.toolrefesh.Size = new System.Drawing.Size(51, 22);
            this.toolrefesh.Text = "刷新";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(82, 22);
            this.toolStripLabel1.Text = "货架信息查询:";
            // 
            // toolStripComboBox2
            // 
            this.toolStripComboBox2.Items.AddRange(new object[] {
            "销售订单",
            "采购订单"});
            this.toolStripComboBox2.Name = "toolStripComboBox2";
            this.toolStripComboBox2.Size = new System.Drawing.Size(121, 25);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.comboBox1);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.button8);
            this.groupBox6.Controls.Add(this.tb_hjcengshu);
            this.groupBox6.Controls.Add(this.tb_hjweizhi);
            this.groupBox6.Controls.Add(this.tb_hjID);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Location = new System.Drawing.Point(6, 27);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(487, 71);
            this.groupBox6.TabIndex = 316;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "货架信息";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(84, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(80, 20);
            this.comboBox1.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10F);
            this.label6.Location = new System.Drawing.Point(13, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 14);
            this.label6.TabIndex = 17;
            this.label6.Text = "AGV编号：";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(308, 107);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(0, 0);
            this.button8.TabIndex = 17;
            this.button8.Text = "确定";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // tb_hjcengshu
            // 
            this.tb_hjcengshu.Location = new System.Drawing.Point(248, 10);
            this.tb_hjcengshu.Name = "tb_hjcengshu";
            this.tb_hjcengshu.Size = new System.Drawing.Size(72, 21);
            this.tb_hjcengshu.TabIndex = 7;
            // 
            // tb_hjweizhi
            // 
            this.tb_hjweizhi.Location = new System.Drawing.Point(248, 35);
            this.tb_hjweizhi.Name = "tb_hjweizhi";
            this.tb_hjweizhi.Size = new System.Drawing.Size(72, 21);
            this.tb_hjweizhi.TabIndex = 6;
            // 
            // tb_hjID
            // 
            this.tb_hjID.Location = new System.Drawing.Point(84, 10);
            this.tb_hjID.Name = "tb_hjID";
            this.tb_hjID.Size = new System.Drawing.Size(80, 21);
            this.tb_hjID.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 10F);
            this.label15.Location = new System.Drawing.Point(172, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 14);
            this.label15.TabIndex = 3;
            this.label15.Text = "货架层数:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 10F);
            this.label16.Location = new System.Drawing.Point(167, 42);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 14);
            this.label16.TabIndex = 2;
            this.label16.Text = "货架位置：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 10F);
            this.label17.Location = new System.Drawing.Point(9, 17);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 14);
            this.label17.TabIndex = 0;
            this.label17.Text = "货架编号：";
            // 
            // Frm_user
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 262);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Frm_user";
            this.Text = "Frm_user";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolAdd9;
        private System.Windows.Forms.ToolStripButton toolDelete;
        private System.Windows.Forms.ToolStripButton toolrefesh;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox tb_hjcengshu;
        private System.Windows.Forms.TextBox tb_hjweizhi;
        private System.Windows.Forms.TextBox tb_hjID;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
    }
}