using BLL;
using SL.Authentication;
using SL.Authorization;
using System;
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
                    return Content(HttpStatusCode.Unauthorized, "Contraseña incorrecta.");
                }

                // Generar el token JWT
                var token = JwtService.GenerateToken(user.Email, user.Role);

                // Enviar correo de notificación de inicio de sesión
                //await _emailService.SendEmailAsync(user.Email, "Inicio de sesión exitoso", "Se ha iniciado sesión en tu cuenta.");

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
