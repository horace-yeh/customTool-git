using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper
{
    public class FileTool
    {
        /// <summary>
        /// 檢核資料夾不存在就建立
        /// </summary>
        /// <param name="Path"></param>
        public void CheckDirectory(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        /// <summary>
        /// 取得資料夾路徑
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 取得檔案名稱
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// 取得根目錄
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetPathRoot(string path)
        {
            return Path.GetPathRoot(path);
        }

        /// <summary>
        /// 移除資料路徑中的指定指串(string.Replace異常可使用)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReplacePathByKey(string path, string key)
        {
            StringBuilder builder = new StringBuilder(path);
            builder.Replace(key, "");
            var temp = builder.ToString();
            return temp;
        }
    }
}