using Entities;
using System.Collections.Generic;

namespace SLC
{
    public interface IUserAccount
    {
        // Crea un nuevo usuario
        UserAccount CreateUserAccount(UserAccount newUserAccount);

        // Recupera un usuario por su ID
        UserAccount GetUserAccountByID(int userId);

        // Actualiza un usuario existente
        bool UpdateUserAccount(UserAccount userAccountToUpdate);

        // Elimina un usuario por su ID
        bool DeleteUserAccount(int userId);

        // Recupera todos los usuarios
        List<UserAccount> GetAllUserAccounts();

        // Recupera un usuario por su email
        UserAccount GetUserByEmail(string email);
    }
}