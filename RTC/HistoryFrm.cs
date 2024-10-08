using Common;
using Model;
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
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RTC
{
    public delegate bool WritteTool(double[] doubles, int toolnum, int step);
    public partial class HistoryFrm : Form
    {
        private ConfigPara _configPara;
        private double[] _point = new double[10];
        private int _RobotIndex;
        private DataTable _dataTable;
        private string _pathRoot = AppDomain.CurrentDomain.BaseDirectory;
        public WritteTool _writeTool;

        private bool writeRsult = false;
        private int _RobotsToolIndex = 0;
        private string _configPath;
        public HistoryFrm()
        {
            InitializeComponent();
            //_configPara = ConfigPara.DeepClone(config);
            //_point = point;
            //_RobotIndex = RobotIndex;
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
        public void LoadFun(ConfigPara config, double[] point, int RobotIndex)
        {
            _configPara = ConfigPara.DeepClone(config);
            _point = point;
            _RobotIndex = RobotIndex;
        }
        private void HistoryFrm_Load(object sender, EventArgs e)
        {
            Update();
            //_BtnCheck = false;
        }
        private void Update()
        {
            NowRobotNameLab.Text = _configPara.Robots.BU + "_" + _configPara.Robots.LineName + "_" + _configPara.Robots.WorkName + "_" + _configPara.Robots.RobotName + "_" + _configPara.Robots.RobotSeriorNo; ;
            //加载工具项
            ConfigToolsCom.Items.Clear();
            int i = 1;
            foreach (var item in _configPara.Robots.Tools)
            {
                if (item.CaToolNum != 0)
                {
                    ConfigToolsCom.Items.Add(i++);
                }
            }
            ConfigToolsCom.SelectedIndex = 0;
            string robothis = _configPara.Robots.BU + "\\" + _configPara.Robots.LineName + "\\" + _configPara.Robots.WorkName + "\\" + _configPara.Robots.RobotName + "\\" + _configPara.Robots.RobotSeriorNo; ;
            string robotinit = robothis;
            HisFileSelectList.ScrollAlwaysVisible = true; // 使滚动条始终可见  
            // 设置 ListBox 的宽度和高度  
            HisFileSelectList.Height = 100; // 设置高度  
            string TestData = $@"TestData\{robothis}";
            List<string> fileName = ConfigExcel.ReadExlName(TestData);
            // 将 ListBox 的 SelectionMode 属性设置为 Single，以便只能选择一项（如果需要的话）  
            // listBox1.SelectionMode = SelectionMode.MultiSimple;
            // 清空 ListBox 中的现有项（如果需要的话）  
            HisFileSelectList.Items.Clear();
            // 将 fileName 中的每个元素添加到 ListBox 中  
            foreach (string item in fileName)
            {
                HisFileSelectList.Items.Add(item);
            }
            string name = DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            _dataTable = GetData(ConfigExcel.ReadExl(name, TestData), "1");
            HisFileSelectDataView.RowHeadersVisible = false;
            HisFileSelectDataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            HisFileSelectDataView.AutoResizeRows();
            HisFileSelectDataView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // dataGridView1.Rows[0].Selected = true;

            //配置为文件的工具坐标值
            //After_A_lab.Text = (Convert.ToDouble(_configPara.Robots[_RobotIndex].Tools[_RobotsToolIndex].toolA)).ToString("0.000");
            //After_B_lab.Text = (Convert.ToDouble(_configPara.Robots[_RobotIndex].Tools[_RobotsToolIndex].toolB)).ToString("0.000");
            //After_C_lab.Text = (Convert.ToDouble(_configPara.Robots[_RobotIndex].Tools[_RobotsToolIndex].toolC)).ToString("0.000");
            //After_X_lab.Text = (Convert.ToDouble(_configPara.Robots[_RobotIndex].Tools[_RobotsToolIndex].toolX)).ToString("0.000");
            //After_Y_lab.Text = (Convert.ToDouble(_configPara.Robots[_RobotIndex].Tools[_RobotsToolIndex].toolY)).ToString("0.000");

            //After_Z_lab.Text = (Convert.ToDouble(_configPara.Robots[_RobotIndex].Tools[_RobotsToolIndex].toolZ)).ToString("0.000");

            //改变后            
            After_A_lab.Text = "未选择";
            After_B_lab.Text = "未选择";
            After_C_lab.Text = "未选择";
            After_X_lab.Text = "未选择";
            After_Y_lab.Text = "未选择";
            After_Z_lab.Text = "未选择";

            Befor_Z_lab.Text = "未选择";
            Befor_Y_lab.Text = "未选择";
            Befor_X_lab.Text = "未选择";
            Befor_C_lab.Text = "未选择";
            Befor_B_lab.Text = "未选择";
            Befor_A_lab.Text = "未选择";

            HisFileSelectDataView.DataSource = _dataTable;
            HisFileSelectList.SelectedIndex = HisFileSelectList.Items.Count - 1;
            UpdateControl();
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
        {
            try
            {


                //获取选中行
                DataGridViewRow row = HisFileSelectDataView.SelectedRows[0];
                if (row == null) { return; }
                //获取行中的指定列数据
                string columnValue = row.Cells[0].Value.ToString();
                //获取工具编号
                ConfigRealToolLab.Text = row.Cells[47].Value.ToString();
                //改变前
                After_A_lab.Text = (Convert.ToDouble(row.Cells[21].Value)).ToString("0.000");
                After_B_lab.Text = (Convert.ToDouble(row.Cells[22].Value)).ToString("0.000");
                After_C_lab.Text = (Convert.ToDouble(row.Cells[23].Value)).ToString("0.000");
                After_X_lab.Text = (Convert.ToDouble(row.Cells[24].Value)).ToString("0.000");
                After_Y_lab.Text = (Convert.ToDouble(row.Cells[25].Value)).ToString("0.000");
                After_Z_lab.Text = (Convert.ToDouble(row.Cells[26].Value)).ToString("0.000");

                //改变后
                Befor_Z_lab.Text = (Convert.ToDouble(row.Cells[32].Value)).ToString("0.000");
                Befor_Y_lab.Text = (Convert.ToDouble(row.Cells[31].Value)).ToString("0.000");
                Befor_X_lab.Text = (Convert.ToDouble(row.Cells[30].Value)).ToString("0.000");
                Befor_C_lab.Text = (Convert.ToDouble(row.Cells[29].Value)).ToString("0.000");
                Befor_B_lab.Text = (Convert.ToDouble(row.Cells[28].Value)).ToString("0.000");
                Befor_A_lab.Text = (Convert.ToDouble(row.Cells[27].Value)).ToString("0.000");
                if (row.Cells[44].Value.ToString() == "Check")
                {
                    HischeckLab.Text = "检查";
                }
                else if (row.Cells[44].Value.ToString() == "Start")
                {
                    HischeckLab.Text = "矫正";
                }
                else
                {
                    HischeckLab.Text = "";
                }


                if (double.TryParse(row.Cells[33].Value.ToString(), out double dou))
                {
                    //生产使用的工具坐标
                    Pro_A_lab.Text = (Convert.ToDouble(row.Cells[33].Value)).ToString("0.000");
                    Pro_B_lab.Text = (Convert.ToDouble(row.Cells[34].Value)).ToString("0.000");
                    Pro_C_lab.Text = (Convert.ToDouble(row.Cells[35].Value)).ToString("0.000");
                    Pro_X_lab.Text = (Convert.ToDouble(row.Cells[36].Value)).ToString("0.000");
                    Pro_Y_lab.Text = (Convert.ToDouble(row.Cells[37].Value)).ToString("0.000");
                    Pro_Z_lab.Text = (Convert.ToDouble(row.Cells[38].Value)).ToString("0.000");
                }


                XyPre_YLab.Text = (Convert.ToDouble(row.Cells[19].Value)).ToString("0.000");
                XyPre_XLab.Text = (Convert.ToDouble(row.Cells[18].Value)).ToString("0.000");
                ABCPre_ZLab.Text = (Convert.ToDouble(row.Cells[17].Value)).ToString("0.000");
                ABCPre_YLab.Text = (Convert.ToDouble(row.Cells[16].Value)).ToString("0.000");

                //差值（改变后-改变前）
                Diff_A_lab.Text = (Convert.ToDouble(row.Cells[27].Value) - Convert.ToDouble(row.Cells[21].Value)).ToString("0.000");
                Diff_B_lab.Text = (Convert.ToDouble(row.Cells[28].Value) - Convert.ToDouble(row.Cells[22].Value)).ToString("0.000");
                Diff_C_lab.Text = (Convert.ToDouble(row.Cells[29].Value) - Convert.ToDouble(row.Cells[23].Value)).ToString("0.000");
                Diff_X_lab.Text = (Convert.ToDouble(row.Cells[30].Value) - Convert.ToDouble(row.Cells[24].Value)).ToString("0.000");
                Diff_Y_lab.Text = (Convert.ToDouble(row.Cells[31].Value) - Convert.ToDouble(row.Cells[25].Value)).ToString("0.000");
                Diff_Z_lab.Text = (Convert.ToDouble(row.Cells[32].Value) - Convert.ToDouble(row.Cells[26].Value)).ToString("0.000");
            }
            catch (Exception)
            {
                return ;
                //throw;
            }
        }

        private void WriteProToolBtn_Click(object sender, EventArgs e)
        {
            if (StaticCommonVar.RobotName != NowRobotNameLab.Text)
            {
                MessageBox.Show("未选择当前型号的数据，不能进行写入");
                return;

            }
            if (Befor_A_lab.Text == "未选择")
            {
                MessageBox.Show("未选择数据");
                return;
            }
            if (writeRsult)
                return;
            //获取选中行
            DataGridViewRow row = HisFileSelectDataView.SelectedRows[0];
            //获取行中的指定列数据
            string columnValue = row.Cells[0].Value.ToString();
            double[] doubles = new double[6];
            int i = 0;
            if (Pro_After_Check.Checked)
            {
                for (int j = 0; j <= 5; j++)
                {
                    doubles[j] = Convert.ToDouble(row.Cells[21 + j].Value);
                }
            }
            if (Pro_Befor_Check.Checked)
            {
                for (int j = 0; j <= 5; j++)
                {
                    doubles[j] = Convert.ToDouble(row.Cells[27 + j].Value);
                }
            }
            if (Pro_Real_Check.Checked)
            {
                for (int j = 0; j <= 5; j++)
                {
                    doubles[j] = Convert.ToDouble(row.Cells[33 + j].Value);
                }
            }
            writeRsult = true;
            Task.Run(() =>
            {
                if (_writeTool(doubles, _configPara.Robots.Tools[_RobotsToolIndex].CaToolNum, i))
                {
                    MessageBox.Show("写入成功");
                    writeRsult = false;
                }
                else
                {
                    MessageBox.Show("写入失败，请检查时候满足写入条件");
                    writeRsult = false;
                }


            });
        }

        private void Pro_Befor_Check_Click(object sender, EventArgs e)
        {
            Pro_Befor_Check.Checked = true;
            Pro_After_Check.Checked = false;
            Pro_Real_Check.Checked = false;
        }

        private void Pro_After_Check_Click(object sender, EventArgs e)
        {
            Pro_Befor_Check.Checked = false;
            Pro_After_Check.Checked = true;
            Pro_Real_Check.Checked = false;
        }

        private void Pro_Real_Check_Click(object sender, EventArgs e)
        {
            Pro_Befor_Check.Checked = false;
            Pro_After_Check.Checked = false;
            Pro_Real_Check.Checked = true;
        }

        private void HisFileSelectDataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //  UpdateControl();
        }

        private void HisFileSelectList_SelectedIndexChanged(object sender, EventArgs e)
        {

            // 检查是否有项被选中  
            if (HisFileSelectList.SelectedIndex != -1)
            {
                string robothis = _configPara.Robots.BU + "\\" + _configPara.Robots.LineName + "\\" + _configPara.Robots.WorkName + "\\" + _configPara.Robots.RobotName + "\\" + _configPara.Robots.RobotSeriorNo; ;
                // 获取选中的项的文本  
                string selectedItem = HisFileSelectList.SelectedItem.ToString();
                string data1 = $@"TestData\{robothis}";

                _dataTable = GetData(ConfigExcel.ReadExl(selectedItem, data1), ConfigToolsCom.Text);
                HisFileSelectDataView.DataSource = _dataTable;

            }
        }

        private void ConfigToolsCom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigRealToolLab.Text = ConfigToolsCom.Text;
            _RobotsToolIndex = int.Parse(ConfigToolsCom.Text) - 1;
            if (HisFileSelectList.SelectedIndex != -1)
            {
                // 获取选中的项的文本  
                string selectedItem = HisFileSelectList.SelectedItem.ToString();
                string robotinit = _configPara.Robots.RobotExcelAdress;
                string data1 = $@"TestData\{robotinit}";
                _dataTable = GetData(ConfigExcel.ReadExl(selectedItem, data1), ConfigRealToolLab.Text); //ConfigExcel.ReadExl(selectedItem, data1);

                HisFileSelectDataView.DataSource = _dataTable;
                UpdateControl();
                // Updatedatagrid();

            }
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            new RobotsListFrm(true).ShowDialog();

            if (StaticCommonVar.RobotSelectName == "" || StaticCommonVar.RobotSelectName == null)
            {
                return;
            }


            _configPara = (RobotConfigFun.GetPara(StaticCommonVar.RobotSelectName));
            StaticCommonVar.RobotSelectName = "";
            // UpdateControl();
            // _configPara= RobotConfigFun.GetPara(RobotsFileList.SelectedItem.ToString());
            Update();
        }

        private void HisFileSelectDataView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //获取选中行
            UpdateControl();
            //  if()
        }
    }
}
