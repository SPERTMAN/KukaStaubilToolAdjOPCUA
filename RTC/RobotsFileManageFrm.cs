using Common;
using Model;
using Org.BouncyCastle.Security;
using RTC.Common;
using RTC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
    public partial class RobotsFileManageFrm : Form
    {
        public RobotsFileManageFrm()
        {
            InitializeComponent();
        }

        private void RobotsFileManageFrm_Load(object sender, EventArgs e)
        {
            UpdateControl();
        }

        private void DeletRobotBtn_Click(object sender, EventArgs e)
        {
            if (RobotsFileManaList.SelectedItem == null)
            {
                MessageBox.Show("未选择机器人数据");
                return;
            }
            if (MessageBox.Show($"是否删除机器人{RobotsFileManaList.SelectedItem}", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                //获取当前configpara
                RobotConfigFun.DeleteName(RobotsFileManaList.SelectedItem.ToString()); UpdateControl();
            }
        }

        private void UpdateControl()
        {
            RobotsFileManaList.Items.Clear();
            foreach (var item in RobotConfigFun.ReadAllName(StaticCommonVar.RobotConfigPath))
            {
                if (!item.Contains("Config"))
                {
                    RobotsFileManaList.Items.Add(item.Replace(".xml", ""));
                }

            }
            LineCombox.Items.Add("全部");
            BUCombox.Items.Add("全部");
            foreach (var item in RobotsFileManaList.Items)
            {
                string str = item.ToString();
                int index = str.IndexOf('_');
                
                LineCombox.Items.Add(str.Substring(index+1, str.IndexOf('_', index+1)-index-1));

                foreach (var item1 in BUCombox.Items)
                {
                    if(item1.ToString()== str.Substring(0, index))
                    {

                        goto done;
                    }
                   
                }
                BUCombox.Items.Add(str.Substring(0, index));
                done:
                str = null;
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            BUCombox.Items.Clear ();
            LineCombox.Items.Clear();
            RobotsFileManaList.Items.Clear ();
            UpdateControl();
            List<string> NameList= RobotsFileManaList.Items.Cast<string>().ToList();
            List<string> newNameList=new List<string>();
            List<string> newNameList2 = new List<string>();
            if (BUCombox.Text != "全部")
            {
                foreach (var item in NameList) 
                {
                    if (item.Contains(BUCombox.Text))
                    {
                        newNameList.Add(item);
                    }
                }
            }
            else
            {
                newNameList = NameList;
            }
            if (LineCombox.Text != "全部")
            {
                foreach (var item in newNameList)
                {
                    if (item.Contains(LineCombox.Text))
                    {
                        newNameList2.Add(item);
                    }
                }
            }
            else
            {
                newNameList2 = newNameList;
            }
            RobotsFileManaList.Items.Clear();
            foreach (var item in newNameList2) {
                RobotsFileManaList.Items.Add((string)item);
            }

            NameList = null;
            newNameList = null;
            newNameList2 = null;
        }

        private void RobotAddBtn_Click(object sender, EventArgs e)
        {
            if (WorkNameTxt.Text == "" || BUTxt.Text == "" || LineTxt.Text == ""|| RobotNameTxt.Text==""|| RobotSerialTxt.Text=="")
            {
                MessageBox.Show("输入的信息不全");
                return;
            }
            if (MessageBox.Show($"是否添加机器人", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ConfigPara para = RobotConfigFun.GetPara("ConfigPara");

                if (para != null)
                {
                    para.Robots.BU = BUTxt.Text;
                    para.Robots.LineName = LineTxt.Text;
                    para.Robots.WorkName = WorkNameTxt.Text;
                    para.Robots.RobotName = RobotNameTxt.Text;
                    para.Robots.RobotSeriorNo = RobotSerialTxt.Text;
                    string name = para.Robots.BU + "_" + para.Robots.LineName + "_" + para.Robots.WorkName + "_" + para.Robots.RobotName+"_" + para.Robots.RobotSeriorNo;
                    ConfigExcel.NewConfig(para, name);

                    UpdateControl();
                    WorkNameTxt.Text = "";
                    BUTxt.Text = "";
                    LineTxt.Text = "";
                    RobotNameTxt.Text = "";
                    RobotSerialTxt.Text = "";
                    MessageBox.Show("添加成功");
                }
            }
        }
    }
}
