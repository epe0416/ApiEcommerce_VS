using ApiEcommerce_VS.Data;
using ApiEcommerce_VS.Models;
using ApiEcommerce_VS.Models.Dtos;
using ApiEcommerce_VS.Repository.IRepository;

namespace ApiEcommerce_VS.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public User? GetUser(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<User> GetUsers()
        {
            return _db.Users.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string username)
        {
            return !_db.Users.Any(u => u.UserName.ToLower().Trim() == username.ToLower().Trim());
        }

        public Task<UserLoginDto> Login(UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<User> Register(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
