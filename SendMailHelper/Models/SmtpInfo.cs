using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMailHelper.Models
{
    /// <summary>
    /// 信件伺服器資訊
    /// </summary>
    public class SmtpInfo
    {
        /// <summary>
        /// 設定使否用SSL
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// 信件登入帳號
        /// </summary>
        public string LoginAccount { get; set; }

        /// <summary>
        /// 信件登入密碼
        /// </summary>
        public string LoginPassword { get; set; }

        /// <summary>
        /// 埠號
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 寄信主機IP或Domain
        /// </summary>
        public string SmtpHost { get; set; }
    }
}