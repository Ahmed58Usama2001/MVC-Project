using Demo.DAL.Models;
using Demo.PL.Setting;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public  class EmailSettings : IMailSetting
	{
        private MailSetting _options;

        public EmailSettings(IOptions<MailSetting>options)
        {
            _options = options.Value;
        }

        public  void SendEmail(Email email)
		{
            //Sender
            var mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject
            };


            //Receiver
            mail.To.Add(MailboxAddress.Parse(email.Recipients));

            //Body
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body=builder.ToMessageBody();

            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

            //open connection
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port,MailKit.Security.SecureSocketOptions.StartTls);

            smtp.Authenticate(_options.Email, _options.Password);

            smtp.Send(mail);

            smtp.Disconnect(true);
        }

      
    }
}
