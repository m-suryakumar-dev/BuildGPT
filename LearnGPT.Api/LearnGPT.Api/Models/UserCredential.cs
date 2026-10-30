namespace LearnGPT.Api.Models
{
    public class UserCredential
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}
