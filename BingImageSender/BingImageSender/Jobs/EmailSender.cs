using Quartz;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Xml;

namespace BingImageSender.Jobs
{
    public class EmailSender : IJob
    {
        private string _senderEmail = "ogurtsov.alexandr2016@yandex.ru";  //how to be in this situations?
        private string _password = "123456!@#";                           //not to show

        private string _host = "smtp.yandex.ru";
        private int _port = 25;

        
                  

        public static Uri GetImageUrl()
        {
            var FilePath = "http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US"; //daily picture xml
            string xmlStr = string.Empty;

            using (var wc = new WebClient())
            {
                xmlStr = wc.DownloadString(FilePath);
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            return new Uri("http://www.bing.com" + xmlDoc.GetElementsByTagName("url")[0].InnerText);
        }

        public static void DownloadImage()
        {
            string saveTo = HttpContext.Current.Server.MapPath("~/Files/image.jpg");
               //System.Web.Hosting.HostingEnvironment.MapPath("~/Files/image.jpg");
            Uri saveFrom = GetImageUrl();

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(saveFrom, saveTo);
            }
        }


       

        public void Execute(IJobExecutionContext context)
        {
            string to = (string)context.Scheduler.Context.Get("email");

            using (MailMessage message = new MailMessage(_senderEmail, to))
            {
                message.Subject = "Daily Bing picture";
                message.Body = string.Format(@"Hello, it's a Bing picture below for to day. Have a nice day :)");
                try
                {
                    string x = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/image.jpg");
                    Debug.WriteLine(x);
                    message.Attachments.Add(new Attachment(x));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = _host,
                    Port = _port,
                    Credentials = new NetworkCredential(_senderEmail, _password)
                })
                {
                    client.Send(message);
                }
            }
        }
    }
}