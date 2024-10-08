using Newtonsoft.Json;
using Opc.Ua;
using Opc.Ua.Configuration;
using Opc.Ua.Helper;
using Spire.Doc.Fields;
using Spire.Xls;
using Spire.Xls.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using TwinCAT.Ads;
using Model;
using Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using RTC.Model;
using RTC.Common;


namespace RTC
{
    public partial class MainFrm : Form
    {

        private Point _MousePos;
        private Form currentChildForm;
        private ProcessFrm _ProcessFrm;
        private HistoryFrm _HistoryFrm;
        private ConfigFrm _ConfigFrm;
        private Form _RobotsFileManageFrm;
        private AnalyDataFrm _AnalyDataFrm;

        public MainFrm()
        {
            InitializeComponent();
        }
        private void MainFrm_Load(object sender, EventArgs e)
        {
            //StaticCommonVar.Logined = true;
            MainFrmBtn.BackColor = SystemColors.ScrollBar;
            _ProcessFrm =new ProcessFrm();
            _HistoryFrm =new HistoryFrm();
            _ConfigFrm = new ConfigFrm();
            _AnalyDataFrm=new AnalyDataFrm();
            _RobotsFileManageFrm=new RobotsFileManageFrm();
            _HistoryFrm._writeTool = _ProcessFrm.HisWriTool;
            OpenChileForm(_ProcessFrm);

            //将窗体的最大区域设置为工作区域
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;

            //窗体最大化
            this.WindowState = FormWindowState.Maximized;
            //  this.FormBorderStyle = FormBorderStyle.FixedSingle;

            Task.Run(() => {

                while (StaticCommonVar._Exit)
                {
                    Thread.Sleep(100);
                    if (!StaticCommonVar._Exit)
                        return;
                    Invoke(new Action(() =>
                    {
                        if(StaticCommonVar.Logined)
                        {
                            if (StaticCommonVar.RobotSaveBtn!=null)
                            {
                                StaticCommonVar.RobotSaveBtn.Enabled = true;// true;
                            }
                           
                            AnalyDataBtn.Enabled = true;
                            RobotDataBtn.Enabled = true;
                        }
                        else
                        {
                            if (StaticCommonVar.RobotSaveBtn != null)
                            {
                                StaticCommonVar.RobotSaveBtn.Enabled = false;// true;
                            }
                            // AnalyDataBtn.Enabled = false;
                            RobotDataBtn.Enabled= false;
                        }
                        
                        SysStauLab.ForeColor = StaticCommonVar.SysStaus ? Color.Green : Color.Red;
                        OpcuaConLab.ForeColor = StaticCommonVar.Opcua_Status ? Color.Green : Color.Red;
                      
                        SensorXLab.ForeColor = StaticCommonVar.X_Status ? Color.Green : Color.Red;
                        SensorYLab.ForeColor = StaticCommonVar.Y_Status ? Color.Green : Color.Red;
                        DataStatuLab.ForeColor = StaticCommonVar.Data_Status? Color.Green : Color.Red;
                        ReadyLab.ForeColor = StaticCommonVar.Ready_Status ? Color.Green : Color.Red;
                        AutoExitLog();
                        RobotNameLab.Text = StaticCommonVar.RobotName;
                    }));


                }
            });
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
       
           

        /// <summary>
        /// 打开嵌入子窗体
        /// </summary>
        /// <param name="childForm"></param>
        private void OpenChileForm(Form childForm)
        {
            // currentChildForm = childForm;

            if (currentChildForm != null)
            {
                Invoke(new Action(() => { currentChildForm.Hide(); }));
            }
            currentChildForm = childForm;
            if (currentChildForm.InvokeRequired)
            {

                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                ChildFrmPanel.Controls.Add(childForm);
                ChildFrmPanel.Tag = childForm;
                childForm.BringToFront();
                childForm.Show();
            }
            else
            {
                Invoke(new Action(() =>
                {

                    currentChildForm = childForm;
                    childForm.TopLevel = false;
                    childForm.FormBorderStyle = FormBorderStyle.None;
                    childForm.Dock = DockStyle.Fill;
                    ChildFrmPanel.Controls.Add(childForm);
                    ChildFrmPanel.Tag = childForm;
                    childForm.BringToFront();
                    childForm.Show();
                }));
            }

            //  lblTitleChildForm.Text = childForm.Text;
        }

      

        private void ExitBtn_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否退出软件？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                return;
            }

            _ProcessFrm.ExitFun();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChileForm(_ProcessFrm);
            MainFrmBtn.BackColor = SystemColors.ScrollBar;
            HistoryFrmBtn.BackColor = SystemColors.Window;
            RobotDataBtn.BackColor = SystemColors.Window;
            AnalyDataBtn.BackColor = SystemColors.Window;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[] doubles;
            int RobotIndex;
          
            MainFrmBtn.BackColor = SystemColors.Window;
            HistoryFrmBtn.BackColor = SystemColors.ScrollBar;
            RobotDataBtn.BackColor = SystemColors.Window;
            AnalyDataBtn.BackColor = SystemColors.Window;
            _HistoryFrm.LoadFun(_ProcessFrm.HistoryParaBack(out doubles,out RobotIndex), doubles, RobotIndex);
            OpenChileForm(_HistoryFrm);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChileForm(_RobotsFileManageFrm);
            MainFrmBtn.BackColor = SystemColors.Window;
            HistoryFrmBtn.BackColor = SystemColors.Window;
            RobotDataBtn.BackColor = SystemColors.ScrollBar;
            AnalyDataBtn.BackColor = SystemColors.Window;
        }

        private void AnalyDataBtn_Click(object sender, EventArgs e)
        {
            //new RobotsListFrm(true).ShowDialog();
            //double[] doubles;
            //int RobotIndex;
            //string path;
            //if (StaticCommonVar.RobotSelectName == ""|| StaticCommonVar.RobotSelectName == null)
            //{
            //    return;
            //}
            MainFrmBtn.BackColor = SystemColors.Window;
            HistoryFrmBtn.BackColor = SystemColors.Window;
            RobotDataBtn.BackColor = SystemColors.Window;
            AnalyDataBtn.BackColor = SystemColors.ScrollBar;
            
            _ConfigFrm.LoadFun(RobotConfigFun.GetPara("ConfigPara"), "ConfigPara");
            StaticCommonVar.RobotSelectName = "";
            OpenChileForm(_ConfigFrm);
            //double[] doubles;
            //int RobotIndex;
            //string path;

            //MainFrmBtn.BackColor = SystemColors.Window;
            //HistoryFrmBtn.BackColor = SystemColors.Window;
            //RobotDataBtn.BackColor = SystemColors.Window;
            //AnalyDataBtn.BackColor = SystemColors.ScrollBar;
            //_AnalyDataFrm.LoadFun(_ProcessFrm.AnalyParaBack(out doubles, out RobotIndex,out path), doubles, RobotIndex,path);
            //OpenChileForm(_AnalyDataFrm);
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (ToolStripMenuItem.Text == "登录")
            //{
            //    string Pwd = Microsoft.VisualBasic.Interaction.InputBox("请输入密码：" + "\n", "权限登入", "", -1, -1);
            //    if (Pwd == StaticCommonVar.AdminPwd)

            //    {
            //        AnalyDataBtn.Enabled = true;
            //        //StartBtn.BackgroundImage = null;
            //        //HistoryBtn.BackgroundImage = null;
            //        //ConfigRunBtn.BackgroundImage = null;
            //        //AlgorConfugButton.BackgroundImage = null;
            //        StaticCommonVar.Logined = true;
            //        // groupBox11.Visible = true;


            //        //StartBtn.Text = "开始矫正";
            //        //HistoryBtn.Text = "历史数据查询";
            //        //ConfigRunBtn.Text = "初始配置运行";
            //        //AlgorConfugButton.Text= "算法基础信息配置";
            //        //StartBtn.Enabled = true;
            //        //HistoryBtn.Enabled = true;
            //        //ConfigRunBtn.Enabled = true;
            //        //AlgorConfugButton.Enabled = true;
            //        // comboBox1.Enabled = true;
            //        //comboBox2.Enabled = true;

            //        // _Admin=true;
            //        ToolStripMenuItem.Text = "退出登录";
            //    }
            //    else
            //    {
            //        //StartBtn.Enabled = false;
            //        //HistoryBtn.Enabled = false;
            //        //ConfigRunBtn.Enabled = false;
            //        //AlgorConfugButton.Enabled = false;
            //        //StartBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
            //        //HistoryBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
            //        //ConfigRunBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
            //        //AlgorConfugButton.BackgroundImage = Image.FromFile(_BtnImagePath);
            //        //// AutoExitLogTimer.Stop();
            //        //Logined = false;

            //        //// comboBox1.Enabled = false;
            //        //// comboBox2.Enabled = false;
            //        //StartBtn.Text = "";
            //        //HistoryBtn.Text = "";
            //        //ConfigRunBtn.Text = "";
            //        //AlgorConfugButton.Text = "";

            //        MessageBox.Show("密码错误");
            //    }



            //}
            //else
            //{
            //    AnalyDataBtn.Enabled = false;
            //    //StartBtn.Enabled = false;
            //    //HistoryBtn.Enabled = false;
            //    //ConfigRunBtn.Enabled = false;
            //    //AlgorConfugButton.Enabled = false;
            //    //StartBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
            //    //HistoryBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
            //    //ConfigRunBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
            //    //AlgorConfugButton.BackgroundImage = Image.FromFile(_BtnImagePath);

            //    //StartBtn.Text = "";
            //    //HistoryBtn.Text = "";
            //    //ConfigRunBtn.Text = "";
            //    //AlgorConfugButton.Text = "";
            //    //AutoExitLogTimer.Stop();
            //    StaticCommonVar. Logined = false;


            //    //comboBox1.Enabled = false;
            //    // comboBox2.Enabled = false;
            //    // check1.Visible = false;
               
            //    ToolStripMenuItem.Text = "登录";
            //}


        }
        private void AutoExitLog()
        {
            if (StaticCommonVar.Logined)
            {
                if (IsMouseActiveInControl(this)|| StaticCommonVar.SysStaus)
                {
                    if (AutoExitLogTimer.Enabled)
                        AutoExitLogTimer.Stop();

                }
                else
                {
                    if (!AutoExitLogTimer.Enabled)
                        AutoExitLogTimer.Start();

                }

            }
            else
            {
                if (AutoExitLogTimer.Enabled)
                    AutoExitLogTimer.Stop();

            }
        }

        public bool IsMouseActiveInControl(Control control)
        {
            Point mousePos = Control.MousePosition;
            if (_MousePos != mousePos)
            {
                _MousePos = mousePos;
                return true;
            }
            return false;
            //    Rectangle screenRect = control.RectangleToScreen(control.ClientRectangle);
            //    return screenRect.Contains(mousePos);
            //}
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (LoginBtn.BackColor == Color.White)
            {
                string Pwd = Microsoft.VisualBasic.Interaction.InputBox("请输入密码：" + "\n", "权限登入", "", -1, -1);
                if (Pwd == StaticCommonVar.AdminPwd)

                {
                    AnalyDataBtn.Enabled = true;
                    //StartBtn.BackgroundImage = null;
                    //HistoryBtn.BackgroundImage = null;
                    //ConfigRunBtn.BackgroundImage = null;
                    //AlgorConfugButton.BackgroundImage = null;
                    StaticCommonVar.Logined = true;
                    // groupBox11.Visible = true;


                    //StartBtn.Text = "开始矫正";
                    //HistoryBtn.Text = "历史数据查询";
                    //ConfigRunBtn.Text = "初始配置运行";
                    //AlgorConfugButton.Text= "算法基础信息配置";
                    //StartBtn.Enabled = true;
                    //HistoryBtn.Enabled = true;
                    //ConfigRunBtn.Enabled = true;
                    //AlgorConfugButton.Enabled = true;
                    // comboBox1.Enabled = true;
                    //comboBox2.Enabled = true;

                    // _Admin=true;
                    LoginBtn.BackColor= SystemColors.ScrollBar;
                }
                else
                {
                    StaticCommonVar.Logined = false;
                    //StartBtn.Enabled = false;
                    //HistoryBtn.Enabled = false;
                    //ConfigRunBtn.Enabled = false;
                    //AlgorConfugButton.Enabled = false;
                    //StartBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
                    //HistoryBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
                    //ConfigRunBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
                    //AlgorConfugButton.BackgroundImage = Image.FromFile(_BtnImagePath);
                    //// AutoExitLogTimer.Stop();
                    //Logined = false;

                    //// comboBox1.Enabled = false;
                    //// comboBox2.Enabled = false;
                    //StartBtn.Text = "";
                    //HistoryBtn.Text = "";
                    //ConfigRunBtn.Text = "";
                    //AlgorConfugButton.Text = "";

                    MessageBox.Show("密码错误");
                }



            }
            else
            {
               // AnalyDataBtn.Enabled = false;
                //StartBtn.Enabled = false;
                //HistoryBtn.Enabled = false;
                //ConfigRunBtn.Enabled = false;
                //AlgorConfugButton.Enabled = false;
                //StartBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
                //HistoryBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
                //ConfigRunBtn.BackgroundImage = Image.FromFile(_BtnImagePath);
                //AlgorConfugButton.BackgroundImage = Image.FromFile(_BtnImagePath);

                //StartBtn.Text = "";
                //HistoryBtn.Text = "";
                //ConfigRunBtn.Text = "";
                //AlgorConfugButton.Text = "";
                //AutoExitLogTimer.Stop();
                StaticCommonVar.Logined = false;
               
                LoginBtn.BackColor = Color.White;
                OpenChileForm(_ProcessFrm);
                MainFrmBtn.BackColor = SystemColors.ScrollBar;
                HistoryFrmBtn.BackColor = SystemColors.Window;
                RobotDataBtn.BackColor = SystemColors.Window;
                AnalyDataBtn.BackColor = SystemColors.Window;

                //comboBox1.Enabled = false;
                // comboBox2.Enabled = false;
                // check1.Visible = false;

                LoginBtn.BackColor = Color.White;
            }

        }

        private void RobotSWBtn_Click(object sender, EventArgs e)
        {
            if (StaticCommonVar.SysStaus)
            {
                MessageBox.Show("系统正在运行中");
                return;
            }
            new RobotsListFrm(false).ShowDialog();
        }

        private void AutoExitLogTimer_Tick(object sender, EventArgs e)
        {
           if( StaticCommonVar.Logined)
            {
                StaticCommonVar.Logined = false;
                LoginBtn.BackColor = Color.White;
                OpenChileForm(_ProcessFrm);
                MainFrmBtn.BackColor = SystemColors.ScrollBar;
                HistoryFrmBtn.BackColor = SystemColors.Window;
                RobotDataBtn.BackColor = SystemColors.Window;
                AnalyDataBtn.BackColor = SystemColors.Window;
            }
            AutoExitLogTimer.Stop();
        }

    }
    
}
