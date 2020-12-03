using System.Collections.Generic;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface ICommentService
    {
        void Add(Comment comment);
        void Edit(int userId, int id, Comment commentRequest);
        Comment GetComment(int userId, int id);
        List<Comment> GetUserActiveComment(int userId);
        List<ProductCommentList> GetUserComment(int userId);
        List<ProductCommentList> GetProductComment(int productId);
        void Delete(int userId, int id);
        void ChangeStatus(int userId, int id, CommentTypes status);
        List<CommentResultDto> GetCommentSearch(CommentSearchRequestDto request);
    }
}