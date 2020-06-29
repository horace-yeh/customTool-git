using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHelper
{
    public class NPOITool
    {
        /// <summary>
        /// 取得ExcelTemplate寫入新資料回傳
        /// 固定拷貝表頭一列，第二列拷貝樣式重複帶資料寫入
        /// </summary>
        /// <typeparam name="T">泛型使用</typeparam>
        /// <param name="workbook">excel物件</param>
        /// <param name="CellNum">Cell欄位數量</param>
        /// <param name="ListData">要寫入的資料(泛型)</param>
        /// <param name="PropertyArr">輸入泛型資料屬性，需要依照Cell順序放入</param>
        /// <param name="SheetPage">要拷貝及寫入的Sheet</param>
        /// <returns></returns>
        public XSSFWorkbook ExcelByTemplate<T>(XSSFWorkbook workbook, int CellNum, List<T> ListData, string[] PropertyArr, int SheetPage = 0)
        {
            XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(SheetPage);
            //輸出起始列，拷貝Style列
            int copyFormatRow = 1;
            //設定各cell的style
            Dictionary<int, ICellStyle> cellStyleDictionary = new Dictionary<int, ICellStyle>();
            for (int i = 0; i < CellNum; i++)
            {
                cellStyleDictionary.Add(i, sheet.GetRow(copyFormatRow).GetCell(i).CellStyle);
            }
            //資料寫入 Row
            for (var i = 0; i < ListData.Count; i++)
            {
                IRow row = sheet.CreateRow(copyFormatRow + i);
                //處理Cell
                for (var j = 0; j < CellNum; j++)
                {
                    //填值
                    SetCellValueAndStyle(row.CreateCell(j), cellStyleDictionary[j], GetPropertyValue(ListData[i], PropertyArr[j]));
                }
            }
            return workbook;
        }

        /// <summary>
        /// 取得ExcelTemplate寫入新資料匯出
        /// 固定拷貝表頭一列，第二列拷貝樣式重複帶資料寫入
        /// </summary>
        /// <typeparam name="T">泛型使用</typeparam>
        /// <param name="ImportFile">範本完整路徑</param>
        /// <param name="ExportFile">輸出檔案完整路徑</param>
        /// <param name="CellNum">Cell欄位數量</param>
        /// <param name="ListData">要寫入的資料(泛型)</param>
        /// <param name="PropertyArr">輸入泛型資料屬性，需要依照Cell順序放入</param>
        public void ExportExcelByTemplate<T>(string ImportFile, string ExportFile, int CellNum, List<T> ListData, string[] PropertyArr)
        {
            using (FileStream templateFile = new FileStream(ImportFile, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(templateFile);
                HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(0);
                //輸出起始列，拷貝Style列
                int copyFormatRow = 1;
                //設定各cell的style
                Dictionary<int, ICellStyle> cellStyleDictionary = new Dictionary<int, ICellStyle>();
                for (int i = 0; i < CellNum; i++)
                {
                    cellStyleDictionary.Add(i, sheet.GetRow(copyFormatRow).GetCell(i).CellStyle);
                }
                //資料寫入 Row
                for (var i = 0; i < ListData.Count; i++)
                {
                    IRow row = sheet.CreateRow(copyFormatRow + i);
                    //處理Cell
                    for (var j = 0; j < CellNum; j++)
                    {
                        //填值
                        SetCellValueAndStyle(row.CreateCell(j), cellStyleDictionary[j], GetPropertyValue(ListData[i], PropertyArr[j]));
                    }
                }
                //輸出檔案
                using (FileStream file = new FileStream(ExportFile, FileMode.Create))//產生檔案
                {
                    workbook.Write(file);
                }
            }
        }

        /// <summary>
        /// 依照屬性名稱取得泛型參數內容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">泛型物件</param>
        /// <param name="key">屬性名稱</param>
        /// <returns></returns>
        public string GetPropertyValue<T>(T item, string key)
        {
            Type type = item.GetType();
            PropertyInfo proInfo = type.GetProperty(key);
            var val = proInfo.GetValue(item, null).ToString();
            return val;
        }

        /// <summary>
        /// 附檔名xls
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public HSSFWorkbook GetTemplateExcelBy2003(string FilePath)
        {
            HSSFWorkbook workbook;
            using (FileStream templateFile = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(templateFile);
            }
            return workbook;
        }

        /// <summary>
        /// 附檔名xlsx
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public XSSFWorkbook GetTemplateExcelBy2007(string FilePath)
        {
            XSSFWorkbook workbook;
            using (FileStream templateFile = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(templateFile);
            }
            return workbook;
        }

        /// <summary>
        /// 設定Cell Style跟值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellStyle"></param>
        /// <param name="value"></param>
        public void SetCellValueAndStyle(ICell cell, ICellStyle cellStyle, string value)
        {
            cell.SetCellValue(value);
            cell.CellStyle = cellStyle;
        }
    }
}