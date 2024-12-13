using DAL;
using Entities;
using Microsoft.IdentityModel.Logging;
using SL.Authentication;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL
{
    public class UserAccountLogic
    {
        public UserAccount CreateUserAccount(UserAccount newUserAccount)
        {
            UserAccount result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Verificar si el correo ya está registrado
                var existingUser = repository.Retrieve<UserAccount>(u => u.Email == newUserAccount.Email);
                if (existingUser != null)
                {
                    LogHelper.LogWarning($"Intento de registro con un correo ya registrado: {newUserAccount.Email}");
                    throw new InvalidOperationException("El correo electrónico ya está registrado.");
                }

                // Validar contraseña
                if (!ValidatePassword(newUserAccount.PasswordHash))
                {
                    LogHelper.LogWarning($"Contraseña inválida para el usuario: {newUserAccount.Email}");
                    throw new InvalidOperationException("La contraseña no cumple con los requisitos de seguridad.");
                }

                // Registrar el hash de la contraseña original (texto plano)
                LogHelper.LogInformation($"Contraseña original: {newUserAccount.PasswordHash}");

                // Hashear la contraseña
                newUserAccount.PasswordHash = PasswordHasher.HashPassword(newUserAccount.PasswordHash);
                LogHelper.LogInformation($"Contraseña hasheada: {newUserAccount.PasswordHash}");

                // Crear el usuario
                result = repository.Create(newUserAccount);
                LogHelper.LogInformation($"Usuario creado con éxito: {newUserAccount.Email}");
            }
            return result;
        }

        public UserAccount GetUserAccountByID(int userId)
        {
            UserAccount result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.Retrieve<UserAccount>(u => u.UserID == userId);
                if (result == null)
                {
                    throw new KeyNotFoundException("Usuario no encontrado.");
                }
            }
            return result;
        }

        public bool UpdateUserAccount(UserAccount userAccountToUpdate)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el correo no esté en uso por otro usuario
                var existingUser = repository.Retrieve<UserAccount>(u =>
                    u.Email == userAccountToUpdate.Email && u.UserID != userAccountToUpdate.UserID);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("El correo electrónico ya está en uso por otro usuario.");
                }

                result = repository.Update(userAccountToUpdate);
            }
            return result;
        }

        public bool DeleteUserAccount(int userId)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var user = GetUserAccountByID(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException("Usuario no encontrado.");
                }

                result = repository.Delete(user);
            }
            return result;
        }

        public List<UserAccount> GetAllUserAccounts()
        {
            List<UserAccount> result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.GetAll<UserAccount>();
            }
            return result;
        }

        public UserAccount Authenticate(string email, string password)
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var user = repository.Retrieve<UserAccount>(u => u.Email == email);
                if (user == null)
                {
                    LogHelper.LogWarning($"Usuario no encontrado: {email}");
                    return null;
                }

                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    throw new FormatException("El hash de la contraseña está vacío o es nulo.");
                }

                if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                {
                    LogHelper.LogWarning($"Contraseña incorrecta para el usuario: {email}");
                    return null;
                }

                return user;
            }
        }

        private bool ValidatePassword(string password)
        {
            // Validar si la contraseña es nula o vacía
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("La contraseña no puede estar vacía.");

            // Validar longitud mínima
            if (password.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");

            // Validar que contenga al menos una mayúscula, una minúscula y un carácter especial
            if (!Regex.IsMatch(password, "[A-Z]"))
                throw new ArgumentException("La contraseña debe contener al menos una letra mayúscula.");
            if (!Regex.IsMatch(password, "[a-z]"))
                throw new ArgumentException("La contraseña debe contener al menos una letra minúscula.");
            if (!Regex.IsMatch(password, "[!@#$%^&*(),.?{}|<>]"))
                throw new ArgumentException("La contraseña debe contener al menos un carácter especial.");

            return true;
        }

        public UserAccount GetUserByEmail(string email)
        {
            UserAccount result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.Retrieve<UserAccount>(u => u.Email == email);
                if (result == null)
                {
                    throw new KeyNotFoundException("Usuario no encontrado.");
                }
            }
            return result;
        }
    }
}
