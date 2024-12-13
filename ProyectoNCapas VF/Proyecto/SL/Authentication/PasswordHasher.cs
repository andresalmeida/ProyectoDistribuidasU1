using BCrypt.Net;

namespace SL.Authentication
{
    public static class PasswordHasher
    {
        // Método para generar un hash de contraseña
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        // Método para verificar si una contraseña coincide con un hash almacenado
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
