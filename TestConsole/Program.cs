using SendMailHelper;
using SendMailHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        private static void mailTest()
        {
            var smTool = new SendMailTool();

            var data = new SendMailModel();

            //data.FromDisPlayName = "Test";
            //data.FromMail = "Test@Test.com";
            //data.HtmlBody = "123";
            //data.Subject = "12345";
            //data.ToMail = "horace.yeh@taiwantaxi.com.tw";
            //data.AttachmentPath = @"C:\Users\horace.yeh\Desktop\test\欠費不停機名單匯入格式.xls";

            var smtpinfo = new SmtpInfo();

            //smtpinfo.Port = 25;
            //smtpinfo.SmtpHost = "localhost";
            //smtpinfo.EnableSsl = false;

            smTool.SendMail(data, smtpinfo);
        }

        private static void Main(string[] args)
        {
            //mailTest();
        }
    }
}