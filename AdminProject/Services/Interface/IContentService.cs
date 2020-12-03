using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IContentService
    {
        Content GetContent(string url);
    }
}