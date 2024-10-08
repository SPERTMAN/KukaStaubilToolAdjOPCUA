using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinOpcuaKuka
{
    public class SqlHelper
    {

        //SqlSugarClient模式
        public static SqlSugarClient ContextMaster(string connstr)
        {
            SqlSugarClient db1 = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = connstr,
                IsAutoCloseConnection = true,

            });

            return db1;

        }
    }
}
