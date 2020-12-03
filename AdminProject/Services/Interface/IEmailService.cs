namespace AdminProject.Services.Interface
{
    public interface IEmailService
    {
        void SendActivationMail(string userMail, string name, string surname, string code);
        void SendForgotPasswordMail(string userMail, string name, string surname, string password);
        void SendContactMail(string nameSurname, string companyName, string email, string phone, string message);
        void SendEftNotificationForm(string sendMail, string userMail, string name, string surname, string orderNr,
            string message);
    }
}