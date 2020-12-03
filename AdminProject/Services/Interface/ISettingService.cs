using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface ISettingService
    {
        void Add(Settings setting);
        Settings GetActiveSetting();
    }
}