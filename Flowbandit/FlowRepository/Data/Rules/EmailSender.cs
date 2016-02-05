using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Data.Rules
{
    public class EmailSender : IDisposable
    {
        #region IDisposable

        protected bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _smtpClient.Dispose();
                }
                // Release unmanaged resources.
                disposed = true;
            }
        }

        ~EmailSender() { Dispose(false); }

        #endregion

        protected SmtpClient _smtpClient;

        public EmailSender(string host, string username, string password, int port = 587, bool useSsl = false)
        {
            _smtpClient = new SmtpClient(host, port);
            _smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
            _smtpClient.EnableSsl = useSsl;
            _smtpClient.Timeout = 10000;
        }

        public void SendEmail(string toAddresses, string subject, string body, string from, bool isHtml = false)
        {
            var mail = new MailMessage();

            mail.Body = body;
            mail.Subject = subject;
            mail.From = new MailAddress(from);

            string addresses = string.Join(",", toAddresses);
            mail.To.Add(addresses);

            mail.IsBodyHtml = isHtml;

            //TODO switch this to async
            _smtpClient.Send(mail);
        }
    }
}
