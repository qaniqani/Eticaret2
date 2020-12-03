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
    public class CommentService : ICommentService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public CommentService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Comment comment)
        {
            var db = _dbFactory();
            db.Comments.Add(comment);
            db.SaveChanges();
        }

        public void Edit(int userId, int id, Comment commentRequest)
        {
            var db = _dbFactory();
            var comment = db.Comments.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            comment.ProductId = commentRequest.ProductId;
            comment.DateTime = commentRequest.DateTime;
            comment.Detail = commentRequest.Detail;
            comment.Status = commentRequest.Status;
            comment.UserId = commentRequest.UserId;
            db.SaveChanges();
        }

        public List<CommentResultDto> GetCommentSearch(CommentSearchRequestDto request)
        {
            var db = _dbFactory();
            var comments = from comment in db.Comments
                           join product in db.Products
                           on comment.ProductId equals product.Id
                           join user in db.Users
                           on comment.UserId equals user.Id
                           where comment.Status == request.Status
                           select new CommentResultDto
                           {
                               CommentId = comment.Id,
                               CommentDetail = comment.Detail,
                               ProductId = product.Id,
                               ProductName = product.Name,
                               ProductUrl = product.Url,
                               Status = comment.Status,
                               UserEmail = user.Email,
                               UserId = user.Id,
                               UserName = user.Name,
                               UserSurname = user.Surname,
                               CreateDate = comment.DateTime
                           };

            if (!string.IsNullOrEmpty(request.ProductName))
                comments = comments.Where(a => a.ProductName.Contains(request.ProductName));

            if (!string.IsNullOrEmpty(request.UserName))
                comments = comments.Where(a => a.UserName.Contains(request.UserName));

            if (!string.IsNullOrEmpty(request.UserSurname))
                comments = comments.Where(a => a.UserSurname.Contains(request.UserSurname));

            if (!string.IsNullOrEmpty(request.Email))
                comments = comments.Where(a => a.UserEmail.Contains(request.Email));

            var result = comments.ToList();

            return result;
        }

        public Comment GetComment(int userId, int id)
        {
            var db = _dbFactory();
            var comment = db.Comments.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            if (comment == null)
                throw new CustomException("İlgili kullanıcıya ait yorum bulunamadı.");

            return comment;
        }

        public List<Comment> GetUserActiveComment(int userId)
        {
            var db = _dbFactory();
            var comments = db.Comments.Where(a => a.UserId == userId && a.Status == CommentTypes.Active).OrderByDescending(a => a.DateTime).ToList();
            return comments;
        }

        public List<ProductCommentList> GetUserComment(int userId)
        {
            var db = _dbFactory();
            var comments = (from comment in db.Comments
                            join product in db.Products
                            on comment.ProductId equals product.Id
                            where comment.Status != CommentTypes.Deleted
                            select new
                            {
                                product.Name,
                                comment.DateTime,
                                comment.Id,
                                comment.Detail,
                                comment.ProductId,
                                comment.Status,
                                comment.UserId
                            }).AsEnumerable().Select(a => new ProductCommentList
                            {
                                Name = a.Name,
                                Comment = new Comment
                                {
                                    DateTime = a.DateTime,
                                    Id = a.Id,
                                    Detail = a.Detail,
                                    ProductId = a.ProductId,
                                    Status = a.Status,
                                    UserId = a.UserId
                                }
                            }).ToList();
            return comments;
        }

        public List<ProductCommentList> GetProductComment(int productId)
        {
            var db = _dbFactory();
            var comments = (from comment in db.Comments
                join user in db.Users
                on comment.UserId equals user.Id
                where comment.ProductId == productId
                      && comment.Status == CommentTypes.Active
                orderby comment.DateTime
                select new
                {
                    Name = user.Name + " " + user.Surname,
                    comment.DateTime,
                    comment.Id,
                    comment.Detail,
                    comment.ProductId,
                    comment.Status,
                    comment.UserId
                }).ToList().Select(a => new ProductCommentList
            {
                Name = a.Name,
                Comment = new Comment
                {
                    DateTime = a.DateTime,
                    Id = a.Id,
                    Detail = a.Detail,
                    ProductId = a.ProductId,
                    Status = a.Status,
                    UserId = a.UserId
                }
            }).ToList();

            return comments;
        }

        public void Delete(int userId, int id)
        {
            var db = _dbFactory();
            var comment = db.Comments.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            if (comment == null)
                throw new CustomException("İlgili kullanıcıya ait yorum bulunamadı.");

            comment.Status = CommentTypes.Deleted;
            db.SaveChanges();
        }

        public void ChangeStatus(int userId, int id, CommentTypes status)
        {
            var db = _dbFactory();
            var comment = db.Comments.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            if (comment == null)
                throw new CustomException("İlgili kullanıcıya ait yorum bulunamadı.");

            comment.Status = status;
            db.SaveChanges();
        }
    }
}