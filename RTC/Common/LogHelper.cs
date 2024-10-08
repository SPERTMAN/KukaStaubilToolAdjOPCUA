using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    internal class LogHelper
    {

        public static StreamWriter _sw = null;

        private static object _obj = new object();

        private static List<string> _ltLog = new List<string>();


        private static bool _bAbort = false;
        public static void AddNewLogFile(string Path, int ExpireDay)
        {
            Task.Run(() =>
            {
                WriteLog(Path + @"\Log", ExpireDay);
            });
        }

        private static void WriteLog(string Path, int ExpireDay)
        {
            string date = "", datePre = "";

            DirectoryInfo dir = Directory.CreateDirectory(Path);
            while (true)
            {
                if (_bAbort)
                    break;

                Thread.Sleep(100);
                date = DateTime.Now.ToString("yyyy-MM-dd");

                if (datePre != date)
                {
                    if (_sw != null)
                    {
                        _sw.Close();
                        _sw.Dispose();
                        _sw = null;
                    }
                    string fileName = string.Format(@"{0}\{1}.log", Path, date);

                    _sw = new StreamWriter(fileName, true);

                    _sw.AutoFlush = true;



                    foreach (var f in dir.GetFiles())
                    {
                        if (DateTime.Now.Subtract(f.CreationTime) >= new TimeSpan(ExpireDay, 0, 0, 0))
                            f.Delete();
                    }
                }

                if (_sw != null)
                {
                    lock (_obj)
                    {
                        if (_ltLog.Count > 0)
                        {
                            foreach (string str in _ltLog)
                            {
                                _sw.WriteLine(str);
                            }
                            _ltLog.Clear();
                        }
                    }

                }

                datePre = date;
            }

            if (_sw != null)
            {
                _sw.Close();
                _sw.Dispose();
                _sw = null;
            }
        }

        public static void WriteFile(string log)
        {
            lock (_obj)
                _ltLog.Add(GetSysTime() + log);
        }

        public static void CloseLogFile()
        {
            _bAbort = true;
        }

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        private static string GetSysTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---";
        }
    }
}
