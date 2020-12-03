using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;

namespace AdminProject.Services
{
    public class UserService : IUserService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly ICommentService _commentService;
        private readonly IAddressService _addressService;
        private readonly IInvoiceService _invoiceService;
        private readonly IEmailService _emailService;
        private readonly ISettingService _settingService;

        public UserService(Func<AdminDbContext> dbFactory, ICommentService commentService, IAddressService addressService, IInvoiceService invoiceService, IEmailService emailService, ISettingService settingService)
        {
            _dbFactory = dbFactory;
            _commentService = commentService;
            _addressService = addressService;
            _invoiceService = invoiceService;
            _emailService = emailService;
            _settingService = settingService;
        }

        public void Add(User user)
        {
            var db = _dbFactory();

            if (EmailCheck(user.Email))
                throw new CustomException("E-Mail adresi daha önce kullanılmış. Farklı bir email ile deneyiniz.");

            db.Users.Add(user);
            db.SaveChanges();

            _emailService.SendActivationMail(user.Email, user.Name, user.Surname, user.ActivationCode);
        }

        public bool EmailCheck(string email)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email);
            return user != null;
        }

        public bool EmailActivation(string activationCode)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.ActivationCode == activationCode && a.Status == UserTypes.Deactive);
            if (user == null)
                return false;

            user.Status = UserTypes.Active;
            db.SaveChanges();

            return true;
        }

        public void SendForgotPassword(string email)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email);
            if (user == null)
                return;

            _emailService.SendForgotPasswordMail(user.Email, user.Name, user.Surname, user.Password);
        }

        public void Edit(int id, User userRequest)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            user.ActivationCode = userRequest.ActivationCode;
            user.Address = userRequest.Address;
            user.BannedMessage = userRequest.BannedMessage;
            user.City = userRequest.City;
            user.Country = userRequest.Country;
            user.Email = userRequest.Email;
            user.Gender = userRequest.Gender;
            user.Gsm = userRequest.Gsm;
            user.Name = userRequest.Name;
            user.Password = userRequest.Password;
            user.Phone = userRequest.Phone;
            user.Region = userRequest.Region;
            user.Status = userRequest.Status;
            user.Surname = userRequest.Surname;
            user.TcNr = userRequest.TcNr;
            user.UpdateDate = DateTime.Now;
            db.SaveChanges();
        }

        public UserTypes ChangeStatus(int id, UserTypes status)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            user.UpdateDate = DateTime.Now;
            user.Status = status;
            db.SaveChanges();

            return status;
        }

        public UserTypes UserBan(int id, string bannedMessage)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            user.Status = UserTypes.Banned;
            user.BannedMessage = bannedMessage;
            user.UpdateDate = DateTime.Now;
            db.SaveChanges();

            return UserTypes.Banned;
        }

        public void ChangePassword(int id, string password)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            user.Password = password;
            db.SaveChanges();
        }

        public void ResetPassword(string code, string email, string password)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.ActivationCode == code && a.Email == email);
            if (user == null)
                throw new CustomException("Kullanıcı bulunamadı.");

            user.Password = password;
            db.SaveChanges();
        }

        public User Login(string email, string password)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email && a.Password == password);
            
            if (user == null)
                throw new CustomException("Kullanıcı bulunamadı.");

            switch (user.Status)
            {
                case UserTypes.Banned:
                    throw new CustomException("Yasaklı kullanıcı. Yasaklanma nedeni: " + user.BannedMessage);
                case UserTypes.Deactive:
                    throw new CustomException("Kullanıcı doğrulaması yapılmamış. E-Mail adresinize gönderilen aktivasyon kodunu onaylayınız.");
                case UserTypes.Deleted:
                    throw new CustomException("İlgili kullanıcı silinmiştir.");
            }

            user.LastLoginDate = DateTime.Now;
            db.SaveChanges();

            return user;
        }

        public UserActivationTypes UserActive(string activationCode)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.ActivationCode == activationCode);
            if (user == null)
                return UserActivationTypes.ActivationCodeNotFound;

            user.Status = UserTypes.Active;
            user.UpdateDate = DateTime.Now;
            db.SaveChanges();

            return UserActivationTypes.ActivationSuccess;
        }

        public User GetUser(int id)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            return user;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public void Delete(string email)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public PagerList<User> AllUserList(int skip, int take)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var users = db.Users.OrderBy(a => a.Name).Skip(skip).Take(take).ToList();
            var userCount = db.Users.Count();

            var result = new PagerList<User>
            {
                TotalCount = userCount,
                List = users
            };

            return result;
        }

        public PagerList<User> AllUserList(UserSearchRequestDto request)
        {
            var db = _dbFactory();
            request.Skip = (request.Skip - 1) * request.Take;

            var users = db.Users.OrderBy(a => a.Name).Where(a => a.Status == request.Status);

            if (!string.IsNullOrEmpty(request.CityName))
                users = users.Where(a => a.City == request.CityName);

            if (!string.IsNullOrEmpty(request.CountryName))
                users = users.Where(a => a.Country == request.CountryName);

            if (!string.IsNullOrEmpty(request.Email))
                users = users.Where(a => a.Email == request.Email);

            if (!string.IsNullOrEmpty(request.Name))
                users = users.Where(a => a.Name == request.Name);

            if (!string.IsNullOrEmpty(request.RegionName))
                users = users.Where(a => a.Region == request.RegionName);

            if (!string.IsNullOrEmpty(request.Surname))
                users = users.Where(a => a.Surname == request.Surname);

            var userCount = users.Count();
            var userResult = users.Skip(request.Skip).Take(request.Take).ToList();

            var result = new PagerList<User>
            {
                TotalCount = userCount,
                List = userResult
            };

            return result;
        }

        public PagerList<User> ActiveUserList(int skip, int take)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var users = db.Users.Where(a => a.Status == UserTypes.Active).OrderBy(a => a.Name).Skip(skip).Take(take).ToList();
            var userCount = db.Users.Count();

            var result = new PagerList<User>
            {
                TotalCount = userCount,
                List = users
            };

            return result;
        }

        public List<Order> GetUserOrders(int userId)
        {
            var db = _dbFactory();
            var orders = db.Orders.Where(a => a.UserId == userId).OrderBy(a => a.Id).ToList();

            return orders;
        }

        public UserDetailView GetUserAllDetail(int userId)
        {
            var user = GetUser(userId);
            var address = _addressService.GetUserAddressAllList(userId);
            var orders = GetUserOrders(userId);
            var comments = _commentService.GetUserComment(userId);
            var invoices = _invoiceService.GetUserInoive(userId);

            var result = new UserDetailView
            {
                User = user,
                Orders = orders,
                Address = address,
                Comments = comments,
                Invoices = invoices
            };

            return result;
        }

        public UserDetailViewDto GetUserDetail(int userId)
        {
            var detail = GetUserAllDetail(userId);

            var result = new UserDetailViewDto
            {
                Address = detail.User.Address,
                City = detail.User.City,
                Country = detail.User.Country,
                Email = detail.User.Email,
                Gender = detail.User.Gender,
                Gsm = detail.User.Gsm,
                Id = detail.User.Id,
                LastLoginDate = detail.User.LastLoginDate,
                Name = detail.User.Name,
                Password = detail.User.Password,
                Phone = detail.User.Phone,
                Region = detail.User.Region,
                Status = detail.User.Status,
                Surname = detail.User.Surname,
                TcNr = detail.User.TcNr,
                AddressList = detail.Address.Select(a => new UserListItem
                {
                    DateTime = a.CreateDate,
                    Detail = a.City + "/ " + a.Region,
                    Subject = a.AddressSaveName,
                    Id = a.Id
                }).ToList(),
                CommentList = detail.Comments.Select(a => new UserListItem
                {
                    DateTime = a.Comment.DateTime,
                    Detail = a.Comment.Detail,
                    Subject = a.Name,
                    Id = a.Id
                }).ToList(),
                InvoiceList = detail.Invoices.Select(a => new UserListItem
                {
                    DateTime = DateTime.Now,
                    Detail = a.NameSurname,
                    Subject = a.InvoiceSaveName,
                    Id = a.Id
                }).ToList()
            };

            return result;
        }

        public void SendEftNotificationForm(string userMail, string name, string surname, string orderNr, string message)
        {
            var activeSetting = _settingService.GetActiveSetting();

            _emailService.SendEftNotificationForm(activeSetting.MailAddress, userMail, name, surname, orderNr, message);
        }
    }
}