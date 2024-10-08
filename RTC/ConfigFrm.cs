using Common;
using Model;
using RTC.Common;
using RTC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
    public delegate ConfigPara UpdateConfig2(string str);
    public partial class ConfigFrm : Form
    {
        private ConfigPara _configPara;
        private double[] _point = new double[10];
        private int _RobotIndex;
        private DataTable _dataTable;//=new DataTable();
        private string _pathRoot = AppDomain.CurrentDomain.BaseDirectory;
        private int _RobotsToolIndex = 0;
        public UpdateConfig2 _UpdateConfig;
        private string _ConfigPath;
        private string _CurrentRobots;
        public ConfigFrm()
        {
            InitializeComponent();
        }

        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            StaticCommonVar.RobotSaveBtn = RobotSaveBtn;
            UpdateControl();
        }
        public void LoadFun(ConfigPara config,string CurrentRobots)
        {
            _configPara = ConfigPara.DeepClone(config);
            _CurrentRobots=CurrentRobots;
        }
        private string NumberStrb(string input)
        {


            string pattern = @"(-?\d+\.\d{3})"; // 正则表达式，匹配小数点后三位数字  
            MatchCollection matches = Regex.Matches(input, pattern);

            string originalValue = string.Empty;
            foreach (Match match in matches)
            {
                originalValue += match.Value + ","; // 拼接匹配结果，用逗号分隔  
            }
            // 去除末尾的逗号  
            if (originalValue.EndsWith(","))
            {
                originalValue = originalValue.Substring(0, originalValue.Length - 1);
            }
            return originalValue;

        }
        private void UpdateControl()
        {
            int i = 1;
            ConfigRealToolLab.Text = i.ToString();
            ConfigToolsCom.Text="";
            ConfigToolsCom.Items.Clear();
            //加载工具项
            foreach (var item in _configPara.Robots.Tools)
            {
                if (item.CaToolNum != 0)
                {
                    ConfigToolsCom.Items.Add(i++);
                }
            }

            string robothis = _configPara.Robots.BU + "\\" + _configPara.Robots.LineName + "\\" + _configPara.Robots.WorkName + "\\" + _configPara.Robots.RobotName + "\\" + _configPara.Robots.RobotSeriorNo; ;
            string robotinit = robothis;
            AnalyFileSelectList.ScrollAlwaysVisible = true; // 使滚动条始终可见  
            string DataWrite = $@"DataWrite/{robotinit}";
            List<string> fileName = ConfigExcel.ReadExlName(DataWrite);
            AnalyFileSelectList.Items.Clear();
            // 将 fileName 中的每个元素添加到 ListBox 中  
            foreach (string item in fileName)
            {
                AnalyFileSelectList.Items.Add(item);
            }
            // BeforUpdateTimeValLab.Text = _configPara.Robots.Tools[_RobotsToolIndex].BeforUpdateTime;
            // UpdateTimeValLab.Text = _configPara.Robots.Tools[_RobotsToolIndex].UpdateTime;
            //label39.Text = $"{(Convert.ToDouble(_point[4])).ToString("0.000")},{(Convert.ToDouble(_point[5])).ToString("0.000")}";
            //label40.Text = $"{(Convert.ToDouble(_point[2])).ToString("0.000")},{(Convert.ToDouble(_point[3])).ToString("0.000")}";
            //label41.Text = $"{(Convert.ToDouble(_point[0])).ToString("0.000")},{(Convert.ToDouble(_point[1])).ToString("0.000")}";
            CfigXyz_xLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_X).ToString("0.000");
            CfigXyz_yLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_Y).ToString("0.000");
            CfigAbc_zLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Z).ToString("0.000");
            CfigAbc_yLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Y).ToString("0.000");
            //获取配置文件的tool值
            ConfigaLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].toolA).ToString("0.000");
            ConfigbLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].toolB).ToString("0.000");
            ConfigcLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].toolC).ToString("0.000");
            Configxlab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].toolX).ToString("0.000");
            ConfigyLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].toolY).ToString("0.000");
            ConfigzLab.Text = Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].toolZ).ToString("0.000");

            //配置文件的画圆点
            CfigPoint1Lab.Text = NumberStrb(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point1);
            CfigPoint2Lab.Text = NumberStrb(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point2);
            CfigPoint3Lab.Text = NumberStrb(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point3);
            CfigHeighLab.Text = (Convert.ToDouble(_configPara.Robots.Tools[_RobotsToolIndex].RobotConfigPara.PointHeight)).ToString("0.000");
            //上圆和下圆的z轴值（机器人画圆的点位）
            //RealUpCircValLab.Text = _point[6].ToString();
            //RealDoCircValLab.Text = _point[7].ToString();
            WriteHeihtLabe.Text = Math.Abs(_point[6] - _point[7]).ToString("0.000");

            CurrentDataLab.Text = _configPara.Robots.BU + "_" + _configPara.Robots.LineName + "_" + _configPara.Robots.WorkName + "_" + _configPara.Robots.RobotName + "_" + _configPara.Robots.RobotSeriorNo;
            BUTxt.Text = _configPara.Robots.BU;
            RobotBaseTxt.Text = _configPara.Robots.RobotBas.ToString();
            LineNameTxt.Text = _configPara.Robots.LineName;
            WorkNameTxt.Text = _configPara.Robots.WorkName;
            RobotNameTxt.Text = _configPara.Robots.RobotName;
            LocalIpTxt.Text = _configPara.DataIP;
            RobotIpTxt.Text = _configPara.Robots.RobotIP;
            RobotTypeComBox.Text = _configPara.Robots.RobotType;
            DownHeightTxt.Text = _configPara.Robots.AdjDownHeight;
            RobotSerialTxt.Text = _configPara.Robots.RobotSeriorNo;
            CircSpeedTxt.Text = _configPara.Robots.ProSpeed.ToString();
            ZAdjmentTxt.Text = _configPara.Robots.AdjustmentZ.ToString();
            CircDiaTxt.Text = _configPara.Robots.CircDiameter.ToString();
            TempToolTxt.Text = _configPara.Robots.CaTool.ToString();
            WarnXTxt.Text = _configPara.Robots.CheckToolX.ToString();
            WarnYTxt.Text = _configPara.Robots.CheckToolY.ToString();
            WarnZTxt.Text = _configPara.Robots.CheckToolZ.ToString();
            WarnATxt.Text = _configPara.Robots.CheckToolA.ToString();
            WarnBTxt.Text = _configPara.Robots.CheckToolB.ToString();
            WarnCTxt.Text = _configPara.Robots.CheckToolC.ToString();
            RobotGipperNumCbx.Text=_configPara.Robots.RobotGipperNum.ToString();
            UpdateGipper();
        }
        private void UpdateGipper()
        {
            if (_configPara.Robots.Tools[0].CaToolNum != 0)
            {
                Gipper1NumTxt.Text = _configPara.Robots.Tools[0].CaToolNum.ToString();
                Gipper1XTxt.Text = _configPara.Robots.Tools[0].toolX.ToString();
                Gipper1YTxt.Text = _configPara.Robots.Tools[0].toolY.ToString();
                Gipper1ZTxt.Text = _configPara.Robots.Tools[0].toolZ.ToString();
                Gipper1ATxt.Text = _configPara.Robots.Tools[0].toolA.ToString();
                Gipper1BTxt.Text = _configPara.Robots.Tools[0].toolB.ToString();
                Gipper1CTxt.Text = _configPara.Robots.Tools[0].toolC.ToString();
                Gipper1OpenInputTxt.Text = _configPara.Robots.Tools[0].aGipper.OpenInput.ToString();
                Gipper1CloseInputTxt.Text = _configPara.Robots.Tools[0].aGipper.CloseInput.ToString();
                Gipper1OpenOutTxt.Text = _configPara.Robots.Tools[0].aGipper.OpenOut.ToString();
                Gipper1CloseOutTxt.Text = _configPara.Robots.Tools[0].aGipper.CloseOut.ToString();
            }
            if (int.Parse(RobotGipperNumCbx.Text) > 1)
            {
                panel2.Visible = true;
                Gipper2NumTxt.Text = _configPara.Robots.Tools[1].CaToolNum.ToString();
                Gipper2XTxt.Text = _configPara.Robots.Tools[1].toolX.ToString();
                Gipper2YTxt.Text = _configPara.Robots.Tools[1].toolY.ToString();
                Gipper2ZTxt.Text = _configPara.Robots.Tools[1].toolZ.ToString();
                Gipper2ATxt.Text = _configPara.Robots.Tools[1].toolA.ToString();
                Gipper2BTxt.Text = _configPara.Robots.Tools[1].toolB.ToString();
                Gipper2CTxt.Text = _configPara.Robots.Tools[1].toolC.ToString();
                Gipper2OpenInputTxt.Text = _configPara.Robots.Tools[1].aGipper.OpenInput.ToString();
                Gipper2CloseInputTxt.Text = _configPara.Robots.Tools[1].aGipper.CloseInput.ToString();
                Gipper2OpenOutTxt.Text = _configPara.Robots.Tools[1].aGipper.OpenOut.ToString();
                Gipper2CloseOutTxt.Text = _configPara.Robots.Tools[1].aGipper.CloseOut.ToString();
            }
            else
            {
                panel2.Visible = false;
            }
            if (int.Parse(RobotGipperNumCbx.Text) > 2)
            {
                panel3.Visible = true;
                Gipper3NumTxt.Text = _configPara.Robots.Tools[2].CaToolNum.ToString();
                Gipper3XTxt.Text = _configPara.Robots.Tools[2].toolX.ToString();
                Gipper3YTxt.Text = _configPara.Robots.Tools[2].toolY.ToString();
                Gipper3ZTxt.Text = _configPara.Robots.Tools[2].toolZ.ToString();
                Gipper3ATxt.Text = _configPara.Robots.Tools[2].toolA.ToString();
                Gipper3BTxt.Text = _configPara.Robots.Tools[2].toolB.ToString();
                Gipper3CTxt.Text = _configPara.Robots.Tools[2].toolC.ToString();
                Gipper3OpenInputTxt.Text = _configPara.Robots.Tools[2].aGipper.OpenInput.ToString();
                Gipper3CloseInputTxt.Text = _configPara.Robots.Tools[2].aGipper.CloseInput.ToString();
                Gipper3OpenOutTxt.Text = _configPara.Robots.Tools[2].aGipper.OpenOut.ToString();
                Gipper3CloseOutTxt.Text = _configPara.Robots.Tools[2].aGipper.CloseOut.ToString();
            }
            else
            {
                panel3.Visible = false;
            }
            if (int.Parse(RobotGipperNumCbx.Text) > 3)
            {

                panel4.Visible = true;
                Gipper4NumTxt.Text = _configPara.Robots.Tools[3].CaToolNum.ToString();
                Gipper4XTxt.Text = _configPara.Robots.Tools[3].toolX.ToString();
                Gipper4YTxt.Text = _configPara.Robots.Tools[3].toolY.ToString();
                Gipper4ZTxt.Text = _configPara.Robots.Tools[3].toolZ.ToString();
                Gipper4ATxt.Text = _configPara.Robots.Tools[3].toolA.ToString();
                Gipper4BTxt.Text = _configPara.Robots.Tools[3].toolB.ToString();
                Gipper4CTxt.Text = _configPara.Robots.Tools[3].toolC.ToString();
                Gipper4OpenInputTxt.Text = _configPara.Robots.Tools[3].aGipper.OpenInput.ToString();
                Gipper4CloseInputTxt.Text = _configPara.Robots.Tools[3].aGipper.CloseInput.ToString();
                Gipper4OpenOutTxt.Text = _configPara.Robots.Tools[3].aGipper.OpenOut.ToString();
                Gipper4CloseOutTxt.Text = _configPara.Robots.Tools[3].aGipper.CloseOut.ToString();
            }
            else
            {
                panel4.Visible = false;
            }
        }
        private void SelectBtn_Click(object sender, EventArgs e)
        {
            new RobotsListFrm(true).ShowDialog();
           
            if (StaticCommonVar.RobotSelectName == "" || StaticCommonVar.RobotSelectName == null)
            {
                return;
            }
            

            _configPara=(RobotConfigFun.GetPara(StaticCommonVar.RobotSelectName));
            StaticCommonVar.RobotSelectName = "";
            UpdateControl();
        }
        private void VisualControl(bool visual,int num)
        {
            switch (num)
            {
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
        private void ShowNowBtn_Click(object sender, EventArgs e)
        {
           
            _configPara = (RobotConfigFun.GetPara(_CurrentRobots));
            StaticCommonVar.RobotSelectName = "";
            UpdateControl();
        }

        private void AnalyFileSelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 检查是否有项被选中  
            if (AnalyFileSelectList.SelectedIndex != -1)
            {
                // 获取选中的项的文本  
                string selectedItem = AnalyFileSelectList.SelectedItem.ToString();
                string robotinit = _configPara.Robots.BU + "\\" + _configPara.Robots.LineName + "\\" + _configPara.Robots.WorkName + "\\" + _configPara.Robots.RobotName + "\\" + _configPara.Robots.RobotSeriorNo; ;
                string data1 = $@"DataWrite\{robotinit}";
                _dataTable = GetData(ConfigExcel.ReadExl(selectedItem, data1), ConfigRealToolLab.Text); //ConfigExcel.ReadExl(selectedItem, data1);

                AnalyFileSelectDataView.DataSource = _dataTable;

            }
        }
        private DataTable GetData(DataTable dataTable, string value)
        {
            if (dataTable == null) { return null; }
            DataTable data = dataTable.Clone();
            value = (int.Parse(value) - 1).ToString();
            foreach (var item in dataTable.Select())
            {
                if (item.ItemArray[45].ToString() == value)
                {
                    data.Rows.Add(item.ItemArray);
                }
            }
            return data;
        }

        private void ConfigToolsCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigRealToolLab.Text = ConfigToolsCom.Text;
            _RobotsToolIndex = int.Parse(ConfigToolsCom.Text) - 1;
            if (AnalyFileSelectList.SelectedIndex != -1)
            {
                // 获取选中的项的文本  
                string selectedItem = AnalyFileSelectList.SelectedItem.ToString();
                string robotinit = _configPara.Robots.BU + "\\" + _configPara.Robots.LineName + "\\" + _configPara.Robots.WorkName + "\\" + _configPara.Robots.RobotName + "\\" + _configPara.Robots.RobotSeriorNo; ;
                string data1 = $@"DataWrite\{robotinit}";
                _dataTable = GetData(ConfigExcel.ReadExl(selectedItem, data1), ConfigRealToolLab.Text); //ConfigExcel.ReadExl(selectedItem, data1);

                AnalyFileSelectDataView.DataSource = _dataTable;
                UpdateControlConfig();
                //Updatedatagrid();

            }
        }

        private void UpdateControlConfig()
        {
           
        }

        private void AnalyFileSelectDataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RobotSaveBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"是否保存机器人数据", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ConfigPara para = ConfigPara.DeepClone(_configPara);

                para.DataIP = LocalIpTxt.Text;
                para.Robots.BU = BUTxt.Text;
                para.Robots.LineName=LineNameTxt.Text;
                para.Robots.WorkName=WorkNameTxt.Text;
                para.Robots.RobotName=RobotNameTxt.Text;
                para.Robots.RobotIP=RobotIpTxt.Text;
                para.Robots.RobotBas=int.Parse( RobotBaseTxt.Text);
                para.Robots.RobotSeriorNo=RobotSerialTxt.Text;
                para.Robots.RobotType=RobotTypeComBox.Text;
                para.Robots.AdjDownHeight=DownHeightTxt.Text;
                para.Robots.CircDiameter= double.Parse(CircDiaTxt.Text);
                para.Robots.ProSpeed= int.Parse(CircSpeedTxt.Text);
                para.Robots.CaTool= int.Parse(TempToolTxt.Text);
                para.Robots.AdjustmentZ=int.Parse(ZAdjmentTxt.Text);
                para.Robots.CheckToolX=WarnXTxt.Text;
                para.Robots.CheckToolY = WarnYTxt.Text;
                para.Robots.CheckToolZ = WarnZTxt.Text;
                para.Robots.CheckToolA = WarnATxt.Text;
                para.Robots.CheckToolB = WarnBTxt.Text;
                para.Robots.CheckToolC = WarnCTxt.Text;
                para.Robots.RobotGipperNum=int.Parse( RobotGipperNumCbx.Text);

                if (_configPara.Robots.Tools[0].CaToolNum != 0)
                {
                    para.Robots.Tools[0].CaToolNum=int.Parse(Gipper1NumTxt.Text);
                    para.Robots.Tools[0].toolX=Gipper1XTxt.Text;
                    para.Robots.Tools[0].toolY = Gipper1YTxt.Text;
                    para.Robots.Tools[0].toolZ = Gipper1ZTxt.Text;
                    para.Robots.Tools[0].toolA = Gipper1ATxt.Text;
                    para.Robots.Tools[0].toolB = Gipper1BTxt.Text;
                    para.Robots.Tools[0].toolC = Gipper1CTxt.Text;
                    para.Robots.Tools[0].aGipper.OpenInput = int.Parse(Gipper1OpenInputTxt.Text);
                    para.Robots.Tools[0].aGipper.CloseInput = int.Parse(Gipper1CloseInputTxt.Text);
                    para.Robots.Tools[0].aGipper.OpenOut = int.Parse(Gipper1OpenOutTxt.Text);
                    para.Robots.Tools[0].aGipper.CloseOut = int.Parse(Gipper1CloseOutTxt.Text);
                   
                }
                if (_configPara.Robots.Tools[1].CaToolNum != 0)
                {
                    para.Robots.Tools[1].CaToolNum = int.Parse(Gipper2NumTxt.Text);
                    para.Robots.Tools[1].toolX = Gipper2XTxt.Text;
                    para.Robots.Tools[1].toolY = Gipper2YTxt.Text;
                    para.Robots.Tools[1].toolZ = Gipper2ZTxt.Text;
                    para.Robots.Tools[1].toolA = Gipper2ATxt.Text;
                    para.Robots.Tools[1].toolB = Gipper2BTxt.Text;
                    para.Robots.Tools[1].toolC = Gipper2CTxt.Text;
                    para.Robots.Tools[1].aGipper.OpenInput = int.Parse(Gipper2OpenInputTxt.Text);
                    para.Robots.Tools[1].aGipper.CloseInput = int.Parse(Gipper2CloseInputTxt.Text);
                    para.Robots.Tools[1].aGipper.OpenOut = int.Parse(Gipper2OpenOutTxt.Text);
                    para.Robots.Tools[1].aGipper.CloseOut = int.Parse(Gipper2CloseOutTxt.Text);
                }
               
                if (_configPara.Robots.Tools[2].CaToolNum != 0)
                {
                    para.Robots.Tools[2].CaToolNum = int.Parse(Gipper3NumTxt.Text);
                    para.Robots.Tools[2].toolX = Gipper3XTxt.Text;
                    para.Robots.Tools[2].toolY = Gipper3YTxt.Text;
                    para.Robots.Tools[2].toolZ = Gipper3ZTxt.Text;
                    para.Robots.Tools[2].toolA = Gipper3ATxt.Text;
                    para.Robots.Tools[2].toolB = Gipper3BTxt.Text;
                    para.Robots.Tools[2].toolC = Gipper3CTxt.Text;
                    para.Robots.Tools[2].aGipper.OpenInput = int.Parse(Gipper3OpenInputTxt.Text);
                    para.Robots.Tools[2].aGipper.CloseInput = int.Parse(Gipper3CloseInputTxt.Text);
                    para.Robots.Tools[2].aGipper.OpenOut = int.Parse(Gipper3OpenOutTxt.Text);
                    para.Robots.Tools[2].aGipper.CloseOut = int.Parse(Gipper3CloseOutTxt.Text);
                }
                
                if (_configPara.Robots.Tools[3].CaToolNum != 0)
                {

                    para.Robots.Tools[3].CaToolNum = int.Parse(Gipper4NumTxt.Text);
                    para.Robots.Tools[3].toolX = Gipper4XTxt.Text;
                    para.Robots.Tools[3].toolY = Gipper4YTxt.Text;
                    para.Robots.Tools[3].toolZ = Gipper4ZTxt.Text;
                    para.Robots.Tools[3].toolA = Gipper4ATxt.Text;
                    para.Robots.Tools[3].toolB = Gipper4BTxt.Text;
                    para.Robots.Tools[3].toolC = Gipper4CTxt.Text;
                    para.Robots.Tools[3].aGipper.OpenInput = int.Parse(Gipper4OpenInputTxt.Text);
                    para.Robots.Tools[3].aGipper.CloseInput = int.Parse(Gipper4CloseInputTxt.Text);
                    para.Robots.Tools[3].aGipper.OpenOut = int.Parse(Gipper4OpenOutTxt.Text);
                    para.Robots.Tools[3].aGipper.CloseOut = int.Parse(Gipper4CloseOutTxt.Text);
                }
               //if( StaticCommonVar.RobotName== CurrentDataLab.Text)
               // {
               //     ConfigExcel.WriteConfig(para);
               // }
                string newName = para.Robots.BU + "_" + para.Robots.LineName + "_" + para.Robots.WorkName + "_" + para.Robots.RobotName + "_" + para.Robots.RobotSeriorNo;
                File.Move(@"Config\"+CurrentDataLab.Text+".xml", @"Config\"+newName+".xml");

                ConfigExcel.NewConfig(para,newName);
               
                _configPara = (RobotConfigFun.GetPara(newName));
                //StaticCommonVar.RobotName = newName;
                UpdateControl();

            }
        }

        private void Gipper1ToolReadBtn_Click(object sender, EventArgs e)
        {
            int num = Parse(Gipper1NumTxt.Text);

           if (num>0)
            {
               List<double> doubles= StaticCommonVar.ReadTool(num);
                if (doubles != null)
                {
                    Gipper1XTxt.Text = doubles[0].ToString();
                    Gipper1YTxt.Text = doubles[1].ToString();
                    Gipper1ZTxt.Text = doubles[2].ToString();
                    Gipper1ATxt.Text = doubles[3].ToString();
                    Gipper1BTxt.Text = doubles[4].ToString();
                    Gipper1CTxt.Text = doubles[5].ToString();
                }
                else
                {
                    MessageBox.Show("读取错误");
                    return ;
                }
            }
          
           
        }

        private int Parse(string txt)
        {
            int ToolNum;
            if (txt != "" && int.TryParse(txt, out int result))
            {
                ToolNum = int.Parse(txt);
            }
            else
            {
                MessageBox.Show("输入为空或者编号不正确");
                return 0;
            }

            if(ToolNum>0&& ToolNum<=16)
            {
                return int.Parse(txt);
            }
            else
            {
                MessageBox.Show("输入编号不正确,为0-16");
                return 0;
            }
        }

        private void RobotGipperNumCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGipper();
        }

        private void Gipper2ToolReadBtn_Click(object sender, EventArgs e)
        {
            int num = Parse(Gipper2NumTxt.Text);

            if (num > 0)
            {
                List<double> doubles = StaticCommonVar.ReadTool(num);
                if (doubles != null)
                {
                    Gipper2XTxt.Text = doubles[0].ToString();
                    Gipper2YTxt.Text = doubles[1].ToString();
                    Gipper2ZTxt.Text = doubles[2].ToString();
                    Gipper2ATxt.Text = doubles[3].ToString();
                    Gipper2BTxt.Text = doubles[4].ToString();
                    Gipper2CTxt.Text = doubles[5].ToString();
                }
                else
                {
                    MessageBox.Show("读取错误");
                    return;
                }
            }
        }

        private void Gipper3ToolReadBtn_Click(object sender, EventArgs e)
        {
            int num = Parse(Gipper3NumTxt.Text);

            if (num > 0)
            {
                List<double> doubles = StaticCommonVar.ReadTool(num);
                if (doubles != null)
                {
                    Gipper3XTxt.Text = doubles[0].ToString();
                    Gipper3YTxt.Text = doubles[1].ToString();
                    Gipper3ZTxt.Text = doubles[2].ToString();
                    Gipper3ATxt.Text = doubles[3].ToString();
                    Gipper3BTxt.Text = doubles[4].ToString();
                    Gipper3CTxt.Text = doubles[5].ToString();
                }
                else
                {
                    MessageBox.Show("读取错误");
                    return;
                }
            }
        }

        private void Gipper4ToolReadBtn_Click(object sender, EventArgs e)
        {
            
            int num = Parse(Gipper4NumTxt.Text);

            if (num > 0)
            {
                List<double> doubles = StaticCommonVar.ReadTool(num);
                if (doubles != null)
                {
                    Gipper4XTxt.Text = doubles[0].ToString();
                    Gipper4YTxt.Text = doubles[1].ToString();
                    Gipper4ZTxt.Text = doubles[2].ToString();
                    Gipper4ATxt.Text = doubles[3].ToString();
                    Gipper4BTxt.Text = doubles[4].ToString();
                    Gipper4CTxt.Text = doubles[5].ToString();
                }
                else
                {
                    MessageBox.Show("读取错误");
                    return;
                }
            }
        }

        private void AnalyFileSelectDataView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //获取选中行
            DataGridViewRow row = AnalyFileSelectDataView.SelectedRows[0];
            //获取行中的指定列数据
            string columnValue = row.Cells[0].Value.ToString();

            //_UpdateTime = row.Cells[0].Value.ToString();
            // _testdata[47] = $"{para.CaTool}"; 工具编号
            ConfigRealToolLab.Text = row.Cells[47].Value.ToString();
            // label65.Text = row.Cells[36].Value.ToString();
            // label64.Text = row.Cells[33].Value.ToString();
            // label62.Text = row.Cells[34].Value.ToString();
            //label60.Text = row.Cells[35].Value.ToString();
            // checkvalue.Text = row.Cells[44].Value.ToString();
            //改变前
            NowtoolaLab.Text = (Convert.ToDouble(row.Cells[21].Value)).ToString("0.000");
            NowtoolbLab.Text = (Convert.ToDouble(row.Cells[22].Value)).ToString("0.000");
            NowtoolcLab.Text = (Convert.ToDouble(row.Cells[23].Value)).ToString("0.000");
            NowtoolxLab.Text = (Convert.ToDouble(row.Cells[24].Value)).ToString("0.000");
            NowtoolyLab.Text = (Convert.ToDouble(row.Cells[25].Value)).ToString("0.000");
            NowtoolzLab.Text = (Convert.ToDouble(row.Cells[26].Value)).ToString("0.000");

            NowPoint1Lab.Text = NumberStrb(row.Cells[39].Value.ToString());// $"{(Convert.ToDouble(_point[4])).ToString("0.000")},{(Convert.ToDouble(_point[5])).ToString("0.000")}";
            NowPoint2Lab.Text = NumberStrb(row.Cells[40].Value.ToString());// $"{(Convert.ToDouble(_point[2])).ToString("0.000")},{(Convert.ToDouble(_point[3])).ToString("0.000")}";
            NowPoint3Lab.Text = NumberStrb(row.Cells[41].Value.ToString());// $"{(Convert.ToDouble(_point[0])).ToString("0.000")},{(Convert.ToDouble(_point[1])).ToString("0.000")}";

            //label47.Text = row.Cells[27].Value.ToString().Substring(0, 7);

            //label65.Text = row.Cells[36].Value.ToString();
            //label64.Text= row.Cells[33].Value.ToString();
            //label62.Text = row.Cells[34].Value.ToString();
            //label60.Text = row.Cells[35].Value.ToString();
            NowXyz_xLab.Text = (Convert.ToDouble(row.Cells[16].Value)).ToString("0.000");
            NowAbc_yLab.Text = (Convert.ToDouble(row.Cells[17].Value)).ToString("0.000");
            NowAbc_zLab.Text = (Convert.ToDouble(row.Cells[18].Value)).ToString("0.000");
            NowXyz_yLab.Text = (Convert.ToDouble(row.Cells[19].Value)).ToString("0.000");
            // CfigHeighLab
            //增加高度显示
            WriteHeihtLabe.Text = (Convert.ToDouble(row.Cells[46].Value)).ToString("0.000");


        }
    }
}
