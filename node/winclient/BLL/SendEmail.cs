using Sigesoft.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Sigesoft.Node.WinClient.BLL
{
    public class SendEmail
    {
        private string Subject { get; set; }
        private string To { get; set; }
        private string Cc { get; set; }
        private string Bcc { get; set; }
        private string Body { get; set; }
        private List<string> AttachmentPath { get; set; }

        SmtpClient objectSmtp = new SmtpClient();

        public SendEmail(string subject, string to, string cc, string bcc, string body, List<string> attachmentPath)
        {
            Subject = subject;
            To = to;
            Cc = cc;
            Bcc = bcc;
            Body = body;
            AttachmentPath = attachmentPath;
        }

        public void Send()
        {
            try
            {           
                string[] ArrayTo = To.Split(';');
                string[] ArrayCc = Cc.Split(';');
                string[] ArrayBcc = Bcc.Split(';');

                MailMessage objectEMail = new MailMessage();
                objectEMail.From = new MailAddress(Constants.EMAIL_FROM);

                foreach (string strTo in ArrayTo)
                    if (strTo != "") objectEMail.To.Add(strTo.Trim());

                foreach (string strCc in ArrayCc)
                    if (strCc != "") objectEMail.CC.Add(strCc.Trim());

                foreach (string strBcc in ArrayBcc)
                    if (strBcc != "") objectEMail.Bcc.Add(strBcc.Trim());

                objectEMail.Subject = Subject;
                objectEMail.IsBodyHtml = true;
                objectEMail.Body = Body;

                if (AttachmentPath != null)
                {
                    foreach (var item in AttachmentPath)
                        objectEMail.Attachments.Add(new Attachment(item));
                }

                ConfigEmail();
                objectSmtp.Send(objectEMail);
                objectEMail.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConfigEmail() 
        {
            OperationResult objOperationResult = new OperationResult();
            var configEmail = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");
                        
            string from = configEmail[2].Value1.ToLower();
            string fromPassword = configEmail[4].Value1;

            objectSmtp.Host = configEmail[0].Value1.ToLower();
            objectSmtp.Port = int.Parse(configEmail[1].Value1);
            objectSmtp.EnableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
            objectSmtp.Credentials = new System.Net.NetworkCredential(from, fromPassword);

        }

        private bool CheckEmailExists(string address)
        {
            string[] host = (address.Split('@'));
            string hostname = host[1];

            try
            {
                IPHostEntry iPHostEntry = Dns.Resolve(hostname);
                IPEndPoint endPoint = new IPEndPoint(iPHostEntry.AddressList[0], 25);
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool EmailFormat(string email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)                
                    return true;              
                
                return false;                
            }
            
            return false;
            
        }
    }
}
