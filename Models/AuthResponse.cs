namespace G3MWL.Models
{
    public class AuthResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public AuthUser User { get; set; }
    }

    public class AuthUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
