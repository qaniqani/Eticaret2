using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class UserSearchRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string TcNr { get; set; }
        public string CountryUser { get; set; }
        public string CountryName { get; set; }
        public string CityUser { get; set; }
        public string CityName { get; set; } = "All";
        public string RegionUser { get; set; }
        public string RegionName { get; set; } = "All";
        public UserTypes Status { get; set; } = UserTypes.Active;
        public int Skip { get; set; } = 1;
        public int Take { get; set; } = 20;
    }
}