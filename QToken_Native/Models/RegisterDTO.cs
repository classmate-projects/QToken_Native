namespace QToken_Native.Models
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Speciality { get; set; }
        public string Role { get; set; } = "user";
    }
}
