﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SoftPainter.Core.Base
{
    public class EmailUtils
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent, string filename, out string ErrorMessage)
        {
            ErrorMessage = "";

            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "smtp.163.com";          //SMTP服务器
            string mailFrom = "xxx";       //登陆用户名
            string userPassword = "xxx";   //登陆密码

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码
            smtpClient.Timeout = 3600000;

            // 发送邮件设置
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Normal;//优先级
            mailMessage.Attachments.Add(new Attachment(filename));

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件

                DateTime time = DateTime.Now;
                string dataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dataset");
                string destRootDirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "history", "backup");
                string destDirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "history", "backup",
                    string.Format("{0:D4}{1:D2}{2:D2}_{3:D2}_{4:D2}_{5:D2}_{6:D3}", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Millisecond));
                if (!System.IO.Directory.Exists(destRootDirPath))
                    System.IO.Directory.CreateDirectory(destRootDirPath);
                System.IO.Directory.Move(dataPath, destDirPath);

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "未知错误：" + ex.Message;
                return false;
            }
        }
    }
}
