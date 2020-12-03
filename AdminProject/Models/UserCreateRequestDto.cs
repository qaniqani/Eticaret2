namespace AdminProject.Models
{
    public class UserCreateRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TcNr { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
    }
}