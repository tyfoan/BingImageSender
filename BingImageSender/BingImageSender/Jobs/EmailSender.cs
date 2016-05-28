using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private List<string> _suscribers = new List<string>();


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
            string email = (string)context.Scheduler.Context.Get("email");

            if (!_suscribers.Contains(email))
                _suscribers.Add(email);


            foreach (var subscriber in _suscribers)
            {
                using (MailMessage message = new MailMessage(_senderEmail, subscriber))
                {
                    string unsubscribeLink = "localhost:51229/Home/Unsubscribe";// HttpContext.Current.Request.Url.Port
                    //string unsubscribeLink = //.MapPath("~/View/Unsubscribe.cshtml");



                    message.Subject = "Daily Bing picture";
                    message.IsBodyHtml = true;
                    message.Body = "Hello, it's a Bing picture below for to day. "+
                        "<form method=\"post\" action=\"http://localhost:51229/Home/Unsubscribe\">" +
                            "<input type=\"hidden\" name =\"email\" value ="+ subscriber +">" +
                            "<input type=\"submit\" value=\"Submit\">" +
                        "</form>";



                    try
                    {
                        string imagePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/image.jpg");
                        Debug.WriteLine(imagePath);
                        message.Attachments.Add(new Attachment(imagePath));
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
}