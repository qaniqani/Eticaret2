namespace AdminProject.Models
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRememberMe { get; set; }
    }
}