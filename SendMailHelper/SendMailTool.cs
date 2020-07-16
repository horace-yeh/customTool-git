using SendMailHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SendMailHelper
{
    public class SendMailTool
    {
        /// <summary>
        /// 寄信
        /// </summary>
        /// <param name="sendMailData">信件資料</param>
        /// <param name="smtpInfo">信件伺服器資料</param>
        public void SendMail(SendMailModel sendMailData, SmtpInfo smtpInfo)
        {
            var mail = new MailMessage();
            //寄件者 顯示名稱
            mail.From = new MailAddress(sendMailData.FromMail, sendMailData.FromDisPlayName, Encoding.UTF8);
            //標題
            mail.Subject = sendMailData.Subject;
            mail.SubjectEncoding = Encoding.UTF8;
            //設定信件內容
            mail.Body = sendMailData.HtmlBody;
            //郵件內容編碼
            mail.BodyEncoding = Encoding.UTF8;
            //是否使用html格式
            mail.IsBodyHtml = true;
            //收件者
            var toMailArr = this.SplitSCLM(sendMailData.ToMail);
            foreach (var item in toMailArr)
            {
                mail.To.Add(item);
            }
            //副本
            if (!string.IsNullOrWhiteSpace(sendMailData.CCMail))
            {
                var ccMailArr = this.SplitSCLM(sendMailData.CCMail);
                foreach (var item in ccMailArr)
                {
                    mail.CC.Add(item);
                }
            }
            //密件副本
            if (!string.IsNullOrWhiteSpace(sendMailData.BccMail))
            {
                var bccMailArr = this.SplitSCLM(sendMailData.BccMail);
                foreach (var item in bccMailArr)
                {
                    mail.Bcc.Add(item);
                }
            }
            //附件
            if (!string.IsNullOrWhiteSpace(sendMailData.AttachmentPath))
            {
                var AttachmentPathArr = this.SplitSCLM(sendMailData.AttachmentPath);
                foreach (var item in AttachmentPathArr)
                {
                    mail.Attachments.Add(new Attachment(item));
                }
            }
            //設定信件伺服器
            SmtpClient smtp = new SmtpClient(smtpInfo.SmtpHost, smtpInfo.Port);
            //設定使否要用SSL
            smtp.EnableSsl = smtpInfo.EnableSsl;
            if (!string.IsNullOrWhiteSpace(smtpInfo.LoginAccount) && !string.IsNullOrWhiteSpace(smtpInfo.LoginPassword))
            {
                smtp.Credentials = new System.Net.NetworkCredential(smtpInfo.LoginAccount, smtpInfo.LoginPassword);
            }

            try
            {
                smtp.Send(mail);
                smtp.Dispose(); //釋放資源
                mail.Dispose(); //釋放資源
            }
            catch (Exception ex)
            {
                //例外錯誤外拋
                throw;
                //ex.ToString();
            }
        }

        private string[] SplitSCLM(string temp)
        {
            char sign = ';';
            if (temp.IndexOf(sign) == -1)
            {
                string[] data = { temp };
                return data;
            }
            return this.SplitSign(temp, sign);
        }

        private string[] SplitSign(string temp, char sign)
        {
            return temp.Split(sign);
        }
    }
}