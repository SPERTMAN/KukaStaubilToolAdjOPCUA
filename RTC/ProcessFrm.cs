using Model;
using Opc.Ua.Configuration;
using Opc.Ua.Helper;
using Opc.Ua;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RTC.MainFrm;
using TwinCAT.Ads;
using System.Threading;
using Common;
using System.IO;
using Newtonsoft.Json;
using SqlSugar;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using RTC.Model;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

namespace RTC
{
    public partial class ProcessFrm : Form
    {
        enum MainStep { Init, WaitAuto, Auto, Err = 20 };
        enum AutoStep { Init, WaitAuto };
        enum kukamode { CalibrationTool = 1, WRITE_TOOL_VALUE = 10 };

        enum KukaWriteMode { Init, Start, Tool, Befor, reset };

        public struct Tooltemp
        {
            public double ToolX;
            public double ToolY;
            public double ToolZ;
            public double ToolA;
            public double ToolB;
            public double ToolC;
        };

        //测试时临时变量
        private Tooltemp _Testtool;
        private bool _TestStart;
        private bool _Start;
        private bool _CheckStart;
        private bool _CheckSignal;
        //系统在运行标志位
        private bool _SystemRun;
        //自动测试
        private bool _AutoTest;
        private bool _AutoTestWrite;
        //信息
        private List<string> _ltMsg = new List<string>();
        //节点服务地址
        private string NoteService;
        //配置文件信息
        private ConfigPara _cp = new ConfigPara();
        //日志文件路径矫正的
        private string _pathRoot = AppDomain.CurrentDomain.BaseDirectory;
        //节拍实体
        private Stopwatch _Stopwatch = new Stopwatch();
        //停止库卡采集
        private bool _KukaRead;
        //库卡工具坐标暂存
        private string[] KukaToolData = new string[6];
        //库卡工具坐标暂存s
        private List<string[]> _KukaToolDatas = new List<string[]>();
        List<string[]> KukaToolDatasTemp;
        //plc变量句柄
        private bool _plcConnected;
        private TcAdsClient _PLCclient;
        private string _Hangle_X;
        private string _Hangle_Y;
        private string _Hangle_BtnStart;
        private string _Hangle_LightOK;
        private string _Hangle_LightNG;
        private string _Hangle_BtnCheck;
        private bool _X;
        private bool _Y;
        private bool _BtnStart;
        private bool _LightOK;
        private bool _LightNG;
        private bool _BtnCheck;

        //采集通讯
        private IPAddress _ipAddress;
        private IPEndPoint _remoteEP;
        private Socket _socket;
        private bool _sendCon;
        private bool _reveCon;
        private bool _sendCom;
        private bool _reveCom;
        private string _sendData;
        private string _reveData;
        private Root _Data = new Root();

        private int _DataStep;
        private bool _GetComplete;
        //xyz的补偿值
        //Z轴输入
        private string Zinput;
        //opcuaKuka写入变量
        List<DataValue> dataValues = new List<DataValue>();
        private List<string> OpcuaKukaVar = new List<string>();
        //
        private bool _CACheck_x;
        private bool _CACheck_y;
        private bool _CACheck_xy;
        #region  定义变量接收库卡值

        private string _KukaProName;
        private string _KukaSpeed;
        private string _KukaProStatus;
        private string _KukaMode;
        private double _KukaModeToolA;
        private double _KukaModeToolB;
        private double _KukaModeToolC;
        private double _KukaModeToolX;
        private double _KukaModeToolY;
        private double _KukaModeToolZ;
        private bool _KukaComplete;
        private bool _KukaInit = true;
        private bool _KukaProRun;
        private int _OPCUA_STEP;
        private bool _OPCUA_POSX;
        private bool _OPCUA_POSY;
        private bool _OPCUA_POSXY;
        private bool _OPCUA_GtpperErr;
        private string _RobotSerial;
        private double[] _KukaPoint = new double[8];

        //private double[] _KukaPoint2 = new double[2];
        //private double[] _KukaPoint3 = new double[2];

        #endregion
        //定义矫正后是否继续校验的值
        private bool _CheckContin = false;
        //是否继续校准按钮
        private bool ContiUpdate = false;
        //定义校验的值
        private double CheckX;
        private double Checkzz;
        private double Checkxx;
        private double Checkxy;
        //是否写入按钮 区分校验和校准
        private bool _IsWrite;
        private Task _Mianpro;
        private Task _ReadKukapro;
        private  bool _disable;
        private  bool _disableMain = true;
        private UaClient _UaClient;
        private ApplicationInstance application;
        private List<NodeId> ltNode = new List<NodeId>();
        private List<NodeId> ltNodeCrile = new List<NodeId>();
        private List<string> ltNode1 = new List<string>();
        //是否更新
        private bool _ControlUpdate;
        //kukas index
        private int _RobotsIndex = 0;
        private int _RobotsToolIndex = 0;
        //reset
        private bool _SocketReset;
        //代表在当前步只执行一次
        private bool _StepOnly;
        //是否写入校准值
        private bool writevalve;
        private bool UpOk = false;   //是否是上圆
        private bool DownOk = false;  //是否是下圆
        private bool ThreeOK = false;  //是否是下圆
        private bool Obliquecircle = false; //是否是斜圆
        private bool Obliquecircle2 = false; //是否是斜圆
        private double afterc = 0;
        private List<string> data = new List<string>();
        private DataValue data1;
        private object[] data2;
        //List<Point3D> upCr = new List<Point3D>(); //定义上圆的point集合
        //List<Point3D> DownCr = new List<Point3D>();  //定义下圆的point集合
        private byte[] FourBytes = new byte[4];
        //初始化sqlsguar的连接
        // private SqlSugarClient db = SqlHelper.ContextMaster("server = 127.0.0.1; Database=tcpdata;Uid=root;Pwd=12345;SslMode=none;CharSet=utf8mb4;");
        private int _MainStep = 0;
        private int _AutoStep = 0;
        private int _RunIndex = 0;
        private List<PointAndTime> upointList = new List<PointAndTime>();
        private List<PointAndTime> dpointList = new List<PointAndTime>();
        //最后一个计算xy补偿值的圆
        private List<PointAndTime> xypoint = new List<PointAndTime>();
        //采集x和y同时亮起的点位
        private List<PointAndTime> XndYList = new List<PointAndTime>();
        //斜圆的list1
        private List<PointAndTime> ObliquecircleList = new List<PointAndTime>();
        //斜圆的list2
        private List<PointAndTime> ObliquecircleList2 = new List<PointAndTime>();

        //elx
        private Workbook _workbook = new Workbook();
        private Worksheet _sheet = null;
        private int _cloum = 1;
        private object[] _testdata = new object[50];
        private DataTable _dataTable = new DataTable();

        //VAR
        private bool _Admin = false;
        private RobotConfigPara[] _RobotParaTemp = new RobotConfigPara[4];
        //PCI
        private bool _PCI_X;
        private bool _PCI_Y;
        private int _PCI_int;
        //socket
        private string Socketdata;
        private string[] _strs;
        //general
        private bool _SysReady;
        private string _RobotExcelAdress;
        private string _RobotName;
        private ManualResetEvent m = new ManualResetEvent(true); //实例化阻塞事件
        List<double> result = new List<double>();
        private string _BtnImagePath = AppDomain.CurrentDomain.BaseDirectory + "Image\\Lock.png";
        private string _ID;
        public ProcessFrm()
        {
            InitializeComponent();
        }
        private List<double> ReadTool(int CaTool)
        {
            try
            {
                result.Clear();
                if (_UaClient.Connected == false)
                {

                    return null;

                }
                else
                {
                    if(_KukaToolDatas != null)
                    {

                    
                    if (_KukaToolDatas[15] != null && _KukaToolDatas.Count >= 6)
                    {
                        result.Add(double.Parse(_KukaToolDatas[CaTool - 1][0]));
                        result.Add(double.Parse(_KukaToolDatas[CaTool - 1][1]));
                        result.Add(double.Parse(_KukaToolDatas[CaTool - 1][2]));
                        result.Add(double.Parse(_KukaToolDatas[CaTool - 1][3]));
                        result.Add(double.Parse(_KukaToolDatas[CaTool - 1][4]));
                        result.Add(double.Parse(_KukaToolDatas[CaTool - 1][5]));
                        return result;
                    }
                    else
                    {

                        return null;
                    }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteFile("ReadTool" + ex.ToString());
                return null;
            }
           
        }
        private void ProcessFrm_Load(object sender, EventArgs e)
        {
            try
            {

                //加载日志
                LogHelper.AddNewLogFile(_pathRoot, 3);
                //读取配置文件
                _cp = ReadConfig(_pathRoot);
                //初始化
                Init();
                StaticCommonVar.ReadTool += ReadTool;
                //Main
                Task.Run(() =>
                {
                    try
                    {
                        while (_disableMain)
                        {
                            Thread.Sleep(50);
                            m.WaitOne();
                            readCond();
                        }
                    }
                    catch (Exception ex)
                    { LogHelper.WriteFile("kuka信号采集线程异常" + ex.ToString()); }
                });

                _Mianpro = Task.Run(() =>
                {
                    Tooltemp Befortooltemp = new Tooltemp();
                    Tooltemp tool = new Tooltemp();
                    Tooltemp Calibrtion = new Tooltemp();
                    double[] toolValue = new double[6];
                    while (_disableMain)
                    {
                        try
                        {

                            Thread.Sleep(10);

                            switch (_MainStep)
                            {
                                case (int)MainStep.Init:

                                    Task.Run(General);
                                    Task.Run(() =>
                                    {
                                        try
                                        {
                                            while (_disableMain)
                                            {
                                                Thread.Sleep(10);
                                                m.WaitOne();
                                                DataSocket();

                                            }
                                        }
                                        catch (Exception ex)
                                        { LogHelper.WriteFile("线程异常" + ex.ToString()); }

                                    });

                                    InsetMsg("等待开始，请选择程序（R1/Tcp_Calibration/TCPMIAN）");
                                    // InsetMsg("请将机器人打成自动(速度在10内)后点击开始（检验）");
                                    InsetMsg("系统未就绪");
                                    _ControlUpdate = true;
                                    _GetComplete = false;
                                    _DataStep = 0;
                                    _MainStep = 1;

                                    break;
                                case (int)MainStep.WaitAuto:
                                    Zinput = "";
                                    _LightOK = false;
                                    _LightNG = false;
                                    _CACheck_x = true;
                                    _CACheck_y = true;
                                    _CACheck_xy = true;
                                    _RunIndex = 0;
                                    _RobotsToolIndex = 0;
                                    _Data = null;
                                    Invoke(new Action(() =>
                                    {
                                        // SysRunDisable(true);
                                       

                                    }));


                                    if (_SysReady)
                                    {
                                        if (RtAdjToolALab.Text != "无")
                                        {
                                            _CheckStart = false;
                                            _Start = false;
                                            tool.ToolA = Convert.ToDouble(RtAdjToolALab.Text);
                                            tool.ToolB = Convert.ToDouble(RtAdjToolBLab.Text);
                                            tool.ToolC = Convert.ToDouble(RtAdjToolCLab.Text);
                                            tool.ToolX = Convert.ToDouble(RtAdjToolXLab.Text);
                                            tool.ToolY = Convert.ToDouble(RtAdjToolYLab.Text);
                                            tool.ToolZ = Convert.ToDouble(RtAdjToolZLab.Text);
                                            Befortooltemp = tool;
                                            _Data = new Root();
                                            InsetMsg("系统准备完毕");
                                            _ControlUpdate = true;
                                            _MainStep = 2;

                                        }

                                    }

                                    break;
                                case 2:
                                    _StepOnly = true;

                                    if (_BtnStart || _AutoTest || _Start || _CheckStart)
                                    {
                                        if (_RobotSerial != _cp.Robots.RobotSeriorNo)
                                        {
                                            MessageBox.Show("当前机器人序列号和实际机器人序列号不匹配");
                                            _BtnStart = false;
                                            _Start = false;
                                            _AutoTest = false;
                                            _CheckStart = false;
                                            break;
                                        }

                                        ContiUpdate = false;

                                        InsetMsg("SETP2:系统运行开始");
                                        if (_AutoTestWrite && _AutoTest)
                                        {
                                            _IsWrite = true;//代表执行写入
                                            InsetMsg("当前为自动测试");
                                        }
                                        else if (!_AutoTestWrite && _AutoTest)
                                        {
                                            InsetMsg("当前为配置模式,不会写入任何数据并记录");
                                            _IsWrite = false;
                                        }
                                        else if (_Start)
                                        {
                                            _IsWrite = true;
                                        }
                                        else if (_CheckStart)
                                        {
                                            InsetMsg("检查机器人数据开始");
                                            _IsWrite = true;
                                        }


                                        Invoke(new Action(() =>
                                        {

                                            //textBox3.Text = "提示信息:运行中";
                                            //textBox3.BackColor = Color.Green;
                                            //textBox3.ForeColor = Color.Black;
                                            ////SysRunDisable(false);
                                            StaticCommonVar.SysStaus = true;
                                        }));
                                        _KukaInit = true;
                                        _ControlUpdate = true;
                                        MainStepGeneral();
                                        _MainStep = 31;
                                        Thread.Sleep(1000);

                                    }

                                    break;
                                case 3:

                                    if (_StepOnly)
                                    {
                                        _DataStep = 0;
                                        //工具坐标的值
                                        if (KukaToolData[5] != "")
                                        {
                                            Invoke(new Action(() =>
                                            {

                                                tool.ToolA = Convert.ToDouble(RtAdjToolALab.Text);
                                                tool.ToolB = Convert.ToDouble(RtAdjToolBLab.Text);
                                                tool.ToolC = Convert.ToDouble(RtAdjToolCLab.Text);
                                                tool.ToolX = Convert.ToDouble(RtAdjToolXLab.Text);
                                                tool.ToolY = Convert.ToDouble(RtAdjToolYLab.Text);
                                                tool.ToolZ = Convert.ToDouble(RtAdjToolZLab.Text);
                                                if (_TestStart)
                                                {
                                                    Befortooltemp = _Testtool;
                                                }
                                                else
                                                {
                                                    Befortooltemp = tool;
                                                }

                                                RotAdjBeToolALab.Text = double.Parse(RtAdjToolALab.Text).ToString("0.000");
                                                RotAdjBeToolBLab.Text = double.Parse(RtAdjToolBLab.Text).ToString("0.000");
                                                RotAdjBeToolCLab.Text = double.Parse(RtAdjToolCLab.Text).ToString("0.000");
                                                RotAdjBeToolXLab.Text = double.Parse(RtAdjToolXLab.Text).ToString("0.000");
                                                RotAdjBeToolYLab.Text = double.Parse(RtAdjToolYLab.Text).ToString("0.000");
                                                RotAdjBeToolZLab.Text = double.Parse(RtAdjToolZLab.Text).ToString("0.000");


                                            }));

                                        }
                                        InsetMsg("SETP3:启动数据获取脚本");
                                        _ControlUpdate = true;
                                        OpcuaWrite((int)KukaWriteMode.Start, toolnum: _cp.Robots.Tools[_RunIndex].CaToolNum, gippernum: _RunIndex + 1);
                                        Thread.Sleep(500);
                                        _StepOnly = false;
                                    }
                                    GetData("Start");
                                    if (_GetComplete && _OPCUA_STEP == 10)
                                    {
                                        InsetMsg($"SETP3:当前运行工具：{_RobotsToolIndex + 1}");
                                        InsetMsg("SETP3:夹取工件启动...");
                                        _ControlUpdate = true;
                                        _DataStep = 0;
                                        _TestStart = false;
                                        _MainStep = 4;
                                    }

                                    break;
                                case 31:

                                    if (_CheckStart)
                                    {
                                        //读取工具后赋值
                                        toolValue[3] = Convert.ToDouble(RtProToolXLab.Text);
                                        toolValue[4] = Convert.ToDouble(RtProToolYLab.Text);
                                        toolValue[5] = Convert.ToDouble(RtProToolZLab.Text);
                                        toolValue[0] = Convert.ToDouble(RtProToolALab.Text);
                                        toolValue[1] = Convert.ToDouble(RtProToolBLab.Text);
                                        toolValue[2] = Convert.ToDouble(RtProToolCLab.Text);
                                    }
                                    else
                                    {
                                        //读取工具后赋值
                                        toolValue[3] = Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].toolX);
                                        toolValue[4] = Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].toolY);
                                        toolValue[5] = Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].toolZ);
                                        toolValue[0] = Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].toolA);
                                        toolValue[1] = Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].toolB);
                                        toolValue[2] = Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].toolC);
                                    }



                                    int toolstep = 0;
                                    if (ModeWriteTool(toolValue, _cp.Robots.CaTool, toolstep))
                                    {
                                        Thread.Sleep(1000);
                                        _MainStep = 3;

                                    }


                                    break;
                                case 4:
                                    if (_OPCUA_GtpperErr)
                                    {
                                        InsetMsg("SETP4:系统错误,夹爪信号丢失");
                                        Invoke(new Action(() =>
                                        {
                                            //textBox3.Text = "提示信息:运行错误";
                                            //textBox3.BackColor = Color.Red;
                                        }));
                                        _ControlUpdate = true;
                                        _MainStep = 20;
                                    }
                                    //记录圆的点位 
                                    //readCond();
                                    if (_KukaComplete && _OPCUA_STEP == 20)
                                    {
                                        GetData("GetData");
                                        if (_GetComplete)
                                        {
                                            if (
                                                _Data.abc_do1 <= 0 || _Data.abc_do2 <= 0 || _Data.abc_do3 <= 0 || _Data.abc_do4 <= 0 || _Data.abc_do5 <= 0 ||
                                                _Data.abc_up1 <= 0 || _Data.abc_up2 <= 0 || _Data.abc_up3 <= 0 || _Data.abc_up4 <= 0 || _Data.abc_up5 <= 0
                                                )
                                            {
                                                InsetMsg("SETP4:系统错误,在校准角度时获取的采集数据有丢失");
                                                Invoke(new Action(() =>
                                                {
                                                    //textBox3.Text = "提示信息:运行错误";
                                                    //textBox3.BackColor = Color.Red;
                                                }));
                                                _ControlUpdate = true;
                                                _MainStep = 20;
                                                break;
                                            }
                                            _DataStep = 0;
                                            _MainStep = 5;
                                        }

                                    }
                                    if (_StepOnly)
                                    {
                                        InsetMsg("STEP4:系统采集ABC数据完成");
                                        _StepOnly = false;
                                        _ControlUpdate = true;
                                    }
                                    break;
                                case 5:

                                    ////算法计算
                                    ToolAbc _KukaTool = AlgorTool(_Data);
                                    InsetMsg($"SETP5:角度数据算法中");
                                    _ControlUpdate = true;
                                    _UaClient.ConnectAsync();
                                    Thread.Sleep(2000);
                               
                                    if (_UaClient.Connected && _IsWrite/* && _AutoTest == false*/)
                                    {
                                        tool.ToolA = _KukaTool.A;
                                        tool.ToolB = _KukaTool.B;
                                        tool.ToolC = _KukaTool.C;

                                        if (MessageBox.Show($"矫正后工具abc的值是\r\ntoola:{tool.ToolA},toolb:{ tool.ToolB},toolc:{tool.ToolC}\r\n是否继续矫正XYZ？",
                                       "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                        {
                                            if (!CheckTool(tool))
                                            {
                                                InsetMsg("SETP4:系统错误,计算ABC坐标时计算坐标异常");
                                                Invoke(new Action(() =>
                                                {
                                                    //textBox3.Text = "提示信息:运行错误";
                                                    //textBox3.BackColor = Color.Red;
                                                }));
                                                _ControlUpdate = true;
                                                _MainStep = 20;
                                                break;
                                            }
                                            else
                                            {
                                                OpcuaWrite((int)KukaWriteMode.Tool, tool);
                                                InsetMsg($"SETP5:代入库卡工具坐标");
                                                _ControlUpdate = true;
                                                Thread.Sleep(1000);
                                                _MainStep = 6;

                                            }
                                        }
                                        else {
                                            //不进行后面计算 直接从头开始
                                            _ControlUpdate = true;
                                            _MainStep = 20;
                                            break;
                                        }

                                       

                                    }
                                    else if (_UaClient.Connected == true && !_IsWrite)
                                    {
                                        tool = Befortooltemp;
                                        if (!CheckTool(tool))
                                        {
                                            InsetMsg("SETP4:系统错误,计算ABC坐标时计算坐标异常");
                                            Invoke(new Action(() =>
                                            {
                                                //textBox3.Text = "提示信息:运行错误";
                                                //textBox3.BackColor = Color.Red;
                                            }));
                                            _ControlUpdate = true;
                                            _MainStep = 20;
                                            break;
                                        }
                                        else
                                        {
                                            OpcuaWrite((int)KukaWriteMode.Tool, tool);
                                            InsetMsg($"SETP5:不代入库卡工具坐标");
                                            _ControlUpdate = true;
                                            Thread.Sleep(1000);
                                            _MainStep = 6;
                                        }
                                    }
                                

                                    break;
                                case 16:
                                    if (!_KukaComplete)
                                    {
                                        _MainStep = 6;
                                    }
                                    break;
                                case 6:

                                    if (_StepOnly)
                                    {
                                        _GetComplete = false;
                                        InsetMsg($"SETP6:XYZ角度校准数据采集...");
                                        _ControlUpdate = true;

                                        _StepOnly = false;
                                    }
                                    if (_OPCUA_STEP == 40 && _KukaComplete)
                                    {

                                        GetData("GetData");
                                        if (_GetComplete)
                                        {
                                            if (
                                                _Data.xyz_do1 <= 0 || _Data.xyz_do2 <= 0 || _Data.xyz_do3 <= 0 || _Data.xyz_do4 <= 0 || _Data.xyz_do5 <= 0
                                                )
                                            {
                                                InsetMsg("SETP6:系统错误,在校准角度时获取的采集数据有丢失");
                                                Invoke(new Action(() =>
                                                {

                                                    //textBox3.Text = "提示信息:运行错误";
                                                    //textBox3.BackColor = Color.Red;

                                                }));
                                                _ControlUpdate = true;
                                                _MainStep = 20;
                                                break;
                                            }
                                            InsetMsg($"SETP6:XYZ角度校准数据计算...");
                                            _ControlUpdate = true;

                                            List<PointAndTime> Endpoint = new List<PointAndTime>(); ;
                                            Endpoint = XorYTypes(xypoint);
                                            //改动
                                            ToolXYZ xyz = OffsetXY(_Data);
                                            //写入最后的XYZ值
                                            // _UaClient.ConnectAsync();
                                            //2024-09-27 在矫正过程中增加提示
                                            if (MessageBox.Show($"矫正后工具XYZ的值是\r\ntoolX:{tool.ToolX},toolY:{ tool.ToolY},toolZ:{tool.ToolZ}\r\n是否继续运行？",
                                      "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                            {

                                                if (_UaClient.Connected && _IsWrite/*&& !_AutoTest*/)
                                                {
                                                    tool.ToolX = xyz.X;
                                                    tool.ToolY = xyz.Y;
                                                    tool.ToolZ = xyz.Z;
                                                    if (!CheckTool(tool))
                                                    {
                                                        InsetMsg("SETP4:系统错误,计算XYZ坐标时计算坐标异常");
                                                        Invoke(new Action(() =>
                                                        {
                                                            //textBox3.Text = "提示信息:运行错误";
                                                            //textBox3.BackColor = Color.Red;
                                                        }));
                                                        _ControlUpdate = true;
                                                        _MainStep = 20;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        OpcuaWrite((int)KukaWriteMode.Tool, tool);
                                                        Thread.Sleep(2000);
                                                        _DataStep = 0;
                                                        _MainStep = 7;
                                                    }

                                                }
                                                else if (_UaClient.Connected && !_IsWrite)
                                                {
                                                    tool = Befortooltemp;
                                                    if (!CheckTool(tool))
                                                    {
                                                        InsetMsg("SETP4:系统错误,计算XYZ坐标时计算坐标异常");
                                                        Invoke(new Action(() =>
                                                        {
                                                            //textBox3.Text = "提示信息:运行错误";
                                                            //textBox3.BackColor = Color.Red;
                                                        }));
                                                        _ControlUpdate = true;
                                                        _MainStep = 20;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        OpcuaWrite((int)KukaWriteMode.Tool, tool);
                                                        Thread.Sleep(2000);
                                                        _DataStep = 0;
                                                        _MainStep = 7;
                                                    }
                                                }

                                            }
                                            else {
                                                //跳出循环 不继续执行
                                                _ControlUpdate = true;
                                                _MainStep = 20;
                                                break;
                                            }


                                        }

                                    }
                                    break;
                                case 7:

                                    Invoke(new Action(() =>
                                    {
                                        //改变后
                                        _testdata[21] = Double.Parse(RotAdjRealToolALab.Text.ToString());
                                        _testdata[22] = Double.Parse(RotAdjRealToolBLab.Text.ToString());
                                        _testdata[23] = Double.Parse(RotAdjRealToolCLab.Text.ToString());
                                        _testdata[24] = Double.Parse(tool.ToolX.ToString());
                                        _testdata[25] = Double.Parse(tool.ToolY.ToString());
                                        _testdata[26] = Double.Parse(tool.ToolZ.ToString());


                                        //改变前
                                        _testdata[27] = Double.Parse(RotAdjBeToolALab.Text.ToString());
                                        _testdata[28] = Double.Parse(RotAdjBeToolBLab.Text.ToString());
                                        _testdata[29] = Double.Parse(RotAdjBeToolCLab.Text.ToString());
                                        _testdata[30] = Double.Parse(RotAdjBeToolXLab.Text.ToString());
                                        _testdata[31] = Double.Parse(RotAdjBeToolYLab.Text.ToString());
                                        _testdata[32] = Double.Parse(RotAdjBeToolZLab.Text.ToString());
                                        _testdata[33] = Double.Parse(RtProToolALab.Text.ToString()); //pro_a
                                        _testdata[34] = Double.Parse(RtProToolBLab.Text.ToString());//pro_b
                                        _testdata[35] = Double.Parse(RtProToolCLab.Text.ToString());//pro_c
                                        _testdata[36] = Double.Parse(RtProToolXLab.Text.ToString());//pro_x
                                        _testdata[37] = Double.Parse(RtProToolYLab.Text.ToString());//pro_y
                                        _testdata[38] = Double.Parse(RtProToolZLab.Text.ToString());//pro_z
                                        if (_cp.Robots.AdjustmentZ == 0)
                                        {
                                            InsetMsg($"SETP7:不校准Z轴");
                                            _ControlUpdate = true;
                                            OpcuaWrite((int)KukaWriteMode.Befor);
                                            _MainStep = 11;

                                        }
                                        else
                                        {
                                            if (MessageBox.Show("是否校准z轴", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                            {

                                                InsetMsg($"SETP7:校准Z轴");
                                                _ControlUpdate = true;
                                                Zinput = Microsoft.VisualBasic.Interaction.InputBox("请输入内容：" + "\n" + "可根据现在测量手动填入" + "\n" + "点击确定则采用输入值校准(正数向上调整，负数向下调整)，点击取消则根据算法校准", "Z轴校准输入", "", -1, -1);

                                                _MainStep = 8;
                                            }
                                            else
                                            {
                                                InsetMsg($"SETP7:不校准Z轴");
                                                _ControlUpdate = true;
                                                OpcuaWrite((int)KukaWriteMode.Befor);
                                                _MainStep = 11;
                                            }
                                        }
                                    }));
                                    break;
                                case 8:

                                    if (_StepOnly)
                                    {

                                        _UaClient.ConnectAsync();
                                        _StepOnly = false;
                                    }
                                    if (_OPCUA_STEP == 65 && _KukaComplete)
                                    {
                                        GetData("GetData");
                                        if (_GetComplete)
                                        {
                                            var n = XndYList;

                                            if (_Data.ud_y_out > 0 && _Data.ud_x_in > 0)
                                            {
                                                // 2807  2807  2809
                                                //2917 2916 2915 2916 2978
                                                double timeO = ((double)_Data.ud_y_out - _Data.ud_x_in) / 10000;
                                                _testdata[20] = Double.Parse(timeO.ToString());
                                                //下降的标准时间 配置文件获取
                                                //string timebase = ConfigurationManager.AppSettings["timebase"].ToString();
                                                var testtime = timeO + Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.TimeBase);

                                                //系数固定 下降0.5m每秒 速度10%
                                                double EndOffsetFrist = testtime * 0.001 * 0.5 * 0.05 * 1000 / 1.515;
                                                //EndOffset+(自定义测量夹具与设置的标准夹具夹取的高度差)
                                                //往下就-  上就+
                                                double Zpoint;
                                                //
                                                XndYList.Clear();
                                                if (Zinput == "")
                                                {
                                                    InsetMsg($"不使用输入Z轴");
                                                    _ControlUpdate = true;
                                                    Zpoint = 0;
                                                    // z = Convert.ToDouble(endAgleZ.Text) - EndOffset * Math.Sin((Convert.ToDouble(AfterToolB.Text) / 180) * Math.PI);
                                                }
                                                else
                                                {
                                                    InsetMsg($"使用输入Z轴:{Zinput}");
                                                    _ControlUpdate = true;
                                                    Zpoint = Convert.ToDouble(Zinput);
                                                }

                                                double EndOffset = EndOffsetFrist + Zpoint;
                                                //x=x0 + dz*cos(a)*cos(b)
                                                var x = Convert.ToDouble(RotAdjRealToolXLab.Text)
                                                + EndOffset * Math.Cos((Convert.ToDouble(RotAdjRealToolALab.Text) / 180)
                                                * Math.PI) * Math.Cos(Convert.ToDouble(RotAdjRealToolBLab.Text) / 180 * Math.PI);
                                                // y = y0 + dz * cos(b) * sin(a)
                                                var y = Convert.ToDouble(RotAdjRealToolYLab.Text) + EndOffset * Math.Cos(Convert.ToDouble(RotAdjRealToolBLab.Text) / 180 * Math.PI) * Math.Sin(Convert.ToDouble(RotAdjRealToolALab.Text) / 180 * Math.PI);

                                                var z = Convert.ToDouble(RotAdjRealToolZLab.Text) - EndOffset * Math.Sin((Convert.ToDouble(RotAdjRealToolBLab.Text) / 180) * Math.PI);


                                                _UaClient.ConnectAsync();
                                                Thread.Sleep(2000);

                                                if (_UaClient.Connected == true && _IsWrite == true && !_AutoTest)
                                                {
                                                    tool.ToolX = x;
                                                    tool.ToolY = y;
                                                    tool.ToolZ = z;
                                                    if (!CheckTool(tool))
                                                    {
                                                        InsetMsg("SETP4:系统错误,计算Z时计算坐标异常");
                                                        Invoke(new Action(() =>
                                                        {
                                                            //textBox3.Text = "提示信息:运行错误";
                                                            //textBox3.BackColor = Color.Red;
                                                        }));
                                                        _ControlUpdate = true;
                                                        _MainStep = 20;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        OpcuaWrite((int)KukaWriteMode.Tool, tool);
                                                        Thread.Sleep(100);
                                                    }

                                                }
                                                else
                                                {
                                                    if (!CheckTool(tool))
                                                    {
                                                        InsetMsg("SETP4:系统错误,计算Z时计算坐标异常");
                                                        Invoke(new Action(() =>
                                                        {
                                                            //textBox3.Text = "提示信息:运行错误";
                                                            //textBox3.BackColor = Color.Red;
                                                        }));
                                                        _ControlUpdate = true;
                                                        _MainStep = 20;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        OpcuaWrite((int)KukaWriteMode.Tool, tool);
                                                        Thread.Sleep(100);

                                                        _GetComplete = false;
                                                        _DataStep = 0;
                                                        _MainStep = 9;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                InsetMsg("SETP6:系统错误,在校准角度时获取的采集数据有丢失");
                                                Invoke(new Action(() =>
                                                {
                                                    //textBox3.Text = "提示信息:运行错误";
                                                    //textBox3.BackColor = Color.Red;
                                                }));
                                                _ControlUpdate = true;
                                                _MainStep = 20;
                                                break;
                                            }

                                        }
                                    }

                                    break;
                                case 9:
                                    //绿灯亮起
                                    _LightNG = false;
                                    _LightOK = true;
                                    _MainStep = 11;

                                    _StepOnly = true;
                                    break;
                                case 10:


                                    break;
                                case 11:
                                    if (_OPCUA_STEP == 65 && _KukaComplete)
                                    {
                                        OpcuaWrite((int)KukaWriteMode.Befor);
                                    }

                                    if (_KukaComplete && _OPCUA_STEP == 100)
                                    {
                                        OpcuaWrite((int)KukaWriteMode.Befor);
                                        InsetMsg($"等待机器人放回校验工件");
                                        _ControlUpdate = true;
                                        _MainStep = 13;
                                    }
                                    if (_KukaComplete && _OPCUA_STEP == 150)
                                    {
                                       
                                        _MainStep = 13;
                                    }
                                    break;
                                case 13:
                                    if (_KukaComplete && _OPCUA_STEP == 150)
                                    {
                                        //if(_AutoTest)
                                        //{
                                        //    _MainStep = 18;
                                        //    break;
                                        //}
                                        _StepOnly = true;
                                        _MainStep = 15;
                                    }
                                    break;
                                case 15:
                                    if (_AutoTest == true)
                                    {
                                        //if (MessageBox.Show(
                                        //      $"以下是所有初始化数据\n工具的偏差值:\noffset_A:{offsetA.ToString("0.000")}°, offsetl_B:{offsetB.ToString("0.000")}°, offset_C:{offsetC.ToString("0.000")}°, \n" +
                                        //      $"offset_X:{offsetX.ToString("0.000")}mm, offset_Y:{offsetY.ToString("0.000")}mm, offset_Z:{offsetZ.ToString("0.000")}mm,\n" +
                                        //      $"点击确认将保存本次初始化的数据",
                                        //     "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                        //{

                                        //    LogHelper.WriteFile($"校准接受了数值");
                                        //    _testdata[42] = "Y";
                                        //    _MainStep = 17;
                                        //}

                                        OpcuaWrite((int)KukaWriteMode.Tool, tooltemp: Befortooltemp);
                                        _MainStep = 18;
                                        break;
                                    }
                                    Invoke(new Action(() =>
                                    {

                                        double offsetA = (Convert.ToDouble(RotAdjRealToolALab.Text) - Convert.ToDouble(RotAdjBeToolALab.Text));
                                        double offsetB = Convert.ToDouble(RotAdjRealToolBLab.Text) - Convert.ToDouble(RotAdjBeToolBLab.Text);
                                        double offsetC = Convert.ToDouble(RotAdjRealToolCLab.Text) - Convert.ToDouble(RotAdjBeToolCLab.Text);

                                        double offsetX = Convert.ToDouble(RotAdjRealToolXLab.Text) - Convert.ToDouble(RotAdjBeToolXLab.Text);
                                        double offsetY = Convert.ToDouble(RotAdjRealToolYLab.Text) - Convert.ToDouble(RotAdjBeToolYLab.Text);
                                        double offsetZ = Convert.ToDouble(RotAdjRealToolZLab.Text) - Convert.ToDouble(RotAdjBeToolZLab.Text);

                                        if (!_CheckSignal)
                                        {


                                            if (MessageBox.Show(
                                                 $"校验结束,以下是所有信息记录\n工具的偏差值:\noffset_A:{offsetA.ToString("0.000")}°, offsetl_B:{offsetB.ToString("0.000")}°, offset_C:{offsetC.ToString("0.000")}°, \n" +
                                                 $"offset_X:{offsetX.ToString("0.000")}mm, offset_Y:{offsetY.ToString("0.000")}mm, offset_Z:{offsetZ.ToString("0.000")}mm,\n" +
                                                 $"点击确认将保存本次对工具的修改，取消会还原本次对工具的修改",
                                                "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                            {

                                                LogHelper.WriteFile($"校准接受了数值");
                                                _testdata[42] = "Y";
                                                _MainStep = 17;
                                            }
                                            else
                                            {
                                                LogHelper.WriteFile($"校准拒绝了数值");
                                                _testdata[41] = "N";
                                                OpcuaWrite((int)KukaWriteMode.Befor);
                                                // OpcuaWrite((int)KukaWriteMode.Tool, tooltemp: Befortooltemp);
                                                _MainStep = 18;
                                            }

                                        }
                                        else
                                        {
                                            if ((Convert.ToDouble(_cp.Robots.CheckToolX) >= Math.Abs(offsetX) || Convert.ToDouble(_cp.Robots.CheckToolX) == 0) &&
                                              (Convert.ToDouble(_cp.Robots.CheckToolY) >= Math.Abs(offsetY) || Convert.ToDouble(_cp.Robots.CheckToolY) == 0) &&
                                              (Convert.ToDouble(_cp.Robots.CheckToolZ) >= Math.Abs(offsetZ) || Convert.ToDouble(_cp.Robots.CheckToolZ) == 0) &&
                                              (Convert.ToDouble(_cp.Robots.CheckToolA) >= Math.Abs(offsetA) || Convert.ToDouble(_cp.Robots.CheckToolA) == 0) &&
                                              (Convert.ToDouble(_cp.Robots.CheckToolB) >= Math.Abs(offsetB) || Convert.ToDouble(_cp.Robots.CheckToolB) == 0) &&
                                              (Convert.ToDouble(_cp.Robots.CheckToolC) >= Math.Abs(offsetC) || Convert.ToDouble(_cp.Robots.CheckToolC) == 0)
                                            )
                                            {
                                                if (MessageBox.Show(
                                                 $"检验结束,以下是所有信息记录\n工具的偏差值:\noffset_A:{offsetA.ToString("0.000")}°, offsetl_B:{offsetB.ToString("0.000")}°, offset_C:{offsetC.ToString("0.000")}°, \n" +
                                                 $"offset_X:{offsetX.ToString("0.000")}mm, offset_Y:{offsetY.ToString("0.000")}mm, offset_Z:{offsetZ.ToString("0.000")}mm,\n" +
                                                 $"此次检验偏差在设定范围内",
                                                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                                                {

                                                    OpcuaWrite((int)KukaWriteMode.Befor);
                                                    // OpcuaWrite((int)KukaWriteMode.Tool, tooltemp: Befortooltemp);
                                                    _MainStep = 18;
                                                }
                                                CheckLightLab.ForeColor = Color.Green;
                                            }
                                            else
                                            {
                                                if (MessageBox.Show(
                                                 $"检验结束,以下是所有信息记录\n工具的偏差值:\noffset_A:{offsetA.ToString("0.000")}°, offsetl_B:{offsetB.ToString("0.000")}°, offset_C:{offsetC.ToString("0.000")}°, \n" +
                                                 $"offset_X:{offsetX.ToString("0.000")}mm, offset_Y:{offsetY.ToString("0.000")}mm, offset_Z:{offsetZ.ToString("0.000")}mm,\n" +
                                                 $"此次检验偏差超出设定范围，请联系工程师校准",
                                                "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                                                {

                                                    OpcuaWrite((int)KukaWriteMode.Befor);
                                                    // OpcuaWrite((int)KukaWriteMode.Tool, tooltemp: Befortooltemp);
                                                    _MainStep = 18;
                                                }
                                                CheckLightLab.ForeColor = Color.Red;
                                            }
                                        }
                                        //_testdata[39] = _OPCUA_POSX.ToString();
                                        //_testdata[40] = _OPCUA_POSXY.ToString();
                                        //_testdata[41] = _OPCUA_POSY.ToString();
                                    }));
                                    break;
                                case 17:
                                    //  Thread.Sleep(8000);
                                    int toolstep17 = 0;
                                    toolValue[3] = Convert.ToDouble(RtAdjToolXLab.Text);
                                    toolValue[4] = Convert.ToDouble(RtAdjToolYLab.Text);
                                    toolValue[5] = Convert.ToDouble(RtAdjToolZLab.Text);
                                    toolValue[0] = Convert.ToDouble(RtAdjToolALab.Text);
                                    toolValue[1] = Convert.ToDouble(RtAdjToolBLab.Text);
                                    toolValue[2] = Convert.ToDouble(RtAdjToolCLab.Text);
                                    if (ModeWriteTool(toolValue, _cp.Robots.Tools[_RobotsToolIndex].CaToolNum, toolstep17, isWaitComplete: true))
                                    {
                                        Thread.Sleep(1000);
                                        _StepOnly = true;
                                        _MainStep = 18;
                                    }

                                    break;
                                case 18:
                                    GetData("End");
                                    if (_GetComplete)
                                    {
                                        _MainStep = 19;
                                    }
                                    break;
                                case 19:

                                    
                                        if (_StepOnly)
                                        {

                                            MainEnd();
                                            _RobotParaTemp[_RobotsToolIndex] = ConfigPara.DeepClone( _cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara);
                                            _RobotParaTemp[_RobotsToolIndex].XyzPre_X = _testdata[18].ToString();
                                            _RobotParaTemp[_RobotsToolIndex].XyzPre_Y = _testdata[19].ToString();
                                            _RobotParaTemp[_RobotsToolIndex].AbcPre_Z = _testdata[17].ToString();
                                            _RobotParaTemp[_RobotsToolIndex].AbcPre_Y = _testdata[16].ToString();
                                            _RobotParaTemp[_RobotsToolIndex].Point1 = _testdata[41].ToString();
                                            _RobotParaTemp[_RobotsToolIndex].Point2 = _testdata[40].ToString();
                                            _RobotParaTemp[_RobotsToolIndex].Point3 = _testdata[39].ToString();
                                            _StepOnly = false;
                                        }

                                        if (_RunIndex > _cp.Robots.Tools.Count)
                                        {
                                            OpcuaWrite(7, continueAdj: false);
                                            if (_OPCUA_STEP == 180)
                                            {
                                                _DataStep = 0;
                                                _CheckSignal = false;
                                                _RunIndex = 0;
                                                _MainStep = 1;
                                                break;
                                            }

                                        }
                                        if (_cp.Robots.Tools[_RunIndex].CaToolNum != 0)
                                        {

                                            OpcuaWrite(7, continueAdj: true);
                                            if (_OPCUA_STEP == 180)
                                            {
                                                _DataStep = 0;
                                                _MainStep = 2;
                                                _RobotsToolIndex++;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            OpcuaWrite(7, continueAdj: false);
                                            if (_OPCUA_STEP == 180)
                                            {
                                                if (_AutoTest)
                                                {

                                                

                                                    //Invoke(new Action(() =>
                                                    //{
                                                    //    if (MessageBox.Show("是否接受初始化配置运行的值？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                                    //    {
                                                    //        for (int i = 0; i <= _RobotsToolIndex; i++)
                                                    //        {
                                                    //            _cp.Robots.Tools[i].RobotConfigPara = _RobotParaTemp[i];
                                                    //        }
                                                    //        ConfigExcel.WriteConfig(_cp);
                                                    //    }
                                                    //}));


                                                }
                                                _DataStep = 0;
                                                _CheckSignal = false;
                                                _AutoTest = false;
                                                _MainStep = 1;

                                            //重复跑
                                            _AutoTestWrite = false;
                                            _AutoTest = true;
                                            break;
                                            }

                                        }
                                        // break;
                                    

                                    break;
                                //重复以上步骤但是校准tool2的值

                                case (int)MainStep.Err:


                                    break;
                                case 100://reset
                                    GetData("End");

                                    if (_KukaComplete)
                                    {
                                        if (Befortooltemp.ToolX == 0 || Befortooltemp.ToolZ == 0)
                                        {
                                            InsetMsg($"机器人复位失败，前工具变量为0");
                                            _ControlUpdate = true;

                                            _DataStep = 0;
                                            _MainStep = 1;

                                        }
                                        else
                                        {
                                            InsetMsg($"复位完成");
                                            _ControlUpdate = true;
                                            OpcuaWrite((int)KukaWriteMode.reset, ResetNum: 2, tooltemp: Befortooltemp);
                                            // TestData(_testdata);
                                        }
                                        _SystemRun = false;
                                        _disable = false;
                                        xypoint.Clear();
                                        //ok light
                                        _LightNG = false;
                                        _LightOK = false;
                                        //_UaClient.Disconnect();
                                        _Stopwatch.Stop();
                                        TimeSpan ts = _Stopwatch.Elapsed;
                                        // 格式化并输出时间间隔
                                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                        ts.Hours, ts.Minutes, ts.Seconds,
                                        ts.Milliseconds / 10);

                                        Invoke(new Action(() =>
                                        {
                                            TimeAll.Text = elapsedTime;
                                            StaticCommonVar.SysStaus = false;
                                        }));


                                        _DataStep = 0;
                                        _MainStep = 1;


                                    }
                                    else
                                    {
                                        _SystemRun = false;
                                        _disable = false;
                                        xypoint.Clear();


                                        //ok light
                                        _LightNG = false;
                                        _LightOK = false;
                                        //_UaClient.Disconnect();
                                        _Stopwatch.Stop();
                                        TimeSpan ts = _Stopwatch.Elapsed;
                                        // 格式化并输出时间间隔
                                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                        ts.Hours, ts.Minutes, ts.Seconds,
                                        ts.Milliseconds / 10);

                                        Invoke(new Action(() =>
                                        {
                                            TimeAll.Text = elapsedTime;
                                            StaticCommonVar.SysStaus = false;
                                        }));
                                        InsetMsg($"复位完成");
                                        _ControlUpdate = true;

                                        _DataStep = 0;
                                        _MainStep = 1;
                                    }
                                    break;





                            }

                        }
                        catch (Exception ex)
                        {

                            LogHelper.WriteFile("校准中异常" + ex.ToString());


                        }


                    }


                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteFile("初始化失败" + ex.ToString());
            }
        }

        /// <summary>
        /// 计算xy的偏差值的算法
        /// </summary>
        /// <param name="xyparam"></param>
        /// <returns></returns>
        //public ToolXYZ OffsetXY(Root xyparam)
        //{

        //    ToolXYZ result = new ToolXYZ();


        //    //  7540.  7542.
        //    double uallTotal = xyparam.xyz_do1;
        //    _testdata[11] = uallTotal;

        //    //  1890  1890
        //    double ua11 = xyparam.xyz_do2;
        //    _testdata[12] = ua11;

        //    // 1845  1846.
        //    double ua22 = xyparam.xyz_do3;
        //    _testdata[13] = ua22;

        //    //  1874  1874
        //    double ua33 = xyparam.xyz_do4;
        //    _testdata[14] = ua33;
        //    // 1929. 1930
        //    double ua44 = xyparam.xyz_do5;
        //    _testdata[15] = ua44;
        //    double ua1 = (double)(ua11) / uallTotal * (2 * Math.PI);
        //    double ua2 = (double)(ua22) / uallTotal * (2 * Math.PI);
        //    double ua3 = (double)(ua33) / uallTotal * (2 * Math.PI);
        //    double ua4 = (double)(ua44) / uallTotal * (2 * Math.PI);


        //    //三点定圆 三个固定的点
        //    // 提供的数据
        //    // 提供的数据
        //    //读取配置文件的点位
        //    //string strp1 = ConfigurationManager.AppSettings["point1"].ToString();
        //    //string[] po1 = strp1.Split(',');
        //    //string strp2 = ConfigurationManager.AppSettings["point2"].ToString();
        //    //string[] po2 = strp2.Split(',');
        //    //string strp3 = ConfigurationManager.AppSettings["point3"].ToString();
        //    //string[] po3 = strp3.Split(',');

        //    //string[] po1 = _cp.Robots[_RobotsIndex].RobotConfigPara.Point1.Split(',');
        //    //string[] po2 = _cp.Robots[_RobotsIndex].RobotConfigPara.Point2.Split(',');
        //    //string[] po3 = _cp.Robots[_RobotsIndex].RobotConfigPara.Point3.Split(',');
        //    //// 提供的数据
        //    //KukaPoint point1 = new KukaPoint { X = Convert.ToDouble(po1[0]), Y = Convert.ToDouble(po1[1]) };//294.616211
        //    //KukaPoint point2 = new KukaPoint { X = Convert.ToDouble(po2[0]), Y = Convert.ToDouble(po2[1]) };
        //    //KukaPoint point3 = new KukaPoint { X = Convert.ToDouble(po3[0]), Y = Convert.ToDouble(po3[1]) };
        //    ////279.616180
        //    //// 计算圆的半径
        //    //Circle circle = CalculateCircleRadius(point1, point2, point3);
        //    double R = CalculateCirc();
        //    ////y
        //    //var UDx = (R * Math.Cos((ua3 + ua2) / 2))-8;
        //    ////-z
        //    //var UDy = (R * Math.Cos((ua3 + ua4) / 2))-2;

        //    //补偿 -0.5625u  -1.61256
        //    var pre_UDx = R * Math.Cos((ua1 + ua4) / 2);
        //    //添加xyz的补偿值pre_UDx
        //    _testdata[18] = pre_UDx;
        //    //-1.36258  -1.66256
        //    var pre_UDy = R * Math.Cos((ua1 + ua2) / 2);
        //    //添加xyz的补偿值pre_UDy
        //    _testdata[19] = pre_UDy;

        //    //string xyz_pre_UDx = ConfigurationManager.AppSettings["xyz_pre_UDx"].ToString();
        //    //string xyz_pre_UDy = ConfigurationManager.AppSettings["xyz_pre_UDy"].ToString();
        //    var UDx = pre_UDx + Convert.ToDouble(_cp.Robots[_RobotsIndex].RobotConfigPara.XyzPre_X);
        //    var UDy = pre_UDy + Convert.ToDouble(_cp.Robots[_RobotsIndex].RobotConfigPara.XyzPre_Y);

        //    Checkxx = UDx;
        //    Checkxy = UDy;
        //    //工具坐标的值
        //    double toola = Convert.ToDouble(KukaToolData[3]);
        //    double toolb = Convert.ToDouble(KukaToolData[4]);
        //    double toolc = Convert.ToDouble(KukaToolData[5]);
        //    double toolX = Convert.ToDouble(KukaToolData[0]);
        //    double toolY = Convert.ToDouble(KukaToolData[1]);
        //    double toolZ = Convert.ToDouble(KukaToolData[2]);

        //    double testx = -UDx * (Math.Cos(toolc / 180 * Math.PI) * Math.Sin(toola / 180 * Math.PI)
        //        - Math.Cos(toola / 180 * Math.PI) * Math.Sin(toolb / 180 * Math.PI)
        //        * Math.Sin(toolc / 180 * Math.PI))
        //        - UDy * (Math.Sin(toola / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI)
        //        + Math.Cos(toola / 180 * Math.PI) * Math.Cos(toolc / 180 * Math.PI)
        //        * Math.Sin(toolb / 180 * Math.PI));

        //    double X = toolX + (testx);

        //    double testy = UDx * (Math.Cos(toola / 180 * Math.PI) *
        //        Math.Cos(toolc / 180 * Math.PI) + Math.Sin(toola / 180 * Math.PI)
        //        * Math.Sin(toolb / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI))
        //        + UDy * (Math.Cos(toola / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI)
        //        - Math.Cos(toolc / 180 * Math.PI) * Math.Sin(toola / 180 * Math.PI)
        //        * Math.Sin(toolb / 180 * Math.PI));
        //    double Y = toolY + (testy);

        //    double testz = UDx * Math.Cos(toolb / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI)
        //        - UDy * Math.Cos(toolb / 180 * Math.PI) * Math.Cos(toolc / 180 * Math.PI);

        //    double Z = toolZ + testz;

        //    result.X = X;
        //    result.Y = Y;
        //    result.Z = Z;
        //    //result.X = -0.0263;
        //    //result.Y = 0.2188;
        //    //result.Z = 242.021393;


        //    //result.X = -3.098;
        //    //result.Y = 0.827;
        //    //result.Z = 241.365;


        //    return result;



        //}

        #region xy偏差值计算 z朝下更新
        /// <summary>
        /// 计算xy的偏差值的算法
        /// </summary>
        /// <param name="xyparam"></param>
        /// <returns></returns>
        public ToolXYZ OffsetXY(Root xyparam)
        {

            ToolXYZ result = new ToolXYZ();


            //  7540.  7542.
            double uallTotal = xyparam.xyz_do1;
            _testdata[11] = uallTotal;

            //  1890  1890
            double ua11 = xyparam.xyz_do2;
            _testdata[12] = ua11;

            // 1845  1846.
            double ua22 = xyparam.xyz_do3;
            _testdata[13] = ua22;

            //  1874  1874
            double ua33 = xyparam.xyz_do4;
            _testdata[14] = ua33;
            // 1929. 1930
            double ua44 = xyparam.xyz_do5;
            _testdata[15] = ua44;
            double ua1 = (double)(ua11) / uallTotal * (2 * Math.PI);
            double ua2 = (double)(ua22) / uallTotal * (2 * Math.PI);
            double ua3 = (double)(ua33) / uallTotal * (2 * Math.PI);
            double ua4 = (double)(ua44) / uallTotal * (2 * Math.PI);



            double R = CalculateCirc();
            //补偿 -0.5625u  -1.61256
            var SenorPre_UDx = R * Math.Cos((ua3 + ua2) / 2);
            //添加xyz的补偿值pre_UDx

            //-1.36258  -1.66256
            var SenorPre_UDy = R * Math.Cos((ua3 + ua4) / 2);
            //添加xyz的补偿值pre_UDy

            var toolUPx = -SenorPre_UDy;
            var toolUPY = -SenorPre_UDx;
            _testdata[18] = toolUPx;
            _testdata[19] = toolUPY;
            var ToolUDx = toolUPx - Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_X);
            var ToolUDy = toolUPY - Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.XyzPre_Y);

            //Checkxx = UDx;
            //Checkxy = UDy;
            //工具坐标的值 radians = (Math.PI / 180) * degrees
            double toola = Convert.ToDouble(KukaToolData[3]) / 180 * Math.PI;
            double toolb = Convert.ToDouble(KukaToolData[4]) / 180 * Math.PI;
            double toolc = Convert.ToDouble(KukaToolData[5]) / 180 * Math.PI;
            double toolX = Convert.ToDouble(KukaToolData[0]);
            double toolY = Convert.ToDouble(KukaToolData[1]);
            double toolZ = Convert.ToDouble(KukaToolData[2]);


            double X = ToolUDx * Math.Cos(toola) * Math.Cos(toolb) + ToolUDy
                * (-Math.Sin(toola) * Math.Cos(toolc) + Math.Sin(toolb)
                * Math.Sin(toolc) * Math.Cos(toola)) + toolX;
            double Y = +ToolUDx * Math.Sin(toola) * Math.Cos(toolb) + ToolUDy * (Math.Sin(toola)
                * Math.Sin(toolb) * Math.Sin(toolc) + Math.Cos(toola) * Math.Cos(toolc)) + toolY;
            double Z = -ToolUDx * Math.Sin(toolb) + ToolUDy * Math.Sin(toolc) * Math.Cos(toolb) + toolZ;

            //double testx = -UDx * (Math.Cos(toolc / 180 * Math.PI) * Math.Sin(toola / 180 * Math.PI) - Math.Cos(toola / 180 * Math.PI) * Math.Sin(toolb / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI))
            //    - UDy * (Math.Sin(toola / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI) + Math.Cos(toola / 180 * Math.PI) * Math.Cos(toolc / 180 * Math.PI) * Math.Sin(toolb / 180 * Math.PI));
            //double X = toolX + (testx);

            //double testy = UDx * (Math.Cos(toola / 180 * Math.PI) * Math.Cos(toolc / 180 * Math.PI) + Math.Sin(toola / 180 * Math.PI) * Math.Sin(toolb / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI))
            //    + UDy * (Math.Cos(toola / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI) - Math.Cos(toolc / 180 * Math.PI) * Math.Sin(toola / 180 * Math.PI) * Math.Sin(toolb / 180 * Math.PI));
            //double Y = toolY + (testy);

            //double testz = UDx * Math.Cos(toolb / 180 * Math.PI) * Math.Sin(toolc / 180 * Math.PI)
            //    - UDy * Math.Cos(toolb / 180 * Math.PI) * Math.Cos(toolc / 180 * Math.PI);

            //double Z = toolZ + testz;

            result.X = X;
            result.Y = Y;
            result.Z = Z;
            //result.X = -0.0263;
            //result.Y = 0.2188;
            //result.Z = 242.021393;


            //result.X = -3.098;
            //result.Y = 0.827;
            //result.Z = 241.365;


            return result;



        }
        #endregion
        private bool CheckTool(Tooltemp tooltemp)
        {
            //update config file
            //if (tooltemp.ToolZ == 0 || tooltemp.ToolB == 0 || tooltemp.ToolB > 0)
            //{
            //    return false;
            //}
            return true;
        }
        public List<PointAndTime> XorYTypes(List<PointAndTime> pointList)
        {

            bool isXSignal = false;
            bool isYSignal = false;
            double xThreshold = 0;
            double yThreshold = 0;
            List<PointAndTime> EndPoint = new List<PointAndTime>();
            List<PointAndTime> Point1 = new List<PointAndTime>();
            for (int i = 0; i < pointList.Count - 1; i++)
            {
                var currentTime = pointList[i].TimeMillos;
                var nextTime = pointList[i + 1].TimeMillos;

                PointAndTime frist = new PointAndTime();
                frist.TimeMillos = currentTime;
                frist.IsWhat = pointList[i].IsWhat;
                Point1.Add(frist);
                if (pointList[i].IsWhat == "X" && pointList[i + 1].IsWhat == "Y")
                {
                    isXSignal = true;
                    isYSignal = false;
                    EndPoint.Add(Point1[0]);
                    Point1.Clear();
                }

                if (pointList[i].IsWhat == "Y" && pointList[i + 1].IsWhat == "X")
                {
                    isXSignal = false;
                    isYSignal = true;
                    EndPoint.Add(Point1[0]);
                    Point1.Clear();
                    //yThreshold = (currentTime + nextTime) / 2;
                    //Console.WriteLine($"Y信号临界点：时间 {yThreshold}");
                }
            }

            return EndPoint;
        }
        /// <summary>
        /// 根据三点计算圆心
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <returns></returns>
        static Circle CalculateCircleRadius(KukaPoint point1, KukaPoint point2, KukaPoint point3)
        {
            var k1 = (point2.X - point1.X) / (point2.Y - point1.Y);
            var a1 = (point2.X + point1.X) / 2;
            var b1 = (point2.Y + point1.Y) / 2;
            var k2 = (point3.X - point1.X) / (point3.Y - point1.Y);
            var a2 = (point3.X + point1.X) / 2;
            var b2 = (point3.Y + point1.Y) / 2;
            var Ox = (k2 * a2 + b2 - k1 * a1 - b1) / (k2 - k1);
            var Oy = k1 * a1 + b1 - k1 * ((k2 * a2 + b2 - k1 * a1 - b1) / (k2 - k1));
            var R = Math.Sqrt(Math.Pow((Ox - point1.X), 2) + Math.Pow((Oy - point1.Y), 2));
            return new Circle { Center = new KukaPoint { X = Ox, Y = Oy }, Radius = R };

        }

        /// <summary>
        /// 计算最后工具坐标的补偿值 工具z朝下
        /// </summary>
        public ToolAbc AlgorTool(Root param)
        {
            ToolAbc result = new ToolAbc();

            //  7540 7542.  7541  7539
            double uallTotal = param.abc_up1;
            _testdata[1] = uallTotal;
            // 1896 1900  1895. 1898
            double ua11 = param.abc_up2;
            _testdata[2] = ua11;
            //  1851 1849.  1850. 1851.
            double ua22 = param.abc_up3;
            _testdata[3] = ua22;
            //1865 1865.  1866.  1862.
            double ua33 = param.abc_up4;
            _testdata[4] = ua33;
            //1926 1926.  1928 1926.
            double ua44 = param.abc_up5;
            _testdata[5] = ua44;

            double ua1 = (double)(ua11 / uallTotal * (2 * Math.PI));
            double ua2 = (double)(ua22 / uallTotal * (2 * Math.PI));
            double ua3 = (double)(ua33 / uallTotal * (2 * Math.PI));
            double ua4 = (double)(ua44 / uallTotal * (2 * Math.PI));



            //7542
            double dallTotal = param.abc_do1;
            _testdata[6] = dallTotal;
            //1886
            double da11 = param.abc_do2;
            _testdata[7] = da11;
            //1847
            double da22 = param.abc_do3;
            _testdata[8] = da22;
            //1874
            double da33 = param.abc_do4;
            _testdata[9] = da33;
            //1934
            double da44 = param.abc_do5;
            _testdata[10] = da44;

            double da1 = (double)(da11) / dallTotal * (2 * Math.PI);
            double da2 = (double)(da22) / dallTotal * (2 * Math.PI);
            double da3 = (double)(da33) / dallTotal * (2 * Math.PI);
            double da4 = (double)(da44) / dallTotal * (2 * Math.PI);

            //string[] po1 = _cp.point1.Split(',');
            //string[] po2 = _cp.point2.Split(',');
            //string[] po3 = _cp.point3.Split(',');

            //// 提供的数据
            //KukaPoint point1 = new KukaPoint { X = Convert.ToDouble(po1[0]), Y = Convert.ToDouble(po1[1]) };//294.616211
            //KukaPoint point2 = new KukaPoint { X = Convert.ToDouble(po2[0]), Y = Convert.ToDouble(po2[1]) };
            //KukaPoint point3 = new KukaPoint { X = Convert.ToDouble(po3[0]), Y = Convert.ToDouble(po3[1]) };
            //KukaPoint point=
            ////// 提供的数据
            ////KukaPoint point1 = new KukaPoint { X = 249.319962, Y = -458.233276 };//294.616211
            ////KukaPoint point2 = new KukaPoint { X = 347.484863, Y = -526.643 };
            ////KukaPoint point3 = new KukaPoint { X = 340.725891, Y = -443.747284 };
            //// 计算圆的半径
            //Circle circle = CalculateCircleRadius(point1, point2, point3);
            double R = CalculateCirc();
            //8-18 xyz 补偿改动
            var UDx = R * Math.Cos((ua1 + ua4) / 2);
            var UDy = R * Math.Cos((ua1 + ua2) / 2);

            //以标定件为参考物 
            var SensorUpX = R * Math.Cos((ua3 + ua2) / 2);
            var SensorUpY = R * Math.Cos((ua3 + ua4) / 2);

            //工具和传感器坐标转换成统一方向
            var ToolUpX = -SensorUpY;
            var ToolUpY = -SensorUpX;

            //var UDx = pre_UDx - 5.94;
            // var UDy = pre_UDy - 1.35;

            var DDx = R * Math.Cos((da1 + da4) / 2);
            var DDy = R * Math.Cos((da1 + da2) / 2);

            var SensorDownX = R * Math.Cos((da3 + da2) / 2);
            var SensorDownY = R * Math.Cos((da3 + da4) / 2);

            //
            var ToolDownX = -SensorDownY;
            var ToolDownY = -SensorDownX;


            var offsetZ = Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.PointHeight);
            //补偿值
            //0.1749  0.0126
            //var pre_offsetY = -DDx + UDx;
            //
            // var pre_offsetY = ToolDownY - ToolUpY;
            var pre_offsetY = ToolUpY - ToolDownY;
            _testdata[16] = pre_offsetY;
            //补偿值 
            //var pre_offsetX = ToolDownX - ToolUpX;
            var pre_offsetX = ToolUpX - ToolDownX;
            //加一个相反数  -0.1998
            var offsetY = pre_offsetY - Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Y);
            var offsetX = pre_offsetX - Convert.ToDouble(_cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.AbcPre_Z);
            _testdata[17] = pre_offsetX;

            // var offsetZ = Convert.ToDouble(_cp.Robots[_RobotsIndex].RobotConfigPara.PointHeight);
            //读取配置文件的补偿值
            //string abc_offsetZ = ConfigurationManager.AppSettings["abc_offsetZ"].ToString();


            //加一个相反数 -0.005  0.09231


            //CheckX = offsetY;
            // Checkzz = offsetZ;

            //绕x轴  38.0028267 31.9116325
            double angleA;
            //绕y轴
            double angleB;
            if (Math.Sqrt(Math.Pow(offsetX, 2) + Math.Pow(offsetY, 2)) == 0)
            {
                angleA = 0;
                angleB = 0;
            }
            else
            {
                //angleA = -(Math.Abs(offsetX)) / offsetX * Math.Acos(offsetY / Math.Sqrt(Math.Pow(offsetY, 2) + Math.Pow(offsetY, 2)));

                angleA = -(Math.Abs(offsetX)) / offsetX * Math.Acos(offsetY / Math.Sqrt(Math.Pow(offsetX, 2) + Math.Pow(offsetY, 2)));
                double AA = angleA * 180 / Math.PI;
                angleB = -Math.Acos(offsetZ / Math.Sqrt(Math.Pow(offsetX, 2) + Math.Pow(offsetY, 2) + +Math.Pow(offsetZ, 2)));
                double BB = angleB * 180 / Math.PI;


            }

            double angleC = -angleA;
            ////工具坐标的值
            double toola = Convert.ToDouble(KukaToolData[3]);
            double toolb = Convert.ToDouble(KukaToolData[4]);
            double toolc = Convert.ToDouble(KukaToolData[5]);
            //data[12]
            //data[13]
            var A0 = Convert.ToDouble((toola / 180) * Math.PI);
            double B0 = Convert.ToDouble((toolb / 180) * Math.PI);
            var C0 = Convert.ToDouble((toolc / 180) * Math.PI);



            var T11 = -(((Math.Sin(A0) * Math.Cos(C0) - Math.Sin(B0) * Math.Sin(C0) * Math.Cos(A0)) * Math.Cos(angleA) + Math.Sin(angleA) * Math.Cos(A0) * Math.Cos(B0))
                * Math.Cos(angleB) - (Math.Sin(A0) * Math.Sin(C0) + Math.Sin(B0) * Math.Cos(A0) * Math.Cos(C0)) * Math.Sin(angleB)) * Math.Sin(angleC) - ((Math.Sin(A0)
                * Math.Cos(C0) - Math.Sin(B0) * Math.Sin(C0) * Math.Cos(A0)) * Math.Sin(angleA) - Math.Cos(angleA) * Math.Cos(A0) * Math.Cos(B0)) * Math.Cos(angleC);

            var T21 = (((Math.Sin(A0) * Math.Sin(B0) * Math.Sin(C0) + Math.Cos(A0) * Math.Cos(C0)) * Math.Cos(angleA)
                - Math.Sin(angleA) * Math.Sin(A0) * Math.Cos(B0)) * Math.Cos(angleB) + (Math.Sin(A0) * Math.Sin(B0) * Math.Cos(C0) -
                Math.Sin(C0) * Math.Cos(A0)) * Math.Sin(angleB)) * Math.Sin(angleC)
                + ((Math.Sin(A0) * Math.Sin(B0) * Math.Sin(C0) + Math.Cos(A0) * Math.Cos(C0))
                * Math.Sin(angleA) + Math.Sin(A0) * Math.Cos(angleA) * Math.Cos(B0)) * Math.Cos(angleC);


            var T31 = ((Math.Sin(angleA) * Math.Sin(B0) + Math.Sin(C0) * Math.Cos(angleA) * Math.Cos(B0)) * Math.Cos(angleB)
                + Math.Sin(angleB) * Math.Cos(B0) * Math.Cos(C0)) * Math.Sin(angleC) + (Math.Sin(angleA) *
                Math.Sin(C0) * Math.Cos(B0) - Math.Sin(B0) * Math.Cos(angleA)) * Math.Cos(angleC);



            var T32 = ((Math.Sin(angleA) * Math.Sin(B0) + Math.Sin(C0) * Math.Cos(angleA) * Math.Cos(B0))
                * Math.Cos(angleB) + Math.Sin(angleB) * Math.Cos(B0)
                * Math.Cos(C0)) * Math.Cos(angleC) - (Math.Sin(angleA) * Math.Sin(C0) * Math.Cos(B0) - Math.Sin(B0) * Math.Cos(angleA)) * Math.Sin(angleC);

            var T33 = -(Math.Sin(angleA) * Math.Sin(B0) + Math.Sin(C0) * Math.Cos(angleA) * Math.Cos(B0))
                * Math.Sin(angleB) + Math.Cos(angleB) * Math.Cos(B0) * Math.Cos(C0);
            var b1 = Math.Asin(-T31);
            var c1 = Math.Atan2(T32, T33);
            var a1 = Math.Atan2(T21, T11);
            result.A = a1 * (180 / Math.PI);
            result.B = b1 * (180 / Math.PI);
            result.C = c1 * (180 / Math.PI);
            //result.a = 0;
            //result.b = -90;
            //result.c = 0;
            //result.a = -60.138752;
            //result.b = -89.4789352;
            //result.c = 60.1356888;
            return result;
        }
        public double CalculateCirc()
        {
            string[] po1 = _cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point1.Split(',');
            string[] po2 = _cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point2.Split(',');
            string[] po3 = _cp.Robots.Tools[_RobotsToolIndex].RobotConfigPara.Point3.Split(',');
            // 提供的数据
            KukaPoint point1 = new KukaPoint { X = Convert.ToDouble(po1[0]), Y = Convert.ToDouble(po1[1]) };//294.616211
            KukaPoint point2 = new KukaPoint { X = Convert.ToDouble(po2[0]), Y = Convert.ToDouble(po2[1]) };
            KukaPoint point3 = new KukaPoint { X = Convert.ToDouble(po3[0]), Y = Convert.ToDouble(po3[1]) };
            //279.616180
            // 计算圆的半径
            Circle circle;
            if (point2.Y - point1.Y == 0)
            {
                // 计算圆的半径
                circle = CalculateCircleRadius(point3, point2, point1);
            }
            else if (point3.Y - point1.Y == 0)
            {
                // 计算圆的半径
                circle = CalculateCircleRadius(point2, point1, point3);
            }
            else
            {
                circle = CalculateCircleRadius(point1, point2, point3);
            }

            return circle.Radius;
        }
        /// <summary>
        /// 计算最后工具坐标的补偿值
        /// </summary>
        //public ToolAbc AlgorTool(Root param)
        //{
        //    ToolAbc result = new ToolAbc();

        //    //  7540 7542.  7541  7539
        //    double uallTotal = param.abc_up1;
        //    _testdata[1] = uallTotal;
        //    // 1896 1900  1895. 1898
        //    double ua11 = param.abc_up2;
        //    _testdata[2] = ua11;
        //    //  1851 1849.  1850. 1851.
        //    double ua22 = param.abc_up3;
        //    _testdata[3] = ua22;
        //    //1865 1865.  1866.  1862.
        //    double ua33 = param.abc_up4;
        //    _testdata[4] = ua33;
        //    //1926 1926.  1928 1926.
        //    double ua44 = param.abc_up5;
        //    _testdata[5] = ua44;

        //    double ua1 = (double)(ua11 / uallTotal * (2 * Math.PI));
        //    double ua2 = (double)(ua22 / uallTotal * (2 * Math.PI));
        //    double ua3 = (double)(ua33 / uallTotal * (2 * Math.PI));
        //    double ua4 = (double)(ua44 / uallTotal * (2 * Math.PI));



        //    //7542
        //    double dallTotal = param.abc_do1;
        //    _testdata[6] = dallTotal;
        //    //1886
        //    double da11 = param.abc_do2;
        //    _testdata[7] = da11;
        //    //1847
        //    double da22 = param.abc_do3;
        //    _testdata[8] = da22;
        //    //1874
        //    double da33 = param.abc_do4;
        //    _testdata[9] = da33;
        //    //1934
        //    double da44 = param.abc_do5;
        //    _testdata[10] = da44;

        //    double da1 = (double)(da11) / dallTotal * (2 * Math.PI);
        //    double da2 = (double)(da22) / dallTotal * (2 * Math.PI);
        //    double da3 = (double)(da33) / dallTotal * (2 * Math.PI);
        //    double da4 = (double)(da44) / dallTotal * (2 * Math.PI);

        //    //string[] po1 = _cp.point1.Split(',');
        //    //string[] po2 = _cp.point2.Split(',');
        //    //string[] po3 = _cp.point3.Split(',');

        //    //// 提供的数据
        //    //KukaPoint point1 = new KukaPoint { X = Convert.ToDouble(po1[0]), Y = Convert.ToDouble(po1[1]) };//294.616211
        //    //KukaPoint point2 = new KukaPoint { X = Convert.ToDouble(po2[0]), Y = Convert.ToDouble(po2[1]) };
        //    //KukaPoint point3 = new KukaPoint { X = Convert.ToDouble(po3[0]), Y = Convert.ToDouble(po3[1]) };
        //    //KukaPoint point=
        //    ////// 提供的数据
        //    ////KukaPoint point1 = new KukaPoint { X = 249.319962, Y = -458.233276 };//294.616211
        //    ////KukaPoint point2 = new KukaPoint { X = 347.484863, Y = -526.643 };
        //    ////KukaPoint point3 = new KukaPoint { X = 340.725891, Y = -443.747284 };
        //    //// 计算圆的半径
        //    //Circle circle = CalculateCircleRadius(point1, point2, point3);
        //    double R = CalculateCirc();
        //    //8-18 xyz 补偿改动
        //    var UDx = R * Math.Cos((ua1 + ua4) / 2);
        //    var UDy = R * Math.Cos((ua1 + ua2) / 2);


        //    //var UDx = R * Math.Cos((ua3 + ua2) / 2);
        //    //var UDy = R * Math.Cos((ua3 + ua4) / 2);
        //    //var UDx = pre_UDx-5.94;
        //    //var UDy = pre_UDy-1.35;

        //    var DDx = R * Math.Cos((da1 + da4) / 2);
        //    var DDy = R * Math.Cos((da1 + da2) / 2);
        //    var offsetX = Convert.ToDouble(_cp.Robots[_RobotsIndex].RobotConfigPara.PointHeight);
        //    //补偿值
        //    //0.1749  0.0126
        //    var pre_offsetY = -DDx + UDx;
        //    _testdata[16] = pre_offsetY;
        //    //加一个相反数  -0.1998
        //    var offsetY = pre_offsetY + Convert.ToDouble(_cp.Robots[_RobotsIndex].RobotConfigPara.AbcPre_Y);

        //    //读取配置文件的补偿值
        //    //string abc_offsetZ = ConfigurationManager.AppSettings["abc_offsetZ"].ToString();

        //    //补偿值 
        //    var pre_offsetZ = DDy - UDy;
        //    //加一个相反数 -0.005  0.09231
        //    var offsetZ = pre_offsetZ + Convert.ToDouble(_cp.Robots[_RobotsIndex].RobotConfigPara.AbcPre_Z);
        //    _testdata[17] = pre_offsetZ;

        //    CheckX = offsetY;
        //    Checkzz = offsetZ;

        //    //绕x轴  38.0028267 31.9116325
        //    double angleA;
        //    //绕y轴
        //    double angleB;
        //    if (Math.Sqrt(Math.Pow(offsetX, 2) + Math.Pow(offsetY, 2)) == 0)
        //    {
        //        angleA = 0;
        //        angleB = 0;
        //    }
        //    else
        //    {
        //        angleA = -(Math.Abs(offsetY)) / offsetY * Math.Acos(offsetZ / Math.Sqrt(Math.Pow(offsetZ, 2) + Math.Pow(offsetY, 2)));
        //        double AA = angleA * 180 / Math.PI;
        //        angleB = -Math.Acos(offsetX / Math.Sqrt(Math.Pow(offsetX, 2) + Math.Pow(offsetY, 2) + +Math.Pow(offsetZ, 2)));
        //        double BB = angleB * 180 / Math.PI;

        //    }

        //    double angleC = -angleA;
        //    ////工具坐标的值
        //    double toola = Convert.ToDouble(KukaToolData[3]);
        //    double toolb = Convert.ToDouble(KukaToolData[4]);
        //    double toolc = Convert.ToDouble(KukaToolData[5]);
        //    //data[12]
        //    //data[13]
        //    var A0 = Convert.ToDouble((toola / 180) * Math.PI);
        //    double B0 = Convert.ToDouble((toolb / 180) * Math.PI);
        //    var C0 = Convert.ToDouble((toolc / 180) * Math.PI);



        //    var T11 = Math.Cos(A0) * Math.Cos(angleB) * Math.Cos(B0) - Math.Sin(angleB) * (Math.Cos(angleA) * (Math.Sin(A0) * Math.Sin(C0) + Math.Cos(A0) * Math.Cos(C0) * Math.Sin(B0))
        //                   + Math.Sin(angleA) * (Math.Cos(C0) * Math.Sin(A0) - Math.Cos(A0) * Math.Sin(B0) * Math.Sin(C0)));

        //    var T21 = Math.Sin(angleB) * (Math.Cos(angleA) * (Math.Cos(A0) * Math.Sin(C0) - Math.Cos(C0) * Math.Sin(A0) * Math.Sin(B0)) + Math.Sin(angleA) * (Math.Cos(A0) * Math.Cos(C0) + Math.Sin(A0)
        //        * Math.Sin(B0) * Math.Sin(C0))) + Math.Cos(B0) * Math.Cos(angleB) * Math.Sin(A0);


        //    var T31 = -Math.Sin(angleB) * (Math.Cos(angleA) * Math.Cos(B0) * Math.Cos(C0) - Math.Cos(B0)
        //        * Math.Sin(angleA) * Math.Sin(C0)) - Math.Cos(angleB) * Math.Sin(B0);
        //    var b1 = Math.Asin(-T31);

        //    var T32 = Math.Cos(angleC) * (Math.Cos(angleA) * Math.Cos(B0) * Math.Sin(C0) + Math.Cos(B0)
        //        * Math.Cos(C0) * Math.Sin(angleA)) + Math.Sin(angleC)
        //        * (Math.Cos(angleB) * (Math.Cos(angleA) * Math.Cos(B0) * Math.Cos(C0) - Math.Cos(B0) * Math.Sin(angleA)
        //        * Math.Sin(C0)) - Math.Sin(angleB) * Math.Sin(B0));

        //    var T33 = Math.Cos(angleC) * (Math.Cos(angleB) * (Math.Cos(angleA) * Math.Cos(B0) * Math.Cos(C0) - Math.Cos(B0) * Math.Sin(angleA) * Math.Sin(C0))
        //        - Math.Sin(angleB) * Math.Sin(B0)) - Math.Sin(angleC) * (Math.Cos(angleA) * Math.Cos(B0) * Math.Sin(C0) + Math.Cos(B0) * Math.Cos(C0) * Math.Sin(angleA));
        //    var c1 = Math.Atan2(T32, T33);
        //    var a1 = Math.Atan2(T21, T11);
        //    result.A = a1 * (180 / Math.PI);
        //    result.B = b1 * (180 / Math.PI);
        //    result.C = c1 * (180 / Math.PI);
        //    //result.a = 0;
        //    //result.b = -90;
        //    //result.c = 0;
        //    //result.a = -60.138752;
        //    //result.b = -89.4789352;
        //    //result.c = 60.1356888;
        //    return result;
        //}

        public void ExitFun()
        {
            _disable = false;
            _disableMain = false;
           StaticCommonVar. _Exit = false;
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(50);
                    if (_Mianpro == null)
                    {
                        Invoke(new Action(() =>
                        {

                            System.Windows.Forms.Application.Exit();
                        }));
                        break;
                    }
                    if (_Mianpro.IsCompleted)
                    {
                        _Mianpro.Dispose();
                        Invoke(new Action(() =>
                        {

                            System.Windows.Forms.Application.Exit();
                        }));

                    }
                }
            });
        }
        public void readCond()
        {
            try
            {

                if (_UaClient.Connected&&!StaticCommonVar.RobotChangeOver)
                {
                    data.Clear();
                    //配置库卡夹爪
                    if (_KukaInit)
                    {
                        OpcuaWrite((int)KukaWriteMode.Init);
                        _KukaInit = false;
                    }

                    dataValues = _UaClient.ReadNodes(ltNode.ToArray());
                    if (dataValues[0].Value != null)
                        //操作模式
                        data.Add(dataValues[0].Value.ToString());
                    if (dataValues[1].Value != null)
                        //程序状态
                        data.Add(dataValues[1].Value.ToString());
                    if (dataValues[15].Value != null)
                        _OPCUA_POSX = (bool)dataValues[15].Value;
                    if (dataValues[16].Value != null)
                        _OPCUA_POSXY = (bool)dataValues[16].Value;
                    if (dataValues[17].Value != null)
                        _OPCUA_POSY = (bool)dataValues[17].Value;
                    if (dataValues[21].Value != null)
                        _OPCUA_GtpperErr = (bool)dataValues[21].Value;
                    if (dataValues[22].Value != null)
                        _RobotSerial = dataValues[22].Value.ToString();

                    if ((double[])dataValues[9].Value != null)
                    {
                        _KukaPoint[0] = ((double[])dataValues[9].Value)[0];
                        _KukaPoint[1] = ((double[])dataValues[9].Value)[1];
                        _KukaPoint[2] = ((double[])dataValues[10].Value)[0];
                        _KukaPoint[3] = ((double[])dataValues[10].Value)[1];
                        _KukaPoint[4] = ((double[])dataValues[11].Value)[0];
                        _KukaPoint[5] = ((double[])dataValues[11].Value)[1];
                        _KukaPoint[6] = ((double[])dataValues[19].Value)[0];
                        _KukaPoint[7] = ((double[])dataValues[19].Value)[1];


                    }

                    _KukaProStatus = data[1];
                    if (_KukaProStatus == "#P_ACTIVE")
                    {
                        _KukaProRun = true;
                    }
                    else
                    {
                        _KukaProRun = false;
                    }
                    _KukaMode = data[0];
                    if (dataValues[6].Value != null)
                        _KukaSpeed = dataValues[6].Value.ToString();
                    if (dataValues[18].Value != null)
                        _OPCUA_STEP = (int)dataValues[18].Value;
                    if (dataValues[8].Value != null)
                        _KukaComplete = (bool)dataValues[8].Value;

                    if (dataValues[7].Value != null)
                    {
                        _KukaProName = dataValues[7].Value.ToString();
                    }
                    //if (dataValues[5].Value != null)
                    //{
                    //    bool[] EndRes = (bool[])dataValues[5].Value;
                    //    if (EndRes[0] == true) { UpOk = true; DownOk = false; }
                    //    if (EndRes[1] == true) { UpOk = false; DownOk = true; }
                    //    if (EndRes[0] == false && EndRes[1] == false) { UpOk = false; DownOk = false; }
                    //    if (EndRes[0] == false && EndRes[1] == false && EndRes[2] == true) { UpOk = false; DownOk = false; ThreeOK = true; }
                    //    if (EndRes[0] == false && EndRes[1] == false && EndRes[2] == false) { UpOk = false; DownOk = false; ThreeOK = false; }
                    //    if (EndRes[0] == true && EndRes[1] == false && EndRes[2] == false) { UpOk = true; DownOk = false; ThreeOK = false; }
                    //}
                    if (KukaToolData[5] != "")
                    {
                        _KukaModeToolA = Convert.ToDouble(KukaToolData[3]);
                        _KukaModeToolB = Convert.ToDouble(KukaToolData[4]);
                        _KukaModeToolC = Convert.ToDouble(KukaToolData[5]);
                        _KukaModeToolX = Convert.ToDouble(KukaToolData[0]);
                        _KukaModeToolY = Convert.ToDouble(KukaToolData[1]);
                        _KukaModeToolZ = Convert.ToDouble(KukaToolData[2]);

                    }

                    else
                    {
                        _KukaProName = "未选择程序";
                    }

                    #region //实际坐标读取 暂时没用上 注释
                    //if (dataValues[2].Value != null)
                    //        {
                    //            if (dataValues[2].Value.GetType() == typeof(ExtensionObject) || dataValues[2].Value.GetType() == typeof(ExtensionObject[]))
                    //            {
                    //                int num = 1;
                    //                int index = 0;
                    //                byte[] bytes = new byte[48]; // 字节数组
                    //                byte[] bytes2 = new byte[8] { 192, 61, 207, 159, 54, 254, 128, 64 }; // 字节数组
                    //                                                                                     //for (int i = 0; i < ((dynamic)data.WrappedValue.Value).ToList<object>(data.Value).Length; i++)
                    //            if (dataValues[2].Value.GetType() == typeof(ExtensionObject))
                    //            {
                    //                for (int i = 0; i < 48; i++)//((dynamic)(data.WrappedValue.Value)).Body.Length
                    //                {
                    //                    bytes[i] = ((dynamic)(dataValues[2].WrappedValue.Value)).Body[i];
                    //                    if (8 * num == i || 8 * num == i + 1)
                    //                    {

                    //                        Array.Copy(bytes, index, bytes2, 0, 8);
                    //                        double floatValue = BitConverter.ToDouble(bytes2, 0); // 转换成浮点数
                    //                        data.Add(floatValue.ToString());
                    //                        // Console.WriteLine(floatValue); // 输出转换结果
                    //                        num += 1;
                    //                        index += 8;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //       }
                    #endregion 实际坐标读取结束
                    #region //工具坐标读取
                    if (dataValues[3].Value != null)
                    {
                        data1 = dataValues[3];
                        if (dataValues[3].Value.GetType() == typeof(ExtensionObject) || dataValues[3].Value.GetType() == typeof(ExtensionObject[]))
                        {
                            int num = 1;
                            int index = 0;
                            int dataindex = 0;
                            byte[] bytes = new byte[48]; // 字节数组
                            byte[] bytes2 = new byte[8]; // 字节数组
                                                         //for (int i = 0; i < ((dynamic)data.WrappedValue.Value).ToList<object>(data.Value).Length; i++)
                            if (dataValues[3].Value.GetType() == typeof(ExtensionObject))
                            {
                                for (int i = 0; i < 48; i++)//((dynamic)(data.WrappedValue.Value)).Body.Length
                                {
                                    bytes[i] = ((dynamic)(dataValues[3].WrappedValue.Value)).Body[i];
                                    if (8 * num == i || 8 * num == i + 1)
                                    {

                                        Array.Copy(bytes, index, bytes2, 0, 8);
                                        double floatValue = BitConverter.ToDouble(bytes2, 0); // 转换成浮点数
                                        KukaToolData[dataindex] = floatValue.ToString();
                                        dataindex += 1;
                                        //  data.Add(floatValue.ToString());
                                        // Console.WriteLine(floatValue); // 输出转换结果
                                        num += 1;
                                        index += 8;
                                    }
                                }
                            }
                        }
                    }

                    if (dataValues[20].Value != null)
                    {
                        _KukaToolDatas.Clear();
                        var data = ((dynamic)(dataValues[20].Value));
                        string[] strings = new string[6];
                        for (int i = 0; i < data.Length; i++)
                        {
                            // data1 = data[i];

                            int num = 1;
                            int index = 0;
                            int dataindex = 0;
                            byte[] bytes = new byte[48]; // 字节数组
                            byte[] bytes2 = new byte[8]; // 字节数组
                                                         //for (int i = 0; i < ((dynamic)data.WrappedValue.Value).ToList<object>(data.Value).Length; i++)

                            for (int j = 0; j < 48; j++)//((dynamic)(data.WrappedValue.Value)).Body.Length
                            {
                                bytes[j] = data[i].Body[j];
                                if (8 * num == j || 8 * num == j + 1)
                                {

                                    Array.Copy(bytes, index, bytes2, 0, 8);
                                    double floatValue = BitConverter.ToDouble(bytes2, 0); // 转换成浮点数
                                    strings[dataindex] = floatValue.ToString();
                                    dataindex += 1;
                                    //  data.Add(floatValue.ToString());
                                    // Console.WriteLine(floatValue); // 输出转换结果
                                    num += 1;
                                    index += 8;
                                }
                            }

                            _KukaToolDatas.Add(new string[] { strings[0], strings[1], strings[2], strings[3], strings[4], strings[5] });

                        }


                    }
                    #endregion 工具读取结束
                    #region //传感器io信号读取
                    //if (dataValues[4].Value != null)
                    //{
                    //    byte[] bytes = (byte[])dataValues[4].Value;
                    //    // 假设 bytes 数组已经被赋值
                    //    for (int i = 0; i < 4; i++)
                    //    {
                    //        //访问前四个元素并复制到另一个数组中
                    //        FourBytes[i] = bytes[i];

                    //    }

                    //    #region 根据传感器的io信号将机器人的实际位置存入点位和时间的集合


                    //    #endregion 根据传感器的io信号将机器人的实际位置存入数据库结束


                    //}
                    #endregion 传感器io信号读取结束
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteFile("读取库卡变量异常" + ex.ToString() + $"dataValues:{dataValues}" + $"_UaClient.Connected:{_UaClient.Connected}");
            }

        }
        public bool HisWriTool(double[] doubles, int toolnum, int step)
        {

            //  int step=0;
            return ModeWriteTool(doubles, toolnum, step);
        }
        private bool ModeWriteTool(double[] doubles, int toolnum, int step, bool isWaitComplete = false, bool continueadj = false)
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(200);
                    switch (step)
                    {

                        case 0:
                            if (_UaClient.Connected && /*_KukaProName == "TCPMAIN" && */(!_KukaComplete || isWaitComplete) && _KukaProRun)
                            {
                                OpcuaWrite(5, tooltemp: new Tooltemp() { ToolX = doubles[3], ToolY = doubles[4], ToolZ = doubles[5], ToolA = doubles[0], ToolB = doubles[1], ToolC = doubles[2] }, toolnum: toolnum);

                                step = 1;
                                Thread.Sleep(100);
                                break;
                            }
                            else
                            {
                                return false;
                            }

                        case 1:
                            OpcuaWrite(6);
                            step = 2;
                            Thread.Sleep(500);
                            break;
                        case 2:

                            if (!_KukaComplete)
                            {
                                step = 3;
                            }
                            break;
                        case 3:


                            Thread.Sleep(500);
                            return true;
                    }
                }

                catch (Exception)
                {


                }

            }
        }
        public void OpcuaWrite(int mode, Tooltemp tooltemp = new Tooltemp(), int speed = 0, int ResetNum = 0, int toolnum = 0, int gippernum = 0, bool continueAdj = false)
        {
            // _UaClient.ConnectAsync();
            if (_UaClient.Connected)
            {
                switch (mode)
                {
                    case 0:
                        _UaClient.WriteNodes
                            (new string[]{OpcuaKukaVar[11],OpcuaKukaVar[12], OpcuaKukaVar[13],OpcuaKukaVar[14],
                            OpcuaKukaVar[15],OpcuaKukaVar[16], OpcuaKukaVar[17],OpcuaKukaVar[18] ,OpcuaKukaVar[21],
                                OpcuaKukaVar[19],OpcuaKukaVar[20],OpcuaKukaVar[22],OpcuaKukaVar[24],OpcuaKukaVar[25],OpcuaKukaVar[26],OpcuaKukaVar[27],OpcuaKukaVar[32],OpcuaKukaVar[33],OpcuaKukaVar[29]},

                                    new object[]
                                    {   _cp.Robots.Tools[0].aGipper.CloseInput,
                                        _cp.Robots.Tools[0].aGipper.OpenInput,
                                        _cp.Robots.Tools[0].aGipper.CloseOut,
                                        _cp.Robots.Tools[0].aGipper.OpenOut ,
                                        _cp.Robots.Tools[1].aGipper.CloseInput,
                                        _cp.Robots.Tools[1].aGipper.OpenInput,
                                        _cp.Robots.Tools[1].aGipper.CloseOut,
                                        _cp.Robots.Tools[1].aGipper.OpenOut,
                                        true,
                                        _cp.Robots.CaTool,
                                        _cp.Robots.RobotBas,
                                        1,
                                         _cp.Robots.Tools[0].CaToolNum,
                                         _cp.Robots.Tools[1].CaToolNum,
                                         _cp.Robots.Tools[2].CaToolNum,
                                         _cp.Robots.Tools[3].CaToolNum,
                                         _cp.Robots .CircDiameter,
                                         double.Parse( _cp.Robots.AdjDownHeight),
                                         false//不抓料
                                    }
                           ); ;
                        Thread.Sleep(100);
                        if (_cp.Robots.Tools.Count == 1)
                        {
                            _UaClient.WriteNodes(new string[] { OpcuaKukaVar[24] },

                        new object[] { _cp.Robots.Tools[_RobotsToolIndex].CaToolNum });
                        }
                        if (_cp.Robots.Tools.Count == 2)
                        {
                            _UaClient.WriteNodes(new string[] { OpcuaKukaVar[25] },

                        new object[] { _cp.Robots.Tools[1].CaToolNum });
                        }

                        break;
                    case 1:
                        _UaClient.WriteNodes
                           (new string[] { OpcuaKukaVar[10], OpcuaKukaVar[19], OpcuaKukaVar[20], OpcuaKukaVar[22], OpcuaKukaVar[0], OpcuaKukaVar[28], OpcuaKukaVar[30] },

                                   new object[]
                                   {
                                       (int)kukamode.CalibrationTool,

                                        _cp.Robots.CaTool,
                                        _cp.Robots.RobotBas ,
                                        gippernum,
                                        true,
                                        _cp.Robots.ProSpeed,
                                        toolnum

                                   }
                          );

                        break;
                    case 2:
                        _UaClient.WriteNodes(new string[] { OpcuaKukaVar[3], OpcuaKukaVar[4], OpcuaKukaVar[5], OpcuaKukaVar[6], OpcuaKukaVar[7], OpcuaKukaVar[8] },
                                                                      new object[] { tooltemp.ToolX, tooltemp.ToolY, tooltemp.ToolZ, tooltemp.ToolA, tooltemp.ToolB, tooltemp.ToolC });
                        Thread.Sleep(500);
                        _UaClient.WriteNodes(new string[] { OpcuaKukaVar[3], OpcuaKukaVar[4], OpcuaKukaVar[5], OpcuaKukaVar[6], OpcuaKukaVar[7], OpcuaKukaVar[8], OpcuaKukaVar[2] },
                                                                      new object[] { tooltemp.ToolX, tooltemp.ToolY, tooltemp.ToolZ, tooltemp.ToolA, tooltemp.ToolB, tooltemp.ToolC, true });

                        Thread.Sleep(200);
                        break;
                    case 3:

                        _UaClient.WriteNodes(new string[] { OpcuaKukaVar[2] },

                         new object[] { true });
                        Thread.Sleep(200);
                        break;
                    case 4:
                        _UaClient.WriteNodes(new string[] { OpcuaKukaVar[23], OpcuaKukaVar[3], OpcuaKukaVar[4], OpcuaKukaVar[5], OpcuaKukaVar[6], OpcuaKukaVar[7], OpcuaKukaVar[8], OpcuaKukaVar[2] },

                        new object[] { ResetNum, tooltemp.ToolX, tooltemp.ToolY, tooltemp.ToolZ, tooltemp.ToolA, tooltemp.ToolB, tooltemp.ToolC, true });
                        Thread.Sleep(200);
                        break;
                    case 5:
                        _UaClient.WriteNodes(new string[]{ OpcuaKukaVar[10], OpcuaKukaVar[3], OpcuaKukaVar[4], OpcuaKukaVar[5], OpcuaKukaVar[6], OpcuaKukaVar[7], OpcuaKukaVar[8], OpcuaKukaVar[19]
                                                               },
                                 new object[] { 10, tooltemp.ToolX, tooltemp.ToolY, tooltemp.ToolZ, tooltemp.ToolA, tooltemp.ToolB, tooltemp.ToolC, toolnum });
                        Thread.Sleep(200);
                        break;
                    case 6:
                        _UaClient.WriteNodes(new string[]{OpcuaKukaVar[0], OpcuaKukaVar[2]
                                                               },
                              new object[] { true, true });
                        Thread.Sleep(200);
                        break;
                    case 7:
                        _UaClient.WriteNodes(new string[]{OpcuaKukaVar[2],OpcuaKukaVar[31]
                                                               },
                            new object[] { true, continueAdj });
                        Thread.Sleep(200);
                        break;

                }
            }


        }

        private void CreateExl(string Filename)
        {
            //创建一个workbook实例
            // Workbook wb = new Workbook();

            //清除默认的工作表
            _workbook.Worksheets.Clear();

            //添加一个工作表并指定表名
            Worksheet sheet = _workbook.Worksheets.Add("data");

            //创建一个DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime");//0
            dt.Columns.Add("ABC_UP1");//1
            dt.Columns.Add("ABC_UP2");//2
            dt.Columns.Add("ABC_UP3");//3
            dt.Columns.Add("ABC_UP4");//4
            dt.Columns.Add("ABC_UP5");//5
            dt.Columns.Add("ABC_DO1");//6
            dt.Columns.Add("ABC_DO2");//7
            dt.Columns.Add("ABC_DO3");//8
            dt.Columns.Add("ABC_DO4");//9

            dt.Columns.Add("ABC_DO5");//10
            dt.Columns.Add("XYZ1");//11
            dt.Columns.Add("XYZ2");//12
            dt.Columns.Add("XYZ3");//13
            dt.Columns.Add("XYZ4");//14
            dt.Columns.Add("XYZ5");//15
            dt.Columns.Add("ABCPre_Y");//16
            dt.Columns.Add("ABCPre_Z");//17
            dt.Columns.Add("XYZPre_x");//18
            dt.Columns.Add("XYZPre_Y");//19

            dt.Columns.Add("Z");//20
            dt.Columns.Add("After_tool_A");//21
            dt.Columns.Add("After_tool_B");//22
            dt.Columns.Add("After_tool_C");//23
            dt.Columns.Add("After_tool_X");//24
            dt.Columns.Add("After_tool_Y");//25
            dt.Columns.Add("After_tool_Z");//26

            dt.Columns.Add("Befor_tool_A");//27
            dt.Columns.Add("Befor_tool_B");//28
            dt.Columns.Add("Befor_tool_C");//29
            dt.Columns.Add("Befor_tool_X");//30
            dt.Columns.Add("Befor_tool_Y");//31
            dt.Columns.Add("Befor_tool_Z");//32



            dt.Columns.Add("Product_tool_A");//33
            dt.Columns.Add("Product_tool_B");//34
            dt.Columns.Add("Product_tool_C");//35
            dt.Columns.Add("Product_tool_X");//36
            dt.Columns.Add("Product_tool_Y");//37
            dt.Columns.Add("Product_tool_Z");//38

            dt.Columns.Add("Robot_CircPoint1");//(39)
            dt.Columns.Add("Robot_CircPoint2");//(40)
            dt.Columns.Add("Robot_CircPoint3");//(41)
            dt.Columns.Add("Check(Y/N)");//(42)
            dt.Columns.Add("AutoTest(Y/N)");//(43)
            dt.Columns.Add("Check/Start");//(44)
            dt.Columns.Add("GipperNum");//(45)
            dt.Columns.Add("Height");//(46)
            dt.Columns.Add("toolNum");//(47)

            //将DataTable数据写入工作表
            sheet.InsertDataTable(dt, true, 1, 1, true);



            //保存为.xlsx文件
            _workbook.SaveToFile(Filename);


        }

        private void MainEnd()
        {
            //_socket.Close();
            Thread.Sleep(100);
            ConfigPara para = ReadConfig(_pathRoot);
            _testdata[47] = $"{para.Robots.Tools[_RobotsToolIndex].CaToolNum}";
            _testdata[46] = $"{para.Robots.Tools[_RobotsToolIndex].RobotConfigPara.PointHeight}";
            _testdata[45] = _RunIndex;
            _testdata[41] = $"{_KukaPoint[4]},{_KukaPoint[5]}";
            _testdata[40] = $"{_KukaPoint[2]},{_KukaPoint[3]}";
            _testdata[39] = $"{_KukaPoint[0]},{_KukaPoint[1]}";

            TestData(_testdata);

            _SystemRun = false;
            _disable = false;
            xypoint.Clear();
            //ok light
            _LightNG = false;
            _LightOK = true;
            //_UaClient.Disconnect();
            _Stopwatch.Stop();
            TimeSpan ts = _Stopwatch.Elapsed;
            // 格式化并输出时间间隔
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            Invoke(new Action(() =>
            {
                TimeAll.Text = elapsedTime;
                StaticCommonVar.SysStaus = false;
            }));
            InsetMsg($"校准完成");
            _ControlUpdate = true;
            _RunIndex++;
            
        }

        private void WriteExl(object[] data, string Filename)
        {
            double temp = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //double temp = 0;
            _workbook.LoadFromFile(Filename);
            _sheet = _workbook.Worksheets[0];
            //创建一个DataTable
            DataTable dt = new DataTable();
            //data.Add(temp);
            data[0] = _ID; 

            //找到空的格子
            for (int i = 2; i < 10000; i++)
            {
                string A = _sheet.Range["A" + i.ToString()].DisplayedText;
                if (A == "")
                {
                    _cloum = i;
                    break;
                }
            }
            // _sheet.
            //将DataTable数据写入工作表
            _sheet.InsertArray<object>(data, _cloum, 1, false);
            //_sheet.InsertDataTable(dt, true, _cloum, 1, true);

            //设置列宽、行高为自适应（应用于指定数据范围）
            _sheet.AllocatedRange.AutoFitColumns();
            _sheet.AllocatedRange.AutoFitRows();

            //保存为.xlsx文件
            _workbook.SaveToFile(Filename);
        }

        /// <summary>
        /// excel的写入
        /// </summary>
        /// <param name="data"></param>
        private void TestData(object[] data)
        {
            try
            {
                string strPath = "";
                string strFileName = "";
                string robothis = _RobotExcelAdress;//_cp.Robots.RobotExcelAdress;
                string robotinit = _RobotExcelAdress;// _cp.Robots.RobotExcelAdress;
                //生成每日文件
                if (_AutoTest == true)
                {
                    strPath = _pathRoot + $@"DataWrite\{robotinit}\";//+DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                    strFileName = _pathRoot + $@"DataWrite\{robotinit}\" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

                }
                else
                {
                    strPath = _pathRoot + $@"TestData\{robothis}\";//+DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
                    strFileName = _pathRoot + $@"TestData\{robothis}\" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

                }
                //如果纯在就创建文件夹或者文件

                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                if (!File.Exists(strFileName))
                {
                    CreateExl(strFileName);

                }
                WriteExl(data, strFileName);


            }
            catch (Exception ex)
            {

                LogHelper.WriteFile("数据记录异常" + ex.ToString());
            }
        }

        /// <summary>
        /// 数据获取
        /// </summary>
        private void GetData(string SendData)
        {

            switch (_DataStep)
            {
                case 0:
                    _GetComplete = false;
                    _sendData = "";
                    _DataStep = 1;
                    break;
                case 1:
                    _sendData = SendData;

                    if (_sendData == "GetDataGatData")
                    {
                        _sendData = "GetData";
                    }
                    _sendCon = true;
                    _DataStep = 2;
                    break;
                case 2:
                    if (!_sendCon)
                    {
                        if (_sendData == "Start" || _sendData == "End")
                        {
                            _DataStep = 10;
                        }
                        else
                        {
                            _reveCon = true;
                            _DataStep = 3;
                        }
                    }
                    break;
                case 3:
                    if (!_reveCon)
                    {
                        if (_sendData == "GetStatus")
                        {
                            _DataStep = 5;
                        }
                        else
                        {
                            _DataStep = 4;
                        }



                    }
                    break;
                case 4:
                    if (_reveData != "")
                    {
                        // _strs = _reveData.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                        //if (!_strs[0].Contains("{"))
                        // {
                        //     _strs[0] = null;
                        //   //  _reveData
                        // }
                        // _reveData=string.Join("\n", _strs);
                        if (_reveData[0] != '{')
                        {
                            _reveData = _reveData.Substring(1, _reveData.Length - 1);
                        }
                        _Data = JsonConvert.DeserializeObject<Root>(_reveData);
                        _DataStep = 10;
                    }
                    break;
                case 5:
                    if (_reveData != "")
                    {
                        _PCI_int = int.Parse(_reveData);
                        _DataStep = 10;
                    }
                    break;
                case 10:
                    _GetComplete = true;

                    break;
            }
        }

        /// <summary>
        /// 数据通讯
        /// </summary>
        private void DataSocket()
        {
            try
            {
                if (_socket.Connected&&!StaticCommonVar.RobotChangeOver)
                {
                    if (_sendCon)
                    {
                        if (_sendData != null)
                        {
                            byte[] msg = Encoding.ASCII.GetBytes(_sendData);
                            int bytesSent = _socket.Send(msg);
                            _sendCon = false;

                        }


                    }
                    if (_reveCon)
                    {
                        Socketdata = "";
                        byte[] buffer = new byte[1024];
                        int bytesRec = _socket.Receive(buffer);
                        Socketdata = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                        _reveData = Socketdata;
                        // _reveData = JsonConvert.DeserializeObject<Root>(data);
                        _reveCon = false;
                    }
                }
                if (!_socket.Connected || _SocketReset)
                {
                    //_socket.Disconnect(false);
                    _socket.Close();
                    Thread.Sleep(1000);
                    // 创建一个Socket并连接到服务器
                    _ipAddress = IPAddress.Parse(_cp.DataIP);
                    _remoteEP = new IPEndPoint(_ipAddress, _cp.DataPort);
                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _socket.Connect(_remoteEP);
                    _SocketReset = false;
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteFile("DataSocket" + ex.ToString());
            }



        }

        private void CheckCA()
        {

            if (_OPCUA_POSX)
            {
                if (!_X)
                {
                    _CACheck_x = false;
                }
            }
            if (_OPCUA_POSX)
            {
                if (!_Y || !_X)
                {
                    _CACheck_x = false;
                }
            }
            if (_OPCUA_POSX)
            {
                if (!_Y)
                {
                    _CACheck_x = false;
                }
            }
        }
        private void General()
        {
            while (_disableMain)
            {
                try
                {
                    //Thread.Sleep(50);

                   // m.WaitOne();
                    if ((_PLCclient == null || _plcConnected) && _UaClient.Connected &&/* _KukaProName == "TCPMAIN_TOOLZADOWN" &&*/ _KukaSpeed == "10" && _socket.Connected && _KukaProRun)
                    {
                        _SysReady = true;

                    }
                    else
                    {
                        _SysReady = false;
                    }
                    if(StaticCommonVar.RobotChangeOver)
                    {
                        _socket.Close();
                        // _socket.Disconnect(false);
                        _UaClient.DisconnectAsync();
                        m.Reset();
                        //_ReadKukapro.w
                        
                        _cp = ReadConfig(_pathRoot);
                        Thread.Sleep(4000);
                        NoteService = $"opc.tcp://{_cp.Robots.RobotIP}:4840";
                       
                        _UaClient.ServerUrl = NoteService;
                        _UaClient.ConnectAsync();
                       
                        _SocketReset = true;
                        m.Set();
                        Thread.Sleep(3000);
                        StaticCommonVar.RobotChangeOver=false;

                    }
                    Communtion();
                    UpdateControl();
                    PciShow();
                    //SysStauLab.ForeColor = _SystemRun ? Color.Green : Color.Red;
                    StaticCommonVar.Opcua_Status = _UaClient.Connected ;
                    //ADSConLab.ForeColor = _plcConnected ? Color.Green : Color.Red;
                    StaticCommonVar.X_Status = _PCI_X  ;
                    StaticCommonVar.Y_Status = _PCI_Y;
                    StaticCommonVar.Data_Status = _socket.Connected;
                    StaticCommonVar.Ready_Status = _SysReady;
                    Thread.Sleep(10);


                }
                catch (Exception ex)
                {
                    LogHelper.WriteFile("General" + ex.ToString());

                }
            }


        }

        private void PciShow()
        {

            if (!_SystemRun)
            {
                GetData("GetStatus");
                if (_GetComplete)
                {
                    _PCI_X = _PCI_int == 1 || _PCI_int == 3 ? true : false;
                    _PCI_Y = _PCI_int == 2 || _PCI_int == 3 ? true : false;
                    _DataStep = 0;
                }


            }
            else
            {
                _PCI_X = false;
                _PCI_Y = false;

            }



        }
        private void InsetMsg(string msg)
        {
            _ltMsg.Insert(0, GetSysTime() + msg);

            if (_ltMsg.Count > 50)
            {
                _ltMsg = _ltMsg.GetRange(0, 50);
            }
        }
        private string GetSysTime()
        {
            return System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "-";
        }
        private bool DataCheck()
        {
            //bool _result = true;

            //if (_cp.ABC_UP1_DL > _Data.abc_up1 && _Data.abc_up1 > _cp.ABC_UP1_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.ABC_UP2_DL > _Data.abc_up2 && _Data.abc_up2 > _cp.ABC_UP2_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.ABC_UP3_DL > _Data.abc_up3 && _Data.abc_up3 > _cp.ABC_UP3_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.ABC_UP4_DL > _Data.abc_up4 && _Data.abc_up4 > _cp.ABC_UP4_UL)
            //{
            //    _result = false;
            //}

            //if (_cp.ABC_DO1_DL > _Data.abc_do1 && _Data.abc_do1 > _cp.ABC_DO1_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.ABC_DO2_DL > _Data.abc_do2 && _Data.abc_do2 > _cp.ABC_DO2_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.ABC_DO3_DL > _Data.abc_do3 && _Data.abc_do3 > _cp.ABC_DO3_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.ABC_DO4_DL > _Data.abc_do4 && _Data.abc_do4 > _cp.ABC_DO4_UL)
            //{
            //    _result = false;
            //}

            //if (_cp.XYZ_DO1_DL > _Data.xyz_do1 && _Data.xyz_do1 > _cp.XYZ_DO1_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.XYZ_DO2_DL > _Data.xyz_do2 && _Data.xyz_do2 > _cp.XYZ_DO2_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.XYZ_DO3_DL > _Data.xyz_do3 && _Data.xyz_do3 > _cp.XYZ_DO3_UL)
            //{
            //    _result = false;
            //}
            //if (_cp.XYZ_DO4_DL > _Data.xyz_do4 && _Data.xyz_do4 > _cp.XYZ_DO4_UL)
            //{
            //    _result = false;
            //}

            return false;
        }
        private void MainStepGeneral()
        {
            try
            {
                //XndYList.Clear();
                _ltMsg.Clear();
                _ID= Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmss")).ToString();
                _SystemRun = true;
                _disable = true;
                #region opcua采集线程
                //上圆采集数据线程 采集x的信号点
                //Task.Run(() =>
                //{
                //    try
                //    {
                //        while (_disable)
                //        {
                //            Thread.Sleep(1);
                //            // if (FourBytes[0].ToString().Contains("1") && !var[0]) {
                //            if ((bool)_PLCclient.ReadAny(_PLCclient.CreateVariableHandle("GVL.X"), typeof(System.Boolean)))
                //            {
                //                //x信号
                //                //var[0] = true;
                //                //DateTime now = DateTime.Now;
                //                //double milliseconds = now.TimeOfDay.TotalMilliseconds;
                //                DateTime milliseconds = DateTime.UtcNow;
                //                //TimeSpan ts = d2 - d1;
                //                //Console.WriteLine(ts.TotalMilliseconds);
                //                if (UpOk == true && DownOk == false)
                //                {
                //                    PointAndTime u1 = new PointAndTime();
                //                    u1.TimeMillos = milliseconds;
                //                    u1.IsWhat = "X";
                //                    upointList.Add(u1);
                //                }
                //                if (UpOk == false && DownOk == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "X";
                //                    dpointList.Add(d);
                //                }
                //                if (ThreeOK == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "X";
                //                    xypoint.Add(d);
                //                }
                //                if (Obliquecircle == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "X";
                //                    ObliquecircleList.Add(d);

                //                }

                //                if (Obliquecircle2 == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "X";
                //                    ObliquecircleList2.Add(d);

                //                }


                //                #region 写数据库
                //                ////判断是上圆还是下圆
                //                //string types = type;
                //                ////上圆就写入上圆的集合 下圆就写入下圆的集合
                //                //if (type == "up")
                //                //{
                //                //    upCr.Add(new Point3D(Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4])));
                //                //}
                //                //if (type == "down")
                //                //{
                //                //    DownCr.Add(new Point3D(Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4])));
                //                //}
                //                //try
                //                //{
                //                //    //写入数据库
                //                //    InsertData(data, types);

                //                //}
                //                //catch (Exception ex)
                //                //{

                //                //    throw;
                //                //}
                //                #endregion

                //            }
                //            else
                //            {
                //                // var[0] = false;

                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        LogHelper.WriteFile("X信号采集线程异常" + ex.ToString());
                //    }


                //});
                ////下圆采集数据线程 采集所有y的信号点
                //Task.Run(() =>
                //{
                //    try
                //    {
                //        while (_disable)
                //        {
                //            Thread.Sleep(1);
                //            //if (FourBytes[0].ToString().Contains("2") && !var[1])
                //            if ((bool)_PLCclient.ReadAny(_PLCclient.CreateVariableHandle("GVL.Y"), typeof(System.Boolean)))
                //            {
                //                DateTime milliseconds = DateTime.UtcNow;
                //                if (UpOk == true && DownOk == false)
                //                {
                //                    PointAndTime u = new PointAndTime();
                //                    u.TimeMillos = milliseconds;
                //                    u.IsWhat = "Y";
                //                    upointList.Add(u);
                //                }
                //                if (UpOk == false && DownOk == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "Y";
                //                    dpointList.Add(d);
                //                }
                //                if (ThreeOK == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "Y";
                //                    xypoint.Add(d);
                //                }
                //                if (Obliquecircle == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "Y";
                //                    ObliquecircleList.Add(d);

                //                }
                //                if (Obliquecircle2 == true)
                //                {
                //                    PointAndTime d = new PointAndTime();
                //                    d.TimeMillos = milliseconds;
                //                    d.IsWhat = "Y";
                //                    ObliquecircleList2.Add(d);

                //                }
                //                #region 写数据库
                //                ////判断是上圆还是下圆
                //                //string types =type;
                //                //////上圆就写入上圆的集合 下圆就写入下圆的集合
                //                //if (type == "up")
                //                //{
                //                //    upCr.Add(new Point3D(Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4])));
                //                //}
                //                //if (type == "down")
                //                //{
                //                //    DownCr.Add(new Point3D(Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4])));
                //                //}
                //                ////写入数据库
                //                //InsertData(data, types);
                //                #endregion 写数据库结束
                //            }
                //            else
                //            {
                //                // var[1] = false;
                //            }
                //        }

                //    }
                //    catch (Exception ex)
                //    {

                //        LogHelper.WriteFile("Y信号采集线程异常" + ex.ToString());
                //    }

                //});
                ////校准z轴的时候采集点位 采集x和y同时存在的点位
                //Task.Run(() =>
                //{
                //    try
                //    {
                //        while (_disable)
                //        {
                //            Thread.Sleep(1);
                //            //if (FourBytes[0].ToString().Contains("2") && !var[1])
                //            if ((bool)_PLCclient.ReadAny(_PLCclient.CreateVariableHandle("GVL.Y"), typeof(System.Boolean)) && (bool)_PLCclient.ReadAny(_PLCclient.CreateVariableHandle("GVL.X"), typeof(System.Boolean)))
                //            {
                //                //if (DownOk == false && ThreeOK == false)
                //                //{
                //                DateTime milliseconds = DateTime.UtcNow;
                //                PointAndTime xy = new PointAndTime();
                //                xy.TimeMillos = milliseconds;
                //                xy.IsWhat = "XY";
                //                XndYList.Add(xy);
                //                // }
                //                #region 写数据库
                //                ////判断是上圆还是下圆
                //                //string types =type;
                //                //////上圆就写入上圆的集合 下圆就写入下圆的集合
                //                //if (type == "up")
                //                //{
                //                //    upCr.Add(new Point3D(Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4])));
                //                //}
                //                //if (type == "down")
                //                //{
                //                //    DownCr.Add(new Point3D(Convert.ToDouble(data[2]), Convert.ToDouble(data[3]), Convert.ToDouble(data[4])));
                //                //}
                //                ////写入数据库
                //                //InsertData(data, types);
                //                #endregion 写数据库结束
                //            }
                //            else
                //            {
                //                // var[1] = false;
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    { LogHelper.WriteFile("Z信号采集线程异常" + ex.ToString()); }


                //});
                ////精度线程
                //Task.Run(() =>
                //{
                //    try
                //    {
                //        while (_disable)
                //        {
                //            //Thread.Sleep(1);
                //            //if (_plcConnected)
                //            //{
                //            //   // hangle1 = client.CreateVariableHandle("GVL.X");
                //            //    //hangle2 = client.CreateVariableHandle("GVL.Y");
                //            //    _X = (bool)_PLCclient.ReadAny(_PLCclient.CreateVariableHandle("GVL.X"), typeof(System.Boolean));
                //            //    _Y = (bool)_PLCclient.ReadAny(_PLCclient.CreateVariableHandle("GVL.Y"), typeof(System.Boolean));
                //            //}
                //            if (dataValues[5].Value != null)
                //            {
                //                bool[] EndRes = (bool[])dataValues[5].Value;
                //                if (EndRes[0] == true) { UpOk = true; DownOk = false; }
                //                if (EndRes[1] == true) { UpOk = false; DownOk = true; }
                //                if (EndRes[0] == false && EndRes[1] == false) { UpOk = false; DownOk = false; }
                //                if (EndRes[0] == false && EndRes[1] == false && EndRes[2] == true) { UpOk = false; DownOk = false; ThreeOK = true; }
                //                if (EndRes[0] == false && EndRes[1] == false && EndRes[2] == false) { UpOk = false; DownOk = false; ThreeOK = false; }
                //                if (EndRes[0] == true && EndRes[1] == false && EndRes[2] == false) { UpOk = true; DownOk = false; ThreeOK = false; }
                //                //if (EndRes[3] == true) { Obliquecircle = true; }
                //                //if (EndRes[3] == false) { Obliquecircle = false; }
                //                //if (EndRes[4] == false) { Obliquecircle2 = false; }
                //                //if (EndRes[4] == true) { Obliquecircle2 = true; }
                //                //这里的value为{false|false}，所以这样解析不对
                //                // data.Add( dataValues[5].Value.ToString());
                //            }

                //        }
                //    }
                //    catch (Exception ex)
                //    { LogHelper.WriteFile("xy信号采集线程异常" + ex.ToString()); }



                //});
                #endregion
                _Stopwatch.Start();

            }
            catch (Exception ex)
            {

                LogHelper.WriteFile("MainStepGeneral异常" + ex.ToString());
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {

            StartBtn.Enabled = false;
          
            ConfigRunBtn.Enabled = false;
           
            //RtAdjToolGroupBox.Visible = false;
            //RotRealToolGroupBox.Visible = false;
            //MainGrpup.Visible = false;
            //RtProToolGroupBox.Visible = false;
            //CheckLightLab.ForeColor = Color.Gray;
            #region 数据采集软件
            string exefile = _pathRoot + @"DataAPP\pci1761_socket_console.exe";
            //string exefile = _pathRoot + @"DataAPP\tc3Ads_socket_console.exe";
            //判断是否在运行
            Process[] processes = Process.GetProcessesByName("pci1761_socket_console");
            //Process[] processes = Process.GetProcessesByName("tc3Ads_socket_console");
            if (processes.Length == 0)
            {
                InsetMsg("打开数据采集软件...");

                _ControlUpdate = true;
                if (File.Exists(exefile))
                {
                    Process process = new Process();   // params 为 string 类型的参数，多个参数以空格分隔，如果某个参数为空，可以传入””
                    ProcessStartInfo startInfo = new ProcessStartInfo(exefile);
                    process.StartInfo = startInfo;
                    process.Start();
                }
                Thread.Sleep(200);
            }
            else
            {
                InsetMsg("数据采集软件检测已经打开");

                _ControlUpdate = true;
                // using
            }
            #endregion

            #region Socket配置 
            _ipAddress = IPAddress.Parse(_cp.DataIP);
            _remoteEP = new IPEndPoint(_ipAddress, _cp.DataPort);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            #endregion

            #region 界面初始化
            //for (int i = 0; i < _cp.Robots.Count; i++)
            //{
            //    //comboBox3.Items.Add(_cp.Robots[i].RobotIP);
            //}
           
            //OPCUAAdresBox.Text = $"opc.tcp://{_cp.Robots.RobotIP}:4840";

            #endregion

            #region opcua 配置
            NoteService =$"opc.tcp://{_cp.Robots.RobotIP}:4840";
            if (application == null)
            {
                application = new ApplicationInstance();
                application.ApplicationName = "KukawinOPCUAClient";
                application.ApplicationType = ApplicationType.Client;
                application.ConfigSectionName = "KukawinOPCUAClient";
            }



            _UaClient = new UaClient(application);
            //_UaClient.Disconnect();
            _UaClient.UseSecurity = true;
            _UaClient.UserIdentity = new UserIdentity("OpcUaOperator", "kuka");
            _UaClient.ServerUrl = NoteService;

            //因为连接机器人的时候需要sleep 1000

            //try catch 日志纪录
            _UaClient.ConnectAsync();
            ////ads connect
            ///
            #endregion

            #region ADS配置
            if (exefile.Contains("tc3Ads_socket_console"))
            {


                _PLCclient = new TcAdsClient();
                _PLCclient.Connect(AmsNetId.LocalHost, 851);
                //AdsBox.Text = AmsNetId.LocalHost.ToString();
                //ads var
                _Hangle_X = "X";
                _Hangle_Y = "Y";
                _Hangle_BtnStart = "start";
                _Hangle_LightOK = "light_ok";
                _Hangle_LightNG = "light_ng";
                _Hangle_BtnCheck = "Check";
            }
            else
            {
                //label3.Visible = false;
                //label4.Visible = false;
                //label6.Visible = false;
                //AdsBox.Visible = false;
                //ADSConLab.Visible = false;
            }
            #endregion

            #region opcua var
            //操作模式
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.System.$MODE_OP"));
            //程序状态
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.System.$PRO_STATE"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.System.R1.$POS_ACT"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.System.R1.$TOOL"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.System.$IN[8192]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_CIRC1[10]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.System.$OV_PRO1"));//6
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.System.$PRO_NAME[24]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_Complete"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_circ1_point1[2]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_circ1_point2[2]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_circ1_point3[2]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_circ2_point1[2]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_circ2_point2[2]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_circ2_point3[2]"));
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_POSX"));//15
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_POSXY"));//16
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_POSY"));//17
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_STEP"));//18
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_circ_Z[2]"));//19
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.TOOL_DATA[16]"));//20
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_GipperErr"));//21
            ltNode.Add(new NodeId("ns=5;s=MotionDeviceSystem.MotionDevices.MotionDevice_1.Identification.SerialNumber")); //22

            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_START");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.System.$OV_PRO1");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_WRITE");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_X");//3
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_Y");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_Z");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_A");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_B");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_C");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_Complete");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_MODE");//10
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_closeinput1");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_openinput1");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_closeout1");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_openout1");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_closeinput2");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_openinput2");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_closeout2");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_openout2");
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_Tool");//19
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_Bas");//20
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_systemRun");//21
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_GipperNum");//22
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ResetNum");//23
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ToolNum1");//24
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ToolNum2");//25
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ToolNum3");//26
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ToolNum4");//27
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ProSpeed");//28
            //直接抓取现场暂时没有
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.CaGrap");//29
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ToolNum");//30
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_ContinueAdj");//31
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_CircDiameter");//32
            OpcuaKukaVar.Add("ns=5;s=MotionDeviceSystem.ProcessData.R1.System.$config.OPCUA_AdjDownHeight");//33

            //OpcuaKukaVar.Add("");
            #endregion

        }
        /// <summary>
        /// 通讯
        /// </summary>
        public void Communtion()
        {
            try
            {
                if (_PLCclient == null)
                    return;
                if (_PLCclient.IsConnected)
                {
                    StateInfo si = new StateInfo();

                    AdsErrorCode aec = _PLCclient.TryReadState(out si);

                    if (aec == AdsErrorCode.NoError)                            //代表目标服务可用
                    {
                        _plcConnected = true;

                        _BtnStart = (bool)ReadToPlc(_PLCclient, _Hangle_BtnStart, typeof(System.Boolean), _cp.PLCStruct);
                        _BtnCheck = (bool)ReadToPlc(_PLCclient, _Hangle_BtnCheck, typeof(System.Boolean), _cp.PLCStruct);
                        _X = (bool)ReadToPlc(_PLCclient, _Hangle_X, typeof(System.Boolean), _cp.PLCStruct);
                        _Y = (bool)ReadToPlc(_PLCclient, _Hangle_Y, typeof(System.Boolean), _cp.PLCStruct);
                        WirteToPlc(_PLCclient, _Hangle_LightOK, _cp.PLCStruct, _LightOK);
                        WirteToPlc(_PLCclient, _Hangle_LightNG, _cp.PLCStruct, _LightNG);

                    }
                    else
                    {
                        _plcConnected = false;
                    }
                }
                else
                {
                    _plcConnected = false;
                    LogHelper.WriteFile("Pc与Plc断开");
                    _PLCclient.Connect(AmsNetId.LocalHost, 851);
                    Thread.Sleep(1000);
                }
                //if (_UaClient.Connected == false)
                //{
                //    _UaClient.ConnectAsync();
                //    Thread.Sleep(1000);
                //}

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Ads"))
                {
                    _plcConnected = false;
                    _PLCclient.Disconnect();
                    Thread.Sleep(100);
                }
                LogHelper.WriteFile(ex.ToString());

            }

        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private ConfigPara ReadConfig(string strPath)
        {
            string path = strPath + @"\Config\";
            string file = path + "ConfigPara.xml";

            if (!File.Exists(file))
            {
                return null;
            }

            FileStream fs = new FileStream(file, FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(ConfigPara));
            ConfigPara cp = xml.Deserialize(fs) as ConfigPara;
            fs.Close();
            _cp = cp;
            _RobotName = _cp.Robots.BU + "_" + _cp.Robots.LineName + "_" + _cp.Robots.WorkName + "_" + _cp.Robots.RobotName+"_"+_cp.Robots.RobotSeriorNo;
            _RobotExcelAdress = _cp.Robots.BU + "\\" + _cp.Robots.LineName + "\\" + _cp.Robots.WorkName + "\\" + _cp.Robots.RobotName + "\\" + _cp.Robots.RobotSeriorNo; ;
            StaticCommonVar.AdminPwd = _cp.AdminPwd;
            StaticCommonVar.RobotName = _RobotName;
            return cp;
        }
        /// <summary>
        /// 写入plc
        /// </summary>
        /// <param name="_plc">PLCADS实体</param>
        /// <param name="VarName">plc内部变量名</param>
        /// <param name="PlcVarName">变量名的上级</param>
        /// <param name="value">写入值</param>
        public void WirteToPlc(TcAdsClient _plc, string VarName, string PlcVarName, bool value)
        {
            try
            {
                if (_plcConnected)
                {
                    //注意：c#的int等于twincat的dint
                    //      string类型的必须用专门的string写法
                    int hangle;
                    hangle = _plc.CreateVariableHandle(PlcVarName + "." + VarName);
                    _plc.WriteAny(hangle, value);
                    _plc.DeleteVariableHandle(hangle);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteFile("写入PLC错误：" + ex.ToString() + "\n");

            }

        }
        /// <summary>
        /// 读取plc变量
        /// </summary>
        /// <param name="_plc">PLCADS实体</param>
        /// <param name="VarName">plc内部变量名</param>
        /// <param name="type">读取类型</param>
        /// <param name="PlcVarName">变量名的上级</param>
        /// <returns></returns>
        public object ReadToPlc(TcAdsClient _plc, string VarName, System.Type type, string PlcVarName)
        {
            try
            {
                if (_plcConnected)
                {
                    object _result = null;
                    int hangle;
                    hangle = _plc.CreateVariableHandle(PlcVarName + "." + VarName);
                    if (type == System.Type.GetType("System.String"))
                    {
                        _result = _plc.ReadAnyString(hangle, 500, System.Text.Encoding.Default);
                    }
                    else
                    {
                        _result = _plc.ReadAny(hangle, type);
                    }

                    _plc.DeleteVariableHandle(hangle);

                    return _result;

                }
                else
                {
                    LogHelper.WriteFile("PLC断开连接：" + "\n");
                    return null;
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteFile("读取PLC错误：" + ex.ToString() + "\n");
                return null;
            }
        }
        /// <summary>
        /// 更新控件方法 刷新页面上控件的值
        /// </summary>
        public void UpdateControl()
        {
            try
            {
                Invoke(new Action(() =>
                {
                    if (_ControlUpdate)
                    {
                        infolistbox.BeginUpdate();                   //避免闪烁
                        infolistbox.DataSource = null;
                        infolistbox.DataSource = _ltMsg;
                        infolistbox.EndUpdate();
                        _ControlUpdate = false;
                    }
                    RobotTypeLab.Text = _cp.Robots.RobotType;
                    LineNameLab.Text = _cp.Robots.LineName;
                    StationLab.Text = _cp.Robots.WorkName;
                    // AutoExitLog();
                    RobotProNameLab.Text = _KukaProName;
                    RobotProStatuLab.Text = _KukaProStatus;
                    RobotSerialLab.Text = _RobotSerial;
                    RobotNodeLab.Text = _KukaMode;
                    RobotSpeedLab.Text = (Convert.ToInt16(_KukaSpeed) / 10).ToSqlValue();
                    //SysStauLab.ForeColor = _SystemRun ? Color.Green : Color.Red;
                    //OpcuaConLab.ForeColor = _UaClient.Connected ? Color.Green : Color.Red;
                    //ADSConLab.ForeColor = _plcConnected ? Color.Green : Color.Red;
                    //SensorXLab.ForeColor = _X || _PCI_X ? Color.Green : Color.Red;
                    //SensorYLab.ForeColor = _Y || _PCI_Y ? Color.Green : Color.Red;
                    //DataStatuLab.ForeColor = _socket.Connected ? Color.Green : Color.Red;
                    //ReadyLab.ForeColor = _SysReady ? Color.Green : Color.Red;
                    ShowComputerIPLab.Text = _cp.DataIP;
                    ShowRobotIPLab.Text = _cp.Robots.RobotIP;
                    RotAdjRealToolALab.Text = double.Parse(_KukaModeToolA.ToString()).ToString("0.000");
                    RotAdjRealToolBLab.Text = double.Parse(_KukaModeToolB.ToString()).ToString("0.000");
                    RotAdjRealToolCLab.Text = double.Parse(_KukaModeToolC.ToString()).ToString("0.000");
                    RotAdjRealToolXLab.Text = double.Parse(_KukaModeToolX.ToString()).ToString("0.000");
                    RotAdjRealToolYLab.Text = double.Parse(_KukaModeToolY.ToString()).ToString("0.000");
                    RotAdjRealToolZLab.Text = double.Parse(_KukaModeToolZ.ToString()).ToString("0.000");
                    if (StaticCommonVar.Logined)
                    {
                        StartBtn.Enabled = true;
                        ConfigRunBtn.Enabled = true;
                    }
                    else
                    {
                        StartBtn.Enabled = false;
                        ConfigRunBtn.Enabled = false;
                    }
                    int CaTool = _cp.Robots.CaTool;
                    int CaToolNum = _cp.Robots.Tools[_RobotsToolIndex].CaToolNum;
                    RtAdjToolNumLab.Text = CaTool.ToString();
                    RtProToolNumLab.Text = CaToolNum.ToString();
                    try
                    {
                        object retval;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(ms, _KukaToolDatas);
                            ms.Seek(0, SeekOrigin.Begin);
                            retval = bf.Deserialize(ms);
                            ms.Close();
                        }

                        //ConfigPara para = ConfigPara.DeepClone(_KukaToolDatas);
                        KukaToolDatasTemp = (dynamic)retval;
                        //KukaToolDatasTemp = _KukaToolDatas;
                        if (KukaToolDatasTemp[15] != null && KukaToolDatasTemp.Count >= 6)
                        {

                            RtAdjToolXLab.Text = double.Parse(KukaToolDatasTemp[CaTool - 1][0]).ToString("0.000");
                            RtAdjToolYLab.Text = double.Parse(KukaToolDatasTemp[CaTool - 1][1]).ToString("0.000");
                            RtAdjToolZLab.Text = double.Parse(KukaToolDatasTemp[CaTool - 1][2]).ToString("0.000");
                            RtAdjToolALab.Text = double.Parse(KukaToolDatasTemp[CaTool - 1][3]).ToString("0.000");
                            RtAdjToolBLab.Text = double.Parse(KukaToolDatasTemp[CaTool - 1][4]).ToString("0.000");
                            RtAdjToolCLab.Text = double.Parse(KukaToolDatasTemp[CaTool - 1][5]).ToString("0.000");
                        }
                        if (KukaToolDatasTemp[15] != null && KukaToolDatasTemp.Count >= 6)
                        {
                            RtProToolXLab.Text = double.Parse(KukaToolDatasTemp[CaToolNum - 1][0]).ToString("0.000");
                            RtProToolYLab.Text = double.Parse(KukaToolDatasTemp[CaToolNum - 1][1]).ToString("0.000");
                            RtProToolZLab.Text = double.Parse(KukaToolDatasTemp[CaToolNum - 1][2]).ToString("0.000");
                            RtProToolALab.Text = double.Parse(KukaToolDatasTemp[CaToolNum - 1][3]).ToString("0.000");
                            RtProToolBLab.Text = double.Parse(KukaToolDatasTemp[CaToolNum - 1][4]).ToString("0.000");
                            RtProToolCLab.Text = double.Parse(KukaToolDatasTemp[CaToolNum - 1][5]).ToString("0.000");
                        }
                        KukaToolDatasTemp = null;

                    }
                    catch (Exception)
                    {
                        KukaToolDatasTemp = null;
                        //throw;
                    }

                }));
            }
            catch (Exception ex)
            {
                LogHelper.WriteFile("控价刷新错误" + ex.ToString() + "\n");

            }


        }

        private void CheckBtn_Click(object sender, EventArgs e)
        {
            if (!_SysReady)
            {
                MessageBox.Show("系统未准备好");
                return;
            }
            if (StaticCommonVar.SysStaus)
            {
                MessageBox.Show("系统正在运行中");
                return;
            }
            if (MessageBox.Show("是否开始检查？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
               
           
            CheckTimeLab.Text = DateTime.Now.ToString();
            _CheckStart = true;
            _CheckSignal = true;
            _Start = false;
            _testdata[44] = "Check";
            }
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (!_SysReady)
            {
                MessageBox.Show("系统未准备好");
                return;
            }
            _testdata[44] = "Start";
            if (StaticCommonVar.SysStaus)
            {
                MessageBox.Show("系统正在运行中");
                return;
            }
            if (MessageBox.Show("是否开始矫正？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                _CheckSignal = false;
                _Start = true;
            }
           
        }

        private void ConfigRunBtn_Click(object sender, EventArgs e)
        {
            if (!_SysReady)
            {
                MessageBox.Show("系统未准备好");
                return;
            }
            if (StaticCommonVar.SysStaus)
            {
                MessageBox.Show("系统正在运行中");
                return;
            }
            //TestData(new List<double>());
            //if (button4.Text == "自动测试")
            // {
            // if (MessageBox.Show("测试时是否写入算出的校验值？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            if (MessageBox.Show("是否开始初始化配置运行？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                _AutoTestWrite = false;
                _AutoTest = true;
            }
            else
            {
                // _AutoTestWrite = false;
            }
        }

        private void ErrRsBtn_Click(object sender, EventArgs e)
        {
            if (_MainStep < 2 && !_SysReady)
            {
                MessageBox.Show("系统未达到复位条件不能复位");
                return;
            }
            if (MessageBox.Show("请确认错误后复位?", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {

                InsetMsg($"复位中");
                _ControlUpdate = true;
                _MainStep = 100;
                //button4.Text = "初始化配置运行";
                _AutoTest = false;

                // _ltMsg.Clear();
                // _SocketReset = true;

            }
        }

        public ConfigPara HistoryParaBack(out double[] point,out int RobotsIndex)
        {
            point = _KukaPoint; RobotsIndex = _RobotsIndex;
            return _cp;
        }
        public ConfigPara AnalyParaBack(out double[] point, out int RobotsIndex,out string path)
        {
            point = _KukaPoint; RobotsIndex = _RobotsIndex; path = _pathRoot;
            return _cp;
        }
    }
}
