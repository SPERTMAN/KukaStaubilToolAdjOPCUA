using Common;
using Model;
using RTC.Common;
using RTC.Model;
using Spire.Xls.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
    public partial class RobotsListFrm : Form
    {
        private bool _select;
        public RobotsListFrm(bool Select)
        {
            InitializeComponent();
            _select = Select;
        }

        private void RobotsListFrm_Load(object sender, EventArgs e)
        {
            if (_select)
            {
                ChangeOverRobotBtn.Text = "确认选择";
            }
            else
            {
                ChangeOverRobotBtn.Text = "确认换型";
            }
            UpdateControl();
            LineCombox.Items.Add("全部");
            BUCombox.Items.Add("全部");
            foreach (var item in RobotsFileList.Items)
            {
                string str = item.ToString();
                int index = str.IndexOf('_');

                LineCombox.Items.Add(str.Substring(index + 1, str.IndexOf('_', index + 1) - index - 1));

                foreach (var item1 in BUCombox.Items)
                {
                    if (item1.ToString() == str.Substring(0, index))
                    {

                        goto done;
                    }

                }
                BUCombox.Items.Add(str.Substring(0, index));
            done:
                str = null;
            }
        }

        private void ChangeOverRobotBtn_Click(object sender, EventArgs e)
        {
            if (RobotsFileList.SelectedItem == null)
            {
                MessageBox.Show("未选择机器人数据");
                return;
            }
            if (ChangeOverRobotBtn.Text == "确认换型")
            {


                if (MessageBox.Show($"是否更换机器人{RobotsFileList.SelectedItem}", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    //获取当前configpara
                    ConfigPara para = RobotConfigFun.GetPara(RobotsFileList.SelectedItem.ToString());

                    if (para != null)
                    {
                        ChangeOverRobotBtn.Enabled = false;
                        ConfigExcel.WriteConfig(para);

                        StaticCommonVar.RobotChangeOver = true;

                        //更改IP地址
                        IpFun.SetNetworkAdapter(para.DataIP, false, "255.255.255.0");

                        Task.Run(() =>
                        {
                            while (true)
                            {
                                if (!StaticCommonVar.RobotChangeOver)
                                {
                                    Thread.Sleep(500);
                                    Invoke(new Action(() =>
                                    {
                                        ChangeOverRobotBtn.Enabled = true;
                                        MessageBox.Show($"{RobotsFileList.SelectedItem.ToString()}换型成功");

                                    }));
                                    return;
                                }

                            }

                        });

                    }
                    else
                    {
                        MessageBox.Show($"{RobotsFileList.SelectedItem.ToString()}数据为空");
                    }
                }
            }
            else
            {
                //选择
                StaticCommonVar.RobotSelectName = RobotsFileList.SelectedItem.ToString();
                this.Close();
            }
        }

        private void BUCombox_SelectedIndexChanged(object sender, EventArgs e)
        {

            RobotsFileList.Items.Clear();
            UpdateControl();
            List<string> NameList = RobotsFileList.Items.Cast<string>().ToList();
            List<string> newNameList = new List<string>();
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
            RobotsFileList.Items.Clear();
            foreach (var item in newNameList2)
            {
                RobotsFileList.Items.Add((string)item);
            }

            NameList = null;
            newNameList = null;
            newNameList2 = null;
        }

        private void UpdateControl()
        {
            foreach (var item in RobotConfigFun.ReadAllName(StaticCommonVar.RobotConfigPath))
            {
                if (!item.Contains("Config"))
                {
                    RobotsFileList.Items.Add(item.Replace(".xml", ""));
                }

            }

        }


        private void LineCombox_SelectedIndexChanged(object sender, EventArgs e)
        {

            RobotsFileList.Items.Clear();
            UpdateControl();
            List<string> NameList = RobotsFileList.Items.Cast<string>().ToList();
            List<string> newNameList = new List<string>();
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
            RobotsFileList.Items.Clear();
            foreach (var item in newNameList2)
            {
                RobotsFileList.Items.Add((string)item);
            }

            NameList = null;
            newNameList = null;
            newNameList2 = null;
        }
    }
}
