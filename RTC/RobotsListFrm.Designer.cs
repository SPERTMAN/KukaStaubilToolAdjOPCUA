namespace RTC
{
    partial class RobotsListFrm
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
            this.RobotsFileList = new System.Windows.Forms.ListBox();
            this.ChangeOverRobotBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LineCombox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BUCombox = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RobotsFileList
            // 
            this.RobotsFileList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RobotsFileList.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RobotsFileList.FormattingEnabled = true;
            this.RobotsFileList.ItemHeight = 33;
            this.RobotsFileList.Location = new System.Drawing.Point(0, 69);
            this.RobotsFileList.Margin = new System.Windows.Forms.Padding(4);
            this.RobotsFileList.Name = "RobotsFileList";
            this.RobotsFileList.Size = new System.Drawing.Size(794, 363);
            this.RobotsFileList.TabIndex = 0;
            // 
            // ChangeOverRobotBtn
            // 
            this.ChangeOverRobotBtn.Location = new System.Drawing.Point(339, 453);
            this.ChangeOverRobotBtn.Margin = new System.Windows.Forms.Padding(4);
            this.ChangeOverRobotBtn.Name = "ChangeOverRobotBtn";
            this.ChangeOverRobotBtn.Size = new System.Drawing.Size(134, 64);
            this.ChangeOverRobotBtn.TabIndex = 1;
            this.ChangeOverRobotBtn.Text = "确认更换";
            this.ChangeOverRobotBtn.UseVisualStyleBackColor = true;
            this.ChangeOverRobotBtn.Click += new System.EventHandler(this.ChangeOverRobotBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LineCombox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BUCombox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 71);
            this.panel1.TabIndex = 2;
            // 
            // LineCombox
            // 
            this.LineCombox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LineCombox.FormattingEnabled = true;
            this.LineCombox.Location = new System.Drawing.Point(538, 17);
            this.LineCombox.Margin = new System.Windows.Forms.Padding(4);
            this.LineCombox.Name = "LineCombox";
            this.LineCombox.Size = new System.Drawing.Size(180, 36);
            this.LineCombox.TabIndex = 139;
            this.LineCombox.SelectedIndexChanged += new System.EventHandler(this.LineCombox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 137;
            this.label1.Text = "BU:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(484, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 138;
            this.label2.Text = "产线:";
            // 
            // BUCombox
            // 
            this.BUCombox.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BUCombox.FormattingEnabled = true;
            this.BUCombox.Location = new System.Drawing.Point(119, 17);
            this.BUCombox.Margin = new System.Windows.Forms.Padding(4);
            this.BUCombox.Name = "BUCombox";
            this.BUCombox.Size = new System.Drawing.Size(199, 36);
            this.BUCombox.TabIndex = 136;
            this.BUCombox.SelectedIndexChanged += new System.EventHandler(this.BUCombox_SelectedIndexChanged);
            // 
            // RobotsListFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 545);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ChangeOverRobotBtn);
            this.Controls.Add(this.RobotsFileList);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RobotsListFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RobotsListFrm";
            this.Load += new System.EventHandler(this.RobotsListFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox RobotsFileList;
        private System.Windows.Forms.Button ChangeOverRobotBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox LineCombox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox BUCombox;
    }
}