using BLL;
using SL.Authentication;
using SL.Authorization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Service.Controllers
{
    [RoutePrefix("login")]
    public class LoginController : ApiController
    {
        private readonly UserAccountLogic _logic;
        private readonly IEmailService _emailService;

        // Diccionario para almacenar intentos fallidos y el tiempo de bloqueo
        private static readonly Dictionary<string, (int failedAttempts, DateTime? lockoutTime)> loginAttempts = new Dictionary<string, (int, DateTime?)>();

        public LoginController()
        {
            _logic = new UserAccountLogic();
            _emailService = new EmailService(); // Inicialización de EmailService
        }

        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Authenticate([FromBody] LoginRequest loginRequest)
        {
            try
            {
                // Verificar si el usuario está bloqueado
                if (IsUserLocked(loginRequest.Email))
                {
                    return Content(HttpStatusCode.Unauthorized, "Tu cuenta está bloqueada temporalmente debido a intentos fallidos.");
                }

                // Buscar usuario por correo electrónico
                var user = _logic.GetUserByEmail(loginRequest.Email);
                if (user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Usuario no encontrado.");
                }

                // Verificar si la contraseña es correcta
                bool isPasswordValid = PasswordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
                if (!isPasswordValid)
                {
                    IncrementFailedAttempts(loginRequest.Email);
                    return Content(HttpStatusCode.Unauthorized, "Contraseña incorrecta.");
                }

                // Resetear intentos fallidos al iniciar sesión correctamente
                ResetFailedAttempts(loginRequest.Email);

                // Generar el token JWT
                var token = JwtService.GenerateToken(user.Email, user.Role);

                // Crear el cuerpo del correo con la fecha y hora
                string body = $@"
                <html>
                    <body>
                        <h2>Inicio de sesión exitoso</h2>
                        <p>Hola {user.Email},</p>
                        <p>Se ha iniciado sesión exitosamente en tu cuenta el {DateTime.Now:dddd, dd MMMM yyyy HH:mm:ss}.</p>
                        <p>Si no has sido tú, por favor, contacta con nuestro soporte inmediatamente.</p>
                        <br />
                        <p>Gracias,</p>
                        <p>El equipo de Soporte</p>
                    </body>
                </html>";

                // Enviar correo de notificación de inicio de sesión
                await _emailService.SendEmailAsync(user.Email, "Inicio de sesión exitoso", body);

                return Ok(new
                {
                    Token = token,
                    Email = user.Email,
                    Role = user.Role,
                    Message = "Autenticación exitosa."
                });
            }
            catch (FormatException ex)
            {
                // Capturar un error específico de formato, si el hash tiene un problema
                return Content(HttpStatusCode.BadRequest, new { Message = "Error en el formato de la contraseña." });
            }
            catch (System.UnauthorizedAccessException ex)
            {
                // Captura general de acceso no autorizado
                return Content(HttpStatusCode.Unauthorized, new { Message = ex.Message });
            }
            catch (System.Exception ex)
            {
                // Error general para otros casos
                return InternalServerError(ex);
            }
        }

        // Método para verificar si un usuario está bloqueado
        private bool IsUserLocked(string email)
        {
            if (loginAttempts.ContainsKey(email))
            {
                var (failedAttempts, lockoutTime) = loginAttempts[email];
                if (failedAttempts >= 3 && lockoutTime.HasValue && lockoutTime.Value > DateTime.Now)
                {
                    return true; // Usuario bloqueado
                }
            }
            return false;
        }

        // Método para incrementar los intentos fallidos
        private void IncrementFailedAttempts(string email)
        {
            if (loginAttempts.ContainsKey(email))
            {
                var (failedAttempts, lockoutTime) = loginAttempts[email];
                loginAttempts[email] = (failedAttempts + 1, failedAttempts >= 2 ? DateTime.Now.AddMinutes(5) : lockoutTime);
            }
            else
            {
                loginAttempts[email] = (1, null); // Primer intento fallido
            }
        }

        // Método para resetear los intentos fallidos cuando el login es exitoso
        private void ResetFailedAttempts(string email)
        {
            if (loginAttempts.ContainsKey(email))
            {
                loginAttempts[email] = (0, null); // Resetear intentos fallidos
            }
        }

        [HttpPost]
        [Route("verify-code")]
        [AllowAnonymous]
        public IHttpActionResult VerifyCode([FromBody] VerifyCodeRequest verifyRequest)
        {
            // Ejemplo de validación básica, puedes personalizarla según sea necesario
            if (string.IsNullOrEmpty(verifyRequest.Email) || string.IsNullOrEmpty(verifyRequest.Code))
            {
                return Content(HttpStatusCode.BadRequest, "Datos insuficientes para verificar el código.");
            }

            // Suponiendo que el código es válido, generar un token
            var token = JwtService.GenerateToken(verifyRequest.Email, "User");

            return Ok(new
            {
                Token = token,
                Email = verifyRequest.Email,
                Message = "Código verificado con éxito."
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class VerifyCodeRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
