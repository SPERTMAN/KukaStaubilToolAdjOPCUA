using Common;
using Model;
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
    public delegate ConfigPara UpdateConfig(string str);
    public partial class AnalyDataFrm : Form
    {
        private ConfigPara _configPara;
        private double[] _point = new double[10];
        private int _RobotIndex;
        private DataTable _dataTable;//=new DataTable();
        private string _pathRoot = AppDomain.CurrentDomain.BaseDirectory;
        private int _RobotsToolIndex = 0;
        public UpdateConfig _UpdateConfig;
        private string _ConfigPath;
        private string _UpdateTime;
        public AnalyDataFrm()
        {
            InitializeComponent();
        }

        public void LoadFun(ConfigPara config, double[] point, int RobotIndex, string path)
        {
            _configPara = ConfigPara.DeepClone(config);
            _point = point;
            _RobotIndex = RobotIndex;
            _ConfigPath = path;
        }
        private void AnalyDataFrm_Load(object sender, System.EventArgs e)
        {
            //将窗体的最大区域设置为工作区域
            MaximizedBounds = Screen.PrimaryScreen.WorkingArea;

            //窗体最大化
            WindowState = FormWindowState.Maximized;
            //  this.FormBorderStyle = FormBorderStyle.FixedSingle;
            int i = 1;
            ConfigRealToolLab.Text = i.ToString();

            //加载工具项
            foreach (var item in _configPara.Robots.Tools)
            {
                if (item.CaToolNum != 0)
                {
                    ConfigToolsCom.Items.Add(i++);
                }
            }

            ////获取选中行
            //DataGridViewRow row = dataGridView1.SelectedRows[0];
            //获取行中的指定列数据
            // string columnValue = row.Cells[0].Value.ToString();


            ////改变后
            //label42.Text = (Convert.ToDouble(row.Cells[32].Value)).ToString("0.000");
            //label43.Text = (Convert.ToDouble(row.Cells[31].Value)).ToString("0.000");
            //label44.Text = (Convert.ToDouble(row.Cells[30].Value)).ToString("0.000");
            //label45.Text = (Convert.ToDouble(row.Cells[29].Value)).ToString("0.000");
            //label46.Text = (Convert.ToDouble(row.Cells[28].Value)).ToString("0.000");
            //label47.Text = (Convert.ToDouble(row.Cells[27].Value)).ToString("0.000");


            string robothis = _configPara.Robots.RobotExcelAdress;
            string robotinit = _configPara.Robots.RobotExcelAdress;
            AnalyFileSelectList.ScrollAlwaysVisible = true; // 使滚动条始终可见  
            // 设置 ListBox 的宽度和高度  
            //listBox1.Width = 100; // 设置宽度  
            //listBox1.Height = 100; // 设置高度  
            string DataWrite = $@"DataWrite/{robotinit}";
            List<string> fileName = ConfigExcel.ReadExlName(DataWrite);
            // 将 ListBox 的 SelectionMode 属性设置为 Single，以便只能选择一项（如果需要的话）  
            // listBox1.SelectionMode = SelectionMode.MultiSimple;
            // 清空 ListBox 中的现有项（如果需要的话）  
            AnalyFileSelectList.Items.Clear();
            // 将 fileName 中的每个元素添加到 ListBox 中  
            foreach (string item in fileName)
            {
                AnalyFileSelectList.Items.Add(item);
            }
            string name = DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            _dataTable = GetData(ConfigExcel.ReadExl(name, DataWrite), "1");
            UpdateControl();

            AnalyFileSelectDataView.RowHeadersVisible = false;
            AnalyFileSelectDataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            AnalyFileSelectDataView.AutoResizeRows();
            AnalyFileSelectDataView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // dataGridView1.Rows[0].Selected = true;

            AnalyFileSelectDataView.DataSource = _dataTable;
            //_BtnCheck = false;
            AnalyFileSelectList.SelectedIndex = AnalyFileSelectList.Items.Count - 1;
            Updatedatagrid();
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

        private void UpdateControl()
        {  //获取选中行


            BeforUpdateTimeValLab.Text = _configPara.Robots.Tools[_RobotsToolIndex].BeforUpdateTime;
            UpdateTimeValLab.Text = _configPara.Robots.Tools[_RobotsToolIndex].UpdateTime;
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


            //CheckToolXTxt.Text = _configPara.CheckToolX;
            //CheckToolYTxt.Text = _configPara.CheckToolY;
            //CheckToolZTxt.Text = _configPara.CheckToolZ;
            //CheckToolATxt.Text = _configPara.CheckToolA;
            //CheckToolBTxt.Text = _configPara.CheckToolB;
            //CheckToolCTxt.Text = _configPara.CheckToolC;
        }

        private void AnalyFileSelectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 检查是否有项被选中  
            if (AnalyFileSelectList.SelectedIndex != -1)
            {
                // 获取选中的项的文本  
                string selectedItem = AnalyFileSelectList.SelectedItem.ToString();
                string robotinit = _configPara.Robots.RobotExcelAdress;
                string data1 = $@"DataWrite\{robotinit}";
                _dataTable = GetData(ConfigExcel.ReadExl(selectedItem, data1), ConfigRealToolLab.Text); //ConfigExcel.ReadExl(selectedItem, data1);

                AnalyFileSelectDataView.DataSource = _dataTable;

            }
        }

        private void AnalyFileSelectDataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        /// <summary>
        /// 给选择的当前行数据赋值到页面
        /// </summary>
        private void Updatedatagrid()
        {
            //获取选中行
            DataGridViewRow row = AnalyFileSelectDataView.SelectedRows[0];
            //获取行中的指定列数据
            string columnValue = row.Cells[0].Value.ToString();

            _UpdateTime = row.Cells[0].Value.ToString();
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
            label56.Text = (Convert.ToDouble(row.Cells[19].Value)).ToString("0.000");
            label57.Text = (Convert.ToDouble(row.Cells[18].Value)).ToString("0.000");
            label58.Text = (Convert.ToDouble(row.Cells[17].Value)).ToString("0.000");
            label59.Text = (Convert.ToDouble(row.Cells[16].Value)).ToString("0.000");


        }

        private void ConfigToolsCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigRealToolLab.Text = ConfigToolsCom.Text;
            _RobotsToolIndex = int.Parse(ConfigToolsCom.Text) - 1;
            if (AnalyFileSelectList.SelectedIndex != -1)
            {
                // 获取选中的项的文本  
                string selectedItem = AnalyFileSelectList.SelectedItem.ToString();
                string robotinit = _configPara.Robots.RobotExcelAdress;
                string data1 = $@"DataWrite\{robotinit}";
                _dataTable = GetData(ConfigExcel.ReadExl(selectedItem, data1), ConfigRealToolLab.Text); //ConfigExcel.ReadExl(selectedItem, data1);

                AnalyFileSelectDataView.DataSource = _dataTable;
                UpdateControl();
                Updatedatagrid();

            }
        }
        public string LabText(string numberString)
        {
            bool hasNegativeSign = numberString.StartsWith("-"); // 检查是否已包含负号  

            // 根据条件添加或截取负号  
            if (hasNegativeSign)
            {
                // 如果已经有负号，则移除负号  
                numberString = numberString.TrimStart('-');
            }
            else
            {
                // 如果缺少负号，则添加负号  
                numberString = "-" + numberString;
            }

            return numberString;

        }
        private void WriteCfgBtn_Click(object sender, EventArgs e)
        {
            ConfigPara para = ConfigPara.DeepClone(_configPara);
            StringBuilder sb = new StringBuilder();

            //ConfigToolsCom.Text
            sb.AppendLine($"当前写入工具坐标：{int.Parse(ConfigRealToolLab.Text)}");
            //if (checkBox1.Checked)
            //{ }
            //var k1 = (point2.X - point1.X) / (point2.Y - point1.Y);
            //var a1 = (point2.X + point1.X) / 2;
            //var b1 = (point2.Y + point1.Y) / 2;
            //var k2 = (point3.X - point1.X) / (point3.Y - point1.Y);
            if (Math.Abs(_point[5]) == Math.Abs(_point[3]))
            {
                sb.AppendLine("画圆点位：");
                sb.AppendLine("画圆点位：");
                sb.AppendLine($"point1：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point1}->{_point[0]},{_point[1]}");
                sb.AppendLine($"point2：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point2}->{_point[4]},{_point[5]}");
                sb.AppendLine($"point3：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point3}->{_point[2]},{_point[3]}");
                //上下圆的差返回绝对值
                sb.AppendLine($"Z：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.PointHeight}->{Math.Abs(_point[6] - _point[7])}");
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point1 = $"{_point[0]},{_point[1]}"; ;
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point2 = $"{_point[4]},{_point[5]}";
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point3 = $"{_point[2]},{_point[3]}";
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.PointHeight = Math.Abs(_point[6] - _point[7]).ToString();
            }
            else
            {
                sb.AppendLine("画圆点位：");
                sb.AppendLine($"point1：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point1}->{_point[4]},{_point[5]}");
                sb.AppendLine($"point2：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point2}->{_point[2]},{_point[3]}");
                sb.AppendLine($"point3：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point3}->{_point[0]},{_point[1]}");
                //上下圆的差返回绝对值
                sb.AppendLine($"Z：{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.PointHeight}->{Math.Abs(_point[6] - _point[7])}");
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point1 = $"{_point[4]},{_point[5]}"; ;
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point2 = $"{_point[2]},{_point[3]}";
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point3 = $"{_point[0]},{_point[1]}";
                para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.PointHeight = Math.Abs(_point[6] - _point[7]).ToString();
            }



            //if (checkBox2.Checked)
            //{ }
            var l59 = LabText((Convert.ToDouble(label59.Text)).ToString("0.000"));
            var l58 = LabText((Convert.ToDouble(label58.Text)).ToString("0.000"));
            var l57 = LabText((Convert.ToDouble(label57.Text)).ToString("0.000"));
            var l56 = LabText((Convert.ToDouble(label56.Text)).ToString("0.000"));
            sb.AppendLine("补偿值：");
            sb.AppendLine($"ABC_PreY:{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Y}->{label59.Text}");
            sb.AppendLine($"ABC_PreZ:{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Z}->{label58.Text}");
            sb.AppendLine($"XYZ_PreX:{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_X}->{label57.Text}");
            sb.AppendLine($"XYZ_PreY:{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_Y}->{label56.Text}");
            //para.Robots.RobotConfigPara.XyzPre_X=label56.Text;
            //para.Robots.RobotConfigPara.XyzPre_Y = label57.Text;
            //para.Robots.RobotConfigPara.AbcPre_Y = label58.Text;
            //para.Robots.RobotConfigPara.AbcPre_Z = label59.Text;
            para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_X = label57.Text;
            para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_Y = label56.Text;
            para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Y = label59.Text;
            para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Z = label58.Text;

            //if (checkBox3.Checked)
            //{ }
            sb.AppendLine("工具坐标：");
            sb.AppendLine($"toola:{para.Robots.Tools[_RobotsToolIndex].toolA}->{NowtoolaLab.Text}");
            sb.AppendLine($"toolb:{para.Robots.Tools[_RobotsToolIndex].toolB}->{NowtoolbLab.Text}");
            sb.AppendLine($"toolc:{para.Robots.Tools[_RobotsToolIndex].toolC}->{NowtoolcLab.Text}");
            sb.AppendLine($"toolx:{para.Robots.Tools[_RobotsToolIndex].toolX}->{NowtoolxLab.Text}");
            sb.AppendLine($"tooly:{para.Robots.Tools[_RobotsToolIndex].toolY}->{NowtoolyLab.Text}");
            sb.AppendLine($"toolz:{para.Robots.Tools[_RobotsToolIndex].toolZ}->{NowtoolzLab.Text}");
            para.Robots.Tools[_RobotsToolIndex].toolA = NowtoolaLab.Text;
            para.Robots.Tools[_RobotsToolIndex].toolB = NowtoolbLab.Text;
            para.Robots.Tools[_RobotsToolIndex].toolC = NowtoolcLab.Text;
            para.Robots.Tools[_RobotsToolIndex].toolX = NowtoolxLab.Text;
            para.Robots.Tools[_RobotsToolIndex].toolY = NowtoolyLab.Text;
            para.Robots.Tools[_RobotsToolIndex].toolZ = NowtoolzLab.Text;

            para.Robots.Tools[_RobotsToolIndex].BeforUpdateTime = para.Robots.Tools[_RobotsToolIndex].UpdateTime;
            para.Robots.Tools[_RobotsToolIndex].UpdateTime = _UpdateTime;
            if (MessageBox.Show(sb.ToString(), "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                if ((MessageBox.Show("是否将之前的配置文件另存为", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK))
                {
                    string path = _pathRoot + @"\Config\";
                    string file = path + "ConfigPara.xml";
                    File.Copy(file, path + DateTime.Now.ToString("yyyyMMddHHmmss") + "ConfigPara.xml");
                    ConfigExcel.WriteConfig(para);
                }
                else
                {
                    ConfigExcel.WriteConfig(para);
                }

                _configPara = ConfigPara.DeepClone(_UpdateConfig(_ConfigPath));
                UpdateControl();
            }
        }

        private void AnalyFileSelectDataView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Updatedatagrid();
        }
    }
}
