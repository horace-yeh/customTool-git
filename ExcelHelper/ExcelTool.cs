using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHelper
{
    public class ExcelTool
    {
        //出現錯誤: Microsoft.ACE.OLEDB.12.0' 提供者並未登錄於本機電腦上。'
        //參考: https://devmanna.blogspot.com/2017/03/sql-server-excel-microsoftaceoledb120.html
        //下載連結: https://www.microsoft.com/zh-TW/download/details.aspx?id=13255

        //Visual Studio 調用元件 32或64位元都有可能，安裝後還是錯就裝另外一個版本
        //32、64都安裝的方式 先裝一種再裝另一種 用CMD呼叫安裝 指令:AccessDatabaseEngine_X64.exe /passive
        //參考 : https://dotblogs.com.tw/dragoncancer/2016/03/31/102924

        /// <summary>
        /// 依照Excel副檔名取得對應版本連線字串
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string ByExtensionGetExcelConn(string FileName)
        {
            string xlsxConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source={0};" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';", FileName);
            string xlsConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source={0};" + "Extended Properties=Excel 5.0;Persist Security Info=False", FileName);
            string fileConn = (System.IO.Path.GetExtension(FileName.Trim())).ToLower() == ".xls" ? xlsConn : xlsxConn;
            return fileConn;
        }

        /// <summary>
        /// 從檔案讀取Excel所有Sheet名稱轉成List
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public List<string> GetExcelAllSheetByFile(string FileName)
        {
            DataTable tempDt = new DataTable();
            List<string> tempList = new List<string>();
            string FileConn = this.ByExtensionGetExcelConn(FileName);
            using (OleDbConnection oleDbConn = new OleDbConnection(FileConn))
            {
                oleDbConn.Open();
                DataTable dt = oleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                tempDt = dt;
            }

            //遍歷dt的rows得到所有的TABLE_NAME，並Add到cmb中
            foreach (DataRow dr in tempDt.Rows)
            {
                tempList.Add((String)dr["TABLE_NAME"]);
            }
            return tempList;
        }

        /// <summary>
        /// 從檔案讀取Excel轉成DataTable
        /// </summary>
        /// <param name="FileName">完整檔案路徑還有檔名副檔名</param>
        /// <param name="SheetName">Excel分頁名稱</param>
        /// <returns></returns>
        public DataTable GetExcelByFile(string FileName, string SheetName)
        {
            string FileConn = this.ByExtensionGetExcelConn(FileName);
            string QueryStr = "select * from [" + SheetName + "]";

            OleDbDataAdapter da = null;
            DataTable dt = null;

            using (OleDbConnection oleDbConn = new OleDbConnection(FileConn))
            {
                oleDbConn.Open();
                da = new OleDbDataAdapter(QueryStr, oleDbConn);
                dt = new DataTable();
                da.Fill(dt);
                oleDbConn.Close();
            }
            return dt;
        }
    }
}