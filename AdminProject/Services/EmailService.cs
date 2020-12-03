using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly RuntimeSettings _setting;

        public EmailService(RuntimeSettings setting)
        {
            _setting = setting;
        }

        public void SendEftNotificationForm(string sendMail, string userMail, string name, string surname, string orderNr, string message)
        {
            const string mailTemplate = @"<table>
                                            <tr>
                                                <td>
                                                    Date Time
                                                </td>
                                                <td>
                                                    {0}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Name Surname
                                                </td>
                                                <td>
                                                    {1} {2}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Email
                                                </td>
                                                <td>
                                                    {3}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Order Nr
                                                </td>
                                                <td>
                                                    {4}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Message
                                                </td>
                                                <td>
                                                    {5}
                                                </td>
                                            </tr>
                                        </table>";

            var mailBody = string.Format(mailTemplate, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), name, surname,
                userMail, orderNr, message);

            SendUserMail(sendMail, "1 New EFT Notification Form", mailBody);
        }

        public void SendUserMail(string userMail, string subject, string body)
        {
            EmailSending(userMail, subject, body);
        }

        public void SendContactMail(string nameSurname, string companyName, string email, string phone, string message)
        {
            var mailTemplate = $@"
                                <table style='width:100%;'>
                                    <tr>
                                        <td style='font-weight:bold;'>İsim Soyisim</td>
                                        <td style='font-weight:bold;'>{nameSurname}</td>
                                    </tr>
                                    <tr>
                                        <td style='font-weight:bold;'>Şirket Adı</td>
                                        <td style='font-weight:bold;'>{companyName}</td>
                                    </tr>
                                    <tr>
                                        <td style='font-weight:bold;'>E-Mail</td>
                                        <td style='font-weight:bold;'>{email}</td>
                                    </tr>
                                    <tr>
                                        <td style='font-weight:bold;'>Telefon</td>
                                        <td style='font-weight:bold;'>{phone}</td>
                                    </tr>
                                    <tr>
                                        <td style='font-weight:bold;'>Mesaj</td>
                                        <td style='font-weight:bold;'>{message}</td>
                                    </tr>
                                </table>";

            EmailSending(_setting.ContactEmailAddress, "1 Yeni İletişim Formu", mailTemplate);
        }

        public void SendForgotPasswordMail(string userMail, string name, string surname, string password)
        {
            var mailTemplate = $"<html><body style=\"font-family: calibri; font-size: 15px; color: #000000;\">Merhaba {name} {surname} <br>Geçerli şifreniz: <strong>{password}</strong><br><br>İyi alış-verişler dileriz... </body></html>";

            EmailSending(userMail, "Şifre Hatırlatma", mailTemplate);
        }

        public void SendActivationMail(string userMail, string name, string surname, string code)
        {
            var mailTemplate = GetActivationEmailTemplate(name, surname, code);

            EmailSending(userMail, "Hesap Aktivasyonu/ Account Activation", mailTemplate);
        }

        private string GetActivationEmailTemplate(string name, string surname, string code)
        {
            const string body =
                "<html>" +
                "<body style=\"font-family: calibri; font-size: 15px; color: #000000;\">" +
                    "<div>Merhaba {0} {1},<br />Hesabınızı aktif etmek için <a href=\"{2}/user/activation/{3}\">tıklayınız</a>.</div>" +
                    "<br />" +
                    "<div>Hello {0} {1},<br /> To activate your account please <a href=\"{2}/user/activation/{3}\">click</a>.</div>" +
                "</body>" +
                "</html>";

            return
                string.Format(body, name, surname, _setting.Domain, code);
        }

        private void EmailSending(string sendMail, string subject, string mailBody)
        {
            var email = new MailMessage
            {
                From = new MailAddress(_setting.EmailAddress),
                Subject = subject,
                //IsBodyHtml = true,
                BodyEncoding = Encoding.GetEncoding("utf-8")
            };
            email.To.Add(sendMail);

            var plainBody = Regex.Replace(mailBody, @"<(.|\n)*?>", string.Empty);
            var plainView = AlternateView.CreateAlternateViewFromString(plainBody, null, "text/plain");
            email.AlternateViews.Add(plainView);

            var htmlView = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
            email.AlternateViews.Add(htmlView);

            var smtp = new SmtpClient
            {
                Credentials = new NetworkCredential(_setting.EmailAddress, _setting.EmailPassword),
                Port = _setting.Port,
                Host = _setting.Smtp,
                EnableSsl = false
            };
            smtp.Send(email);
        }
    }
}