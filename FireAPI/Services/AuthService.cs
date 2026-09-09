using FireAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace FireAPI.Services
{
    public interface IAuthService
    {
        string HashPassword(Usuario usuario, string password);
        bool VerifyPassword(Usuario usuario, string hashedPassword, string providedPassword);
    }

    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public string HashPassword(Usuario usuario, string password)
        {
            return _passwordHasher.HashPassword(usuario, password);
        }

        public bool VerifyPassword(Usuario usuario, string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(usuario, hashedPassword, providedPassword);
            return result != PasswordVerificationResult.Failed;
        }
    }
}