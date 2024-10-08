using System;

namespace RTC
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LoginBtn = new System.Windows.Forms.Button();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.RobotDataBtn = new System.Windows.Forms.Button();
            this.AnalyDataBtn = new System.Windows.Forms.Button();
            this.HistoryFrmBtn = new System.Windows.Forms.Button();
            this.MainFrmBtn = new System.Windows.Forms.Button();
            this.ChildFrmPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SensorYLab = new System.Windows.Forms.Label();
            this.y = new System.Windows.Forms.Label();
            this.OpcuaConLab = new System.Windows.Forms.Label();
            this.SensorXLab = new System.Windows.Forms.Label();
            this.ReadyLab = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.SysStauLab = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DataStatuLab = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.RobotNameLab = new System.Windows.Forms.Label();
            this.RobotSWBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.AutoExitLogTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(226, 80);
            this.label7.TabIndex = 60;
            this.label7.Text = "RTC";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.RobotDataBtn);
            this.panel1.Controls.Add(this.AnalyDataBtn);
            this.panel1.Controls.Add(this.HistoryFrmBtn);
            this.panel1.Controls.Add(this.MainFrmBtn);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 1030);
            this.panel1.TabIndex = 64;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.LoginBtn);
            this.panel6.Controls.Add(this.ExitBtn);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 985);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(226, 45);
            this.panel6.TabIndex = 65;
            // 
            // LoginBtn
            // 
            this.LoginBtn.BackColor = System.Drawing.Color.White;
            this.LoginBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LoginBtn.BackgroundImage")));
            this.LoginBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.LoginBtn.FlatAppearance.BorderSize = 0;
            this.LoginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginBtn.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoginBtn.Location = new System.Drawing.Point(123, 3);
            this.LoginBtn.Name = "LoginBtn";
            this.LoginBtn.Size = new System.Drawing.Size(104, 40);
            this.LoginBtn.TabIndex = 64;
            this.LoginBtn.UseVisualStyleBackColor = false;
            this.LoginBtn.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // ExitBtn
            // 
            this.ExitBtn.BackColor = System.Drawing.Color.Transparent;
            this.ExitBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ExitBtn.BackgroundImage")));
            this.ExitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExitBtn.FlatAppearance.BorderSize = 0;
            this.ExitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitBtn.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExitBtn.Location = new System.Drawing.Point(3, 0);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(92, 40);
            this.ExitBtn.TabIndex = 61;
            this.ExitBtn.UseVisualStyleBackColor = false;
            this.ExitBtn.Click += new System.EventHandler(this.ExitBtn_Click_1);
            // 
            // RobotDataBtn
            // 
            this.RobotDataBtn.BackColor = System.Drawing.SystemColors.Window;
            this.RobotDataBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.RobotDataBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.RobotDataBtn.FlatAppearance.BorderSize = 0;
            this.RobotDataBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RobotDataBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RobotDataBtn.Image = ((System.Drawing.Image)(resources.GetObject("RobotDataBtn.Image")));
            this.RobotDataBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RobotDataBtn.Location = new System.Drawing.Point(0, 354);
            this.RobotDataBtn.Name = "RobotDataBtn";
            this.RobotDataBtn.Size = new System.Drawing.Size(226, 96);
            this.RobotDataBtn.TabIndex = 63;
            this.RobotDataBtn.Text = " 机器人管理";
            this.RobotDataBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RobotDataBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.RobotDataBtn.UseVisualStyleBackColor = false;
            this.RobotDataBtn.Click += new System.EventHandler(this.button4_Click);
            // 
            // AnalyDataBtn
            // 
            this.AnalyDataBtn.BackColor = System.Drawing.SystemColors.Window;
            this.AnalyDataBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AnalyDataBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.AnalyDataBtn.FlatAppearance.BorderSize = 0;
            this.AnalyDataBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AnalyDataBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AnalyDataBtn.Image = ((System.Drawing.Image)(resources.GetObject("AnalyDataBtn.Image")));
            this.AnalyDataBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AnalyDataBtn.Location = new System.Drawing.Point(0, 262);
            this.AnalyDataBtn.Name = "AnalyDataBtn";
            this.AnalyDataBtn.Size = new System.Drawing.Size(226, 92);
            this.AnalyDataBtn.TabIndex = 62;
            this.AnalyDataBtn.Text = " 参数编辑";
            this.AnalyDataBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AnalyDataBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.AnalyDataBtn.UseVisualStyleBackColor = false;
            this.AnalyDataBtn.Click += new System.EventHandler(this.AnalyDataBtn_Click);
            // 
            // HistoryFrmBtn
            // 
            this.HistoryFrmBtn.BackColor = System.Drawing.SystemColors.Window;
            this.HistoryFrmBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.HistoryFrmBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.HistoryFrmBtn.FlatAppearance.BorderSize = 0;
            this.HistoryFrmBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HistoryFrmBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HistoryFrmBtn.Image = ((System.Drawing.Image)(resources.GetObject("HistoryFrmBtn.Image")));
            this.HistoryFrmBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.HistoryFrmBtn.Location = new System.Drawing.Point(0, 170);
            this.HistoryFrmBtn.Name = "HistoryFrmBtn";
            this.HistoryFrmBtn.Size = new System.Drawing.Size(226, 92);
            this.HistoryFrmBtn.TabIndex = 61;
            this.HistoryFrmBtn.Text = " 历史界面";
            this.HistoryFrmBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.HistoryFrmBtn.UseVisualStyleBackColor = false;
            this.HistoryFrmBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainFrmBtn
            // 
            this.MainFrmBtn.BackColor = System.Drawing.SystemColors.Window;
            this.MainFrmBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainFrmBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainFrmBtn.FlatAppearance.BorderSize = 0;
            this.MainFrmBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainFrmBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainFrmBtn.Image = ((System.Drawing.Image)(resources.GetObject("MainFrmBtn.Image")));
            this.MainFrmBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MainFrmBtn.Location = new System.Drawing.Point(0, 80);
            this.MainFrmBtn.Name = "MainFrmBtn";
            this.MainFrmBtn.Size = new System.Drawing.Size(226, 90);
            this.MainFrmBtn.TabIndex = 0;
            this.MainFrmBtn.Text = " 主界面";
            this.MainFrmBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.MainFrmBtn.UseVisualStyleBackColor = false;
            this.MainFrmBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChildFrmPanel
            // 
            this.ChildFrmPanel.BackColor = System.Drawing.Color.Transparent;
            this.ChildFrmPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildFrmPanel.Location = new System.Drawing.Point(0, 0);
            this.ChildFrmPanel.Name = "ChildFrmPanel";
            this.ChildFrmPanel.Size = new System.Drawing.Size(1282, 906);
            this.ChildFrmPanel.TabIndex = 65;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.SensorYLab);
            this.panel2.Controls.Add(this.y);
            this.panel2.Controls.Add(this.OpcuaConLab);
            this.panel2.Controls.Add(this.SensorXLab);
            this.panel2.Controls.Add(this.ReadyLab);
            this.panel2.Controls.Add(this.label55);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label54);
            this.panel2.Controls.Add(this.SysStauLab);
            this.panel2.Controls.Add(this.label48);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.DataStatuLab);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 906);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1282, 44);
            this.panel2.TabIndex = 1;
            // 
            // SensorYLab
            // 
            this.SensorYLab.AutoSize = true;
            this.SensorYLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SensorYLab.ForeColor = System.Drawing.Color.Red;
            this.SensorYLab.Location = new System.Drawing.Point(1228, 9);
            this.SensorYLab.Name = "SensorYLab";
            this.SensorYLab.Size = new System.Drawing.Size(34, 24);
            this.SensorYLab.TabIndex = 91;
            this.SensorYLab.Text = "●";
            // 
            // y
            // 
            this.y.AutoSize = true;
            this.y.Location = new System.Drawing.Point(1188, 12);
            this.y.Name = "y";
            this.y.Size = new System.Drawing.Size(17, 18);
            this.y.TabIndex = 90;
            this.y.Text = "Y";
            // 
            // OpcuaConLab
            // 
            this.OpcuaConLab.AutoSize = true;
            this.OpcuaConLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpcuaConLab.ForeColor = System.Drawing.Color.Red;
            this.OpcuaConLab.Location = new System.Drawing.Point(460, 12);
            this.OpcuaConLab.Name = "OpcuaConLab";
            this.OpcuaConLab.Size = new System.Drawing.Size(34, 24);
            this.OpcuaConLab.TabIndex = 92;
            this.OpcuaConLab.Text = "●";
            // 
            // SensorXLab
            // 
            this.SensorXLab.AutoSize = true;
            this.SensorXLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SensorXLab.ForeColor = System.Drawing.Color.Red;
            this.SensorXLab.Location = new System.Drawing.Point(1128, 10);
            this.SensorXLab.Name = "SensorXLab";
            this.SensorXLab.Size = new System.Drawing.Size(34, 24);
            this.SensorXLab.TabIndex = 89;
            this.SensorXLab.Text = "●";
            // 
            // ReadyLab
            // 
            this.ReadyLab.AutoSize = true;
            this.ReadyLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ReadyLab.ForeColor = System.Drawing.Color.Red;
            this.ReadyLab.Location = new System.Drawing.Point(240, 10);
            this.ReadyLab.Name = "ReadyLab";
            this.ReadyLab.Size = new System.Drawing.Size(34, 24);
            this.ReadyLab.TabIndex = 70;
            this.ReadyLab.Text = "●";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(1088, 12);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(17, 18);
            this.label55.TabIndex = 88;
            this.label55.Text = "X";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 18);
            this.label10.TabIndex = 67;
            this.label10.Text = "SYSTEM Status";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(188, 12);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(53, 18);
            this.label54.TabIndex = 69;
            this.label54.Text = "Ready";
            // 
            // SysStauLab
            // 
            this.SysStauLab.AutoSize = true;
            this.SysStauLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SysStauLab.ForeColor = System.Drawing.Color.Red;
            this.SysStauLab.Location = new System.Drawing.Point(126, 10);
            this.SysStauLab.Name = "SysStauLab";
            this.SysStauLab.Size = new System.Drawing.Size(34, 24);
            this.SysStauLab.TabIndex = 68;
            this.SysStauLab.Text = "●";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(300, 12);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(44, 18);
            this.label48.TabIndex = 86;
            this.label48.Text = "Data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(402, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 84;
            this.label2.Text = "OPCUA";
            // 
            // DataStatuLab
            // 
            this.DataStatuLab.AutoSize = true;
            this.DataStatuLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataStatuLab.ForeColor = System.Drawing.Color.Red;
            this.DataStatuLab.Location = new System.Drawing.Point(345, 12);
            this.DataStatuLab.Name = "DataStatuLab";
            this.DataStatuLab.Size = new System.Drawing.Size(34, 24);
            this.DataStatuLab.TabIndex = 87;
            this.DataStatuLab.Text = "●";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel3.Controls.Add(this.RobotNameLab);
            this.panel3.Controls.Add(this.RobotSWBtn);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(226, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1282, 80);
            this.panel3.TabIndex = 66;
            // 
            // RobotNameLab
            // 
            this.RobotNameLab.AutoSize = true;
            this.RobotNameLab.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RobotNameLab.Location = new System.Drawing.Point(186, 23);
            this.RobotNameLab.Name = "RobotNameLab";
            this.RobotNameLab.Size = new System.Drawing.Size(270, 33);
            this.RobotNameLab.TabIndex = 87;
            this.RobotNameLab.Text = "Robot_Type_Name";
            // 
            // RobotSWBtn
            // 
            this.RobotSWBtn.BackColor = System.Drawing.SystemColors.HighlightText;
            this.RobotSWBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.RobotSWBtn.FlatAppearance.BorderSize = 0;
            this.RobotSWBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RobotSWBtn.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RobotSWBtn.Location = new System.Drawing.Point(1116, 0);
            this.RobotSWBtn.Name = "RobotSWBtn";
            this.RobotSWBtn.Size = new System.Drawing.Size(166, 80);
            this.RobotSWBtn.TabIndex = 86;
            this.RobotSWBtn.Text = "机器人切换";
            this.RobotSWBtn.UseVisualStyleBackColor = false;
            this.RobotSWBtn.Click += new System.EventHandler(this.RobotSWBtn_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1150, 80);
            this.label1.TabIndex = 62;
            this.label1.Text = "当前机器人:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel5.Controls.Add(this.panel4);
            this.panel5.Controls.Add(this.panel2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(226, 80);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1282, 950);
            this.panel5.TabIndex = 67;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ChildFrmPanel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1282, 906);
            this.panel4.TabIndex = 66;
            // 
            // AutoExitLogTimer
            // 
            this.AutoExitLogTimer.Interval = 100000;
            this.AutoExitLogTimer.Tick += new System.EventHandler(this.AutoExitLogTimer_Tick);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1508, 1030);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button MainFrmBtn;
        private System.Windows.Forms.Panel ChildFrmPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button HistoryFrmBtn;
        private System.Windows.Forms.Button RobotDataBtn;
        private System.Windows.Forms.Button AnalyDataBtn;
        private System.Windows.Forms.Timer AutoExitLogTimer;
        private System.Windows.Forms.Button LoginBtn;
        private System.Windows.Forms.Label ReadyLab;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label SysStauLab;
        private System.Windows.Forms.Label OpcuaConLab;
        private System.Windows.Forms.Label SensorYLab;
        private System.Windows.Forms.Label y;
        private System.Windows.Forms.Label SensorXLab;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label DataStatuLab;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button RobotSWBtn;
        private System.Windows.Forms.Label RobotNameLab;
    }
}

