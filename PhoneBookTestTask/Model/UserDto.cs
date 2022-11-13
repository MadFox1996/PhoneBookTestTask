using Data.EntityModel;

namespace PhoneBookTestTask.Model
{
    public static class UserDtoExtensions
    {
        public static User ToModel(this UserDto user, byte[] hash, byte[] salt)
        {
            return new User
            {
                Login = user.Login,
                Hash = hash,
                Salt = salt
            };
        }
    }

    public class UserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
