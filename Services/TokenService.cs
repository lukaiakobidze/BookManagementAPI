using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public class UserService
    {
       
        public User ValidateUser(User user)
        {
            
            if (user.Username == "testuser" && user.Password == "password")
            {
                return new User { Username = "testuser", Role = "Admin" };
            }

            return null;
        }
    }
}
