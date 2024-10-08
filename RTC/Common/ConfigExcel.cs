using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Spire.Xls;
using System.Windows.Forms;
using Model;
using System.Xml.Serialization;

namespace Common
{
    public class ConfigExcel
    {
        private static Workbook _workbook = new Workbook();
        private static Worksheet _sheet = null;
        //日志文件路径
        private static string _pathRoot = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 读取文件夹的名字list
        /// </summary>
        /// <param name="DateName"></param>
        /// <returns></returns>
        public static List<string> ReadExlName(string Adress)
        {
            List<string> listBoxFiles = new List<string>();
            // 检查目录是否存在，如果不存在则创建  
            if (!Directory.Exists(_pathRoot + Adress))
            {
                Directory.CreateDirectory(_pathRoot + Adress);
            }

            // 获取指定文件夹中的所有文件  
            string[] files = Directory.GetFiles(_pathRoot + Adress + @"\");
            // 将文件列表添加到ListBox中  
            foreach (string file in files)
            {
                if (!Path.GetFileName(file).StartsWith("~$")) { 
                listBoxFiles.Add(Path.GetFileName(file)); // 添加完整路径  
                }                             // 如果你只想添加文件名，可以使用 Path.GetFileName(file)  
            }
            return listBoxFiles;

        }

            /// <summary>
            /// 选择日期带出对应的数据
            /// </summary>
            /// <param name="DateName"></param>
            /// <returns></returns>
        public static DataTable ReadExl(string DateName,string Adress)
            {
            string strFileName = _pathRoot + Adress+ @"\"+ DateName;
            if (!File.Exists(strFileName))
            {
               // MessageBox.Show($"没有找到{DateName}的文件");
                return null;
                //strFileName = _pathRoot + @"TestData\" + DateName.AddDays(-1).ToString("yyyyMMdd") + ".xlsx";
            }
            _workbook.LoadFromFile(strFileName);
            _sheet = _workbook.Worksheets[0];
            DataTable dt = new DataTable();
            int index = 0;
            foreach (var row in _sheet.Rows)
            {
                if (index == 0)
                    foreach (var column in row)
                    {
                        string Name = column.Value.ToString();
                        dt.Columns.Add(Name);   //添加表头
                    }
                else
                {
                    DataRow dr = dt.NewRow();
                    int i = 0;
                    foreach (var column in row)
                    {
                        dr[i] = column.Value.ToString(); //添加内容
                        i++;
                    }
                    dt.Rows.Add(dr);
                }
                index++;
            }
            return dt;

        }


        public static void WriteConfig(ConfigPara configPara)
        {
            ConfigPara Old = new ConfigPara();

            // Old = (ConfigPara)_cp.MemberwiseClone();
           
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigPara));
            using (TextWriter writer = new StreamWriter(_pathRoot + @"\Config\ConfigPara.xml"))
            {
                xmlSerializer.Serialize(writer, configPara);
            }
            LogHelper.WriteFile($"\n成功修改配置文件信息\n");
           
        }
        public static void NewConfig(ConfigPara configPara,string name)
        {
            ConfigPara Old = new ConfigPara();

            // Old = (ConfigPara)_cp.MemberwiseClone();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigPara));
            using (TextWriter writer = new StreamWriter(_pathRoot + $"\\Config\\{name}.xml"))
            {
                xmlSerializer.Serialize(writer, configPara);
            }
           // LogHelper.WriteFile($"\n成功修改配置文件信息\n");

        }
    }
}
