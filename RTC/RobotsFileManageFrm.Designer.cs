namespace RTC
{
    partial class RobotsFileManageFrm
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
            this.RobotsFileManaList = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LineCombox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BUCombox = new System.Windows.Forms.ComboBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RobotAddBtn = new System.Windows.Forms.Button();
            this.DeletRobotBtn = new System.Windows.Forms.Button();
            this.BUTxt = new System.Windows.Forms.TextBox();
            this.label111 = new System.Windows.Forms.Label();
            this.label116 = new System.Windows.Forms.Label();
            this.LineTxt = new System.Windows.Forms.TextBox();
            this.label120 = new System.Windows.Forms.Label();
            this.WorkNameTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RobotNameTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.RobotSerialTxt = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // RobotsFileManaList
            // 
            this.RobotsFileManaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RobotsFileManaList.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RobotsFileManaList.FormattingEnabled = true;
            this.RobotsFileManaList.ItemHeight = 36;
            this.RobotsFileManaList.Location = new System.Drawing.Point(0, 0);
            this.RobotsFileManaList.Name = "RobotsFileManaList";
            this.RobotsFileManaList.Size = new System.Drawing.Size(1256, 480);
            this.RobotsFileManaList.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LineCombox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BUCombox);
            this.panel1.Controls.Add(this.SearchBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1256, 100);
            this.panel1.TabIndex = 1;
            // 
            // LineCombox
            // 
            this.LineCombox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LineCombox.FormattingEnabled = true;
            this.LineCombox.Location = new System.Drawing.Point(621, 34);
            this.LineCombox.Margin = new System.Windows.Forms.Padding(4);
            this.LineCombox.Name = "LineCombox";
            this.LineCombox.Size = new System.Drawing.Size(180, 36);
            this.LineCombox.TabIndex = 135;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(159, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 133;
            this.label1.Text = "BU:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(567, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 134;
            this.label2.Text = "产线:";
            // 
            // BUCombox
            // 
            this.BUCombox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BUCombox.FormattingEnabled = true;
            this.BUCombox.Location = new System.Drawing.Point(202, 34);
            this.BUCombox.Margin = new System.Windows.Forms.Padding(4);
            this.BUCombox.Name = "BUCombox";
            this.BUCombox.Size = new System.Drawing.Size(199, 36);
            this.BUCombox.TabIndex = 131;
            // 
            // SearchBtn
            // 
            this.SearchBtn.Location = new System.Drawing.Point(903, 30);
            this.SearchBtn.Margin = new System.Windows.Forms.Padding(0);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(195, 40);
            this.SearchBtn.TabIndex = 132;
            this.SearchBtn.Text = "搜索";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.RobotsFileManaList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1256, 480);
            this.panel2.TabIndex = 2;
            // 
            // RobotAddBtn
            // 
            this.RobotAddBtn.Location = new System.Drawing.Point(457, 770);
            this.RobotAddBtn.Margin = new System.Windows.Forms.Padding(4);
            this.RobotAddBtn.Name = "RobotAddBtn";
            this.RobotAddBtn.Size = new System.Drawing.Size(134, 64);
            this.RobotAddBtn.TabIndex = 4;
            this.RobotAddBtn.Text = "添加";
            this.RobotAddBtn.UseVisualStyleBackColor = true;
            this.RobotAddBtn.Click += new System.EventHandler(this.RobotAddBtn_Click);
            // 
            // DeletRobotBtn
            // 
            this.DeletRobotBtn.Location = new System.Drawing.Point(893, 754);
            this.DeletRobotBtn.Margin = new System.Windows.Forms.Padding(4);
            this.DeletRobotBtn.Name = "DeletRobotBtn";
            this.DeletRobotBtn.Size = new System.Drawing.Size(134, 64);
            this.DeletRobotBtn.TabIndex = 5;
            this.DeletRobotBtn.Text = "删除";
            this.DeletRobotBtn.UseVisualStyleBackColor = true;
            this.DeletRobotBtn.Click += new System.EventHandler(this.DeletRobotBtn_Click);
            // 
            // BUTxt
            // 
            this.BUTxt.Location = new System.Drawing.Point(223, 702);
            this.BUTxt.Name = "BUTxt";
            this.BUTxt.Size = new System.Drawing.Size(190, 28);
            this.BUTxt.TabIndex = 15;
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Location = new System.Drawing.Point(83, 705);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(44, 18);
            this.label111.TabIndex = 14;
            this.label111.Text = "BU：";
            // 
            // label116
            // 
            this.label116.AutoSize = true;
            this.label116.Location = new System.Drawing.Point(83, 747);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(80, 18);
            this.label116.TabIndex = 10;
            this.label116.Text = "产线名：";
            // 
            // LineTxt
            // 
            this.LineTxt.Location = new System.Drawing.Point(223, 744);
            this.LineTxt.Name = "LineTxt";
            this.LineTxt.Size = new System.Drawing.Size(190, 28);
            this.LineTxt.TabIndex = 11;
            // 
            // label120
            // 
            this.label120.AutoSize = true;
            this.label120.Location = new System.Drawing.Point(83, 793);
            this.label120.Name = "label120";
            this.label120.Size = new System.Drawing.Size(80, 18);
            this.label120.TabIndex = 12;
            this.label120.Text = "工位名：";
            // 
            // WorkNameTxt
            // 
            this.WorkNameTxt.Location = new System.Drawing.Point(223, 790);
            this.WorkNameTxt.Name = "WorkNameTxt";
            this.WorkNameTxt.Size = new System.Drawing.Size(190, 28);
            this.WorkNameTxt.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(637, 589);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(3, 358);
            this.label3.TabIndex = 87;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(7, 583);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(624, 28);
            this.label4.TabIndex = 89;
            this.label4.Text = "机器人添加";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(652, 583);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(637, 28);
            this.label5.TabIndex = 90;
            this.label5.Text = "操作";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(83, 841);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 18);
            this.label6.TabIndex = 91;
            this.label6.Text = "机器人名：";
            // 
            // RobotNameTxt
            // 
            this.RobotNameTxt.Location = new System.Drawing.Point(223, 838);
            this.RobotNameTxt.Name = "RobotNameTxt";
            this.RobotNameTxt.Size = new System.Drawing.Size(190, 28);
            this.RobotNameTxt.TabIndex = 92;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(83, 886);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 18);
            this.label7.TabIndex = 93;
            this.label7.Text = "机器人序列号：";
            // 
            // RobotSerialTxt
            // 
            this.RobotSerialTxt.Location = new System.Drawing.Point(223, 883);
            this.RobotSerialTxt.Name = "RobotSerialTxt";
            this.RobotSerialTxt.Size = new System.Drawing.Size(190, 28);
            this.RobotSerialTxt.TabIndex = 94;
            // 
            // RobotsFileManageFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1256, 950);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.RobotSerialTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.RobotNameTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BUTxt);
            this.Controls.Add(this.label111);
            this.Controls.Add(this.label116);
            this.Controls.Add(this.LineTxt);
            this.Controls.Add(this.label120);
            this.Controls.Add(this.WorkNameTxt);
            this.Controls.Add(this.DeletRobotBtn);
            this.Controls.Add(this.RobotAddBtn);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RobotsFileManageFrm";
            this.Text = "RobotsFileManageFrm";
            this.Load += new System.EventHandler(this.RobotsFileManageFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox RobotsFileManaList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button RobotAddBtn;
        private System.Windows.Forms.Button DeletRobotBtn;
        private System.Windows.Forms.ComboBox LineCombox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox BUCombox;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.TextBox BUTxt;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.TextBox LineTxt;
        private System.Windows.Forms.Label label120;
        private System.Windows.Forms.TextBox WorkNameTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox RobotNameTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox RobotSerialTxt;
    }
}