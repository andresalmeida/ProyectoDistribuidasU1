using Entities;

namespace SLC
{
    public interface ILogin
    {
        // Autentica a un usuario mediante correo y contraseña
        UserAccount Authenticate(string email, string password);
    }
}
