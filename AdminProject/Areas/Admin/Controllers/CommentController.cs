using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService, RuntimeSettings setting) : base(setting)
        {
            _commentService = commentService;
        }

        public ActionResult List(CommentSearchRequestDto request, string name, string surname)
        {
            SetPageHeader("Comment", "List");

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(surname))
            {
                request.UserName = name;
                request.UserSurname = surname;
            }

            ViewBag.CommentTypes = DropdownTypes.GetCommentType(request.Status);

            var result = _commentService.GetCommentSearch(request);
            ViewBag.Comments = result;

            return View(request);
        }

        public ActionResult ChangeStatus(int userId, int id, CommentTypes status)
        {
            ViewBag.CommentTypes = DropdownTypes.GetCommentType(status);

            _commentService.ChangeStatus(userId, id, status);

            Updated();

            return RedirectToAction("List");
        }
    }
}