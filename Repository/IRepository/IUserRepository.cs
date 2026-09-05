using ApiEcommerce_VS.Models;
using ApiEcommerce_VS.Models.Dtos;

namespace ApiEcommerce_VS.Repository.IRepository

/*
=============
🏆 Ejercicio 
=============
*/
// 1. Crear una interfaz llamada IUserRepository.
//
// 2. Incluir los siguientes métodos en la interfaz:
//
//    - GetUsers
//        → Devuelve todos los usuarios en ICollection del tipo User.
//
//    - GetUser
//        → Recibe un id y devuelve un solo objeto User o null si no se encuentra.
//
//    - IsUniqueUser
//        → Recibe un nombre de usuario y devuelve un bool indicando si el nombre de usuario es único.
//
//    - Login
//        → Recibe un objeto UserLoginDto y devuelve un UserLoginResponseDto de forma asíncrona (Task).
//
//    - Register
//        → Recibe un objeto CreateUserDto y devuelve un objeto User de forma asíncrona (Task).
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User? GetUser(int id);
        bool IsUniqueUser(string username);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<UserDataDto> Register(CreateUserDto createUserDto);
    }
}
