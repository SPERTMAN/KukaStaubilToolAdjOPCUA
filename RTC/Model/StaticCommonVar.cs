using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RTC.Model
{
    public delegate List<double> ReadTool(int CaTool);
    public class StaticCommonVar
    {

        public static bool Logined;
        public static string AdminPwd;
        public static string RobotName;
        public static bool SysStaus;
        public static bool X_Status;
        public static bool Y_Status;
        public static bool Opcua_Status;
        public static bool Data_Status;
        public static bool Ready_Status;

        public static bool _Exit=true;
        public static string RobotConfigPath =  @"Config\";
        public static bool RobotChangeOver;
        public static string RobotSelectName;
        public static Button RobotSaveBtn;

        public static ReadTool ReadTool;
    }
}
