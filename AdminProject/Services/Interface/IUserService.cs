using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface IUserService
    {
        void Add(User user);
        void Edit(int id, User userRequest);
        void SendForgotPassword(string email);
        bool EmailActivation(string activationCode);
        UserTypes ChangeStatus(int id, UserTypes status);
        UserTypes UserBan(int id, string bannedMessage);
        void ChangePassword(int id, string password);
        void ResetPassword(string code, string email, string password);
        User Login(string email, string password);
        UserActivationTypes UserActive(string activationCode);
        User GetUser(int id);
        void Delete(int id);
        void Delete(string email);
        PagerList<User> AllUserList(int skip, int take);
        PagerList<User> ActiveUserList(int skip, int take);
        PagerList<User> AllUserList(UserSearchRequestDto request);
        UserDetailView GetUserAllDetail(int userId);
        UserDetailViewDto GetUserDetail(int userId);
        bool EmailCheck(string email);
        void SendEftNotificationForm(string userMail, string name, string surname, string orderNr, string message);
    }
}