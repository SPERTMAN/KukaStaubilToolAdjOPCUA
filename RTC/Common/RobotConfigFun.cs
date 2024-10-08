using Common;
using Org.BouncyCastle.Ocsp;
using RTC.Model;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Model;
using System.Xml.Serialization;

namespace RTC.Common
{
    
    public class RobotConfigFun
    {
       
        public static List<string> ReadAllName(string Path)
        {
           return ConfigExcel.ReadExlName(Path);
        }

        public static void DeleteName(string Path)
        {
            string Filepath = AppDomain.CurrentDomain.BaseDirectory + StaticCommonVar.RobotConfigPath + Path + ".xml";
             File.Delete(Filepath);
        }

        public static ConfigPara GetPara(string Path)
        {
            string Filepath = AppDomain.CurrentDomain.BaseDirectory + StaticCommonVar.RobotConfigPath + Path + ".xml";
            if (!File.Exists(Filepath))
            {
                return null;
            }

            FileStream fs = new FileStream(Filepath, FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(ConfigPara));
            ConfigPara cp = xml.Deserialize(fs) as ConfigPara;
            fs.Close();
             
           
            return cp;
        }
    }
}
