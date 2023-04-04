namespace EmsApi.Dtos
{
    public class LoginDto
    {
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }
    }
}