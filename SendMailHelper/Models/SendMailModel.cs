using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMailHelper.Models
{
    /// <summary>
    /// 信件參數
    /// </summary>
    public class SendMailModel
    {
        /// <summary>
        /// 附件檔可以用(;)間隔多筆，需要使用完整路徑以及檔案名稱包含附檔名
        /// </summary>
        public string AttachmentPath { get; set; }

        /// <summary>
        /// 密件收信者Mail可以用(;)間隔多筆
        /// </summary>
        public string BccMail { get; set; }

        /// <summary>
        /// 副件收信者Mail可以用(;)間隔多筆
        /// </summary>
        public string CCMail { get; set; }

        /// <summary>
        /// 寄信顯示名稱
        /// </summary>
        public string FromDisPlayName { get; set; }

        /// <summary>
        /// 寄信Mail
        /// </summary>
        public string FromMail { get; set; }

        /// <summary>
        /// 信件主體
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// 信件主旨
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 收信者Mail可以用(;)間隔多筆
        /// </summary>
        public string ToMail { get; set; }
    }
}