//using ProyectoProxyService;
//using System;
//using System.Web.Mvc;
//using SL.Logger;
//using Proyecto.MVCPLS.Models;
//using SL.Authentication;  // Asegúrate de tener este espacio de nombres para usar EmailService
//using System.Collections.Generic;
//using System.Threading.Tasks; // Asegúrate de importar este espacio de nombres

//namespace Proyecto.MVCPLS.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly Proxy _proxy;
//        private readonly IEmailService _emailService;

//        // Diccionario para almacenar intentos fallidos y tiempo de bloqueo en memoria
//        private static readonly Dictionary<string, (int failedAttempts, DateTime? lockoutTime)> loginAttempts = new Dictionary<string, (int, DateTime?)>();

//        public AccountController()
//        {
//            _proxy = new Proxy();
//            _emailService = new EmailService();  // Inicialización de EmailService
//        }

//        // GET: Login
//        public ActionResult Login()
//        {
//            LogHelper.LogInformation("Se mostró el formulario de inicio de sesión.");
//            return View();
//        }

//        // POST: Login
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Login(LoginViewModel model)  // Convertido a async
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.ErrorMessage = "Por favor, complete los campos requeridos.";
//                return View(model);
//            }

//            try
//            {
//                // Verificar si el usuario está bloqueado
//                if (IsUserLocked(model.Email))
//                {
//                    ViewBag.ErrorMessage = "Tu cuenta está bloqueada temporalmente debido a intentos fallidos.";
//                    return View(model);
//                }

//                var response = _proxy.Authenticate(model.Email, model.Password);

//                if (response != null)
//                {
//                    LogHelper.LogInformation($"Login exitoso para el usuario: {model.Email}");
//                    Session["UserID"] = response.UserID;
//                    Session["Role"] = response.Role;
//                    Session["Email"] = response.Email;

//                    // Resetear intentos fallidos en memoria después de un login exitoso
//                    ResetFailedAttempts(model.Email);

//                    return RedirectToAction("Index", "Home");
//                }
//                else
//                {
//                    LogHelper.LogWarning($"Credenciales inválidas para el correo: {model.Email}");
//                    await IncrementFailedAttempts(model.Email); // Llamada asíncrona
//                    ViewBag.ErrorMessage = "Credenciales inválidas. Por favor, intente nuevamente.";
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error durante el inicio de sesión", ex);
//                ViewBag.ErrorMessage = "Error al procesar la solicitud. Por favor, intente nuevamente.";
//            }

//            return View(model);
//        }

//        // Método para verificar si un usuario está bloqueado
//        private bool IsUserLocked(string email)
//        {
//            if (loginAttempts.ContainsKey(email))
//            {
//                var loginAttempt = loginAttempts[email];
//                int failedAttempts = loginAttempt.failedAttempts;
//                DateTime? lockoutTime = loginAttempt.lockoutTime;

//                if (failedAttempts >= 3 && lockoutTime.HasValue && lockoutTime.Value > DateTime.Now)
//                {
//                    return true; // Usuario bloqueado
//                }
//            }
//            return false;
//        }

//        // Método para incrementar los intentos fallidos
//        private async Task IncrementFailedAttempts(string email)
//        {
//            if (loginAttempts.ContainsKey(email))
//            {
//                var loginAttempt = loginAttempts[email];
//                int failedAttempts = loginAttempt.failedAttempts;
//                DateTime? lockoutTime = loginAttempt.lockoutTime;

//                loginAttempts[email] = (failedAttempts + 1, failedAttempts >= 2 ? DateTime.Now.AddMinutes(5) : lockoutTime);

//                // Si se alcanzan 3 intentos fallidos, bloquear al usuario y enviar correo
//                if (loginAttempts[email].failedAttempts >= 3 && lockoutTime == null)
//                {
//                    await SendLockoutEmail(email); // Enviar correo de bloqueo
//                }
//            }
//            else
//            {
//                loginAttempts[email] = (1, null); // Primer intento fallido
//            }
//        }

//        // Método para resetear los intentos fallidos cuando el login es exitoso
//        private void ResetFailedAttempts(string email)
//        {
//            if (loginAttempts.ContainsKey(email))
//            {
//                loginAttempts[email] = (0, null); // Resetear intentos fallidos
//            }
//        }

//        // Método para enviar el correo de bloqueo de cuenta
//        private async Task SendLockoutEmail(string email)
//        {
//            var subject = "Tu cuenta está bloqueada temporalmente";
//            var body = $@"
//                <html>
//                    <body>
//                        <h2>Cuenta bloqueada</h2>
//                        <p>Hola,</p>
//                        <p>Hemos detectado múltiples intentos fallidos de inicio de sesión en tu cuenta. Como medida de seguridad, tu cuenta ha sido bloqueada temporalmente durante los próximos 5 minutos.</p>
//                        <p>Por favor, espera 5 minutos y vuelve a intentar iniciar sesión. Si no has intentado iniciar sesión, por favor, contacta con nuestro soporte.</p>
//                        <br />
//                        <p>Gracias por tu comprensión,</p>
//                        <p>El equipo de Soporte</p>
//                    </body>
//                </html>";

//            await _emailService.SendEmailAsync(email, subject, body);
//            LogHelper.LogInformation($"Correo de bloqueo enviado a: {email}");
//        }

//        // GET: Logout
//        public ActionResult Logout()
//        {
//            Session.Abandon();

//            if (Request.Cookies["AuthToken"] != null)
//            {
//                var authCookie = new System.Web.HttpCookie("AuthToken")
//                {
//                    Expires = DateTime.Now.AddDays(-1)
//                };
//                Response.Cookies.Add(authCookie);
//            }

//            if (Request.Cookies["UserRole"] != null)
//            {
//                var roleCookie = new System.Web.HttpCookie("UserRole")
//                {
//                    Expires = DateTime.Now.AddDays(-1)
//                };
//                Response.Cookies.Add(roleCookie);
//            }

//            LogHelper.LogInformation("El usuario cerró sesión.");
//            return RedirectToAction("Login", "Account");
//        }
//    }
//}

//VERSION FINAL

//using ProyectoProxyService;
//using System;
//using System.Web.Mvc;
//using SL.Logger;
//using Proyecto.MVCPLS.Models;
//using SL.Authentication;  // Asegúrate de tener este espacio de nombres para usar EmailService
//using System.Collections.Generic;
//using System.Threading.Tasks; // Asegúrate de importar este espacio de nombres

//namespace Proyecto.MVCPLS.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly Proxy _proxy;
//        private readonly IEmailService _emailService;

//        // Diccionario para almacenar intentos fallidos y tiempo de bloqueo en memoria
//        private static readonly Dictionary<string, (int failedAttempts, DateTime? lockoutTime)> loginAttempts = new Dictionary<string, (int, DateTime?)>();

//        public AccountController()
//        {
//            _proxy = new Proxy();
//            _emailService = new EmailService();  // Inicialización de EmailService
//        }

//        // GET: Login
//        public ActionResult Login()
//        {
//            if (TempData["SuccessMessage"] != null)
//            {
//                ViewBag.SuccessMessage = TempData["SuccessMessage"];
//            }

//            LogHelper.LogInformation("Se mostró el formulario de inicio de sesión.");
//            return View();
//        }


//        // POST: Login
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Login(LoginViewModel model)  // Convertido a async
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.ErrorMessage = "Por favor, complete los campos requeridos.";
//                return View(model);
//            }

//            try
//            {
//                // Verificar si el usuario está bloqueado
//                if (IsUserLocked(model.Email))
//                {
//                    ViewBag.ErrorMessage = "Tu cuenta está bloqueada temporalmente debido a intentos fallidos.";
//                    return View(model);
//                }

//                var response = _proxy.Authenticate(model.Email, model.Password);

//                if (response != null)
//                {
//                    LogHelper.LogInformation($"Login exitoso para el usuario: {model.Email}");
//                    Session["UserID"] = response.UserID;
//                    Session["Role"] = response.Role;
//                    Session["Email"] = response.Email;

//                    // Resetear intentos fallidos en memoria después de un login exitoso
//                    ResetFailedAttempts(model.Email);

//                    return RedirectToAction("Index", "Home");
//                }
//                else
//                {
//                    LogHelper.LogWarning($"Credenciales inválidas para el correo: {model.Email}");
//                    await IncrementFailedAttempts(model.Email); // Llamada asíncrona
//                    ViewBag.ErrorMessage = "Credenciales inválidas. Por favor, intente nuevamente.";
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error durante el inicio de sesión", ex);
//                ViewBag.ErrorMessage = "Error al procesar la solicitud. Por favor, intente nuevamente.";
//            }

//            return View(model);
//        }

//        // Método para verificar si un usuario está bloqueado
//        private bool IsUserLocked(string email)
//        {
//            if (loginAttempts.ContainsKey(email))
//            {
//                var loginAttempt = loginAttempts[email];
//                int failedAttempts = loginAttempt.failedAttempts;
//                DateTime? lockoutTime = loginAttempt.lockoutTime;

//                if (failedAttempts >= 3 && lockoutTime.HasValue && lockoutTime.Value > DateTime.Now)
//                {
//                    return true; // Usuario bloqueado
//                }
//            }
//            return false;
//        }

//        // Método para incrementar los intentos fallidos
//        private async Task IncrementFailedAttempts(string email)
//        {
//            if (loginAttempts.ContainsKey(email))
//            {
//                var loginAttempt = loginAttempts[email];
//                int failedAttempts = loginAttempt.failedAttempts;
//                DateTime? lockoutTime = loginAttempt.lockoutTime;

//                loginAttempts[email] = (failedAttempts + 1, failedAttempts >= 2 ? DateTime.Now.AddMinutes(5) : lockoutTime);

//                // Si se alcanzan 3 intentos fallidos, bloquear al usuario y enviar correo
//                if (loginAttempts[email].failedAttempts >= 3 && lockoutTime == null)
//                {
//                    await SendLockoutEmail(email); // Enviar correo de bloqueo
//                }
//            }
//            else
//            {
//                loginAttempts[email] = (1, null); // Primer intento fallido
//            }
//        }

//        // Método para resetear los intentos fallidos cuando el login es exitoso
//        private void ResetFailedAttempts(string email)
//        {
//            if (loginAttempts.ContainsKey(email))
//            {
//                loginAttempts[email] = (0, null); // Resetear intentos fallidos
//            }
//        }

//        // Método para enviar el correo de bloqueo de cuenta
//        private async Task SendLockoutEmail(string email)
//        {
//            var subject = "Tu cuenta está bloqueada temporalmente";
//            var body = $@"
//                <html>
//                    <body>
//                        <h2>Cuenta bloqueada</h2>
//                        <p>Hola,</p>
//                        <p>Hemos detectado múltiples intentos fallidos de inicio de sesión en tu cuenta. Como medida de seguridad, tu cuenta ha sido bloqueada temporalmente durante los próximos 5 minutos.</p>
//                        <p>Por favor, espera 5 minutos y vuelve a intentar iniciar sesión. Si no has intentado iniciar sesión, por favor, contacta con nuestro soporte.</p>
//                        <br />
//                        <p>Gracias por tu comprensión,</p>
//                        <p>El equipo de Soporte</p>
//                    </body>
//                </html>";

//            await _emailService.SendEmailAsync(email, subject, body);
//            LogHelper.LogInformation($"Correo de bloqueo enviado a: {email}");
//        }

//        // GET: Logout
//        public ActionResult Logout()
//        {
//            Session.Abandon();

//            if (Request.Cookies["AuthToken"] != null)
//            {
//                var authCookie = new System.Web.HttpCookie("AuthToken")
//                {
//                    Expires = DateTime.Now.AddDays(-1)
//                };
//                Response.Cookies.Add(authCookie);
//            }

//            if (Request.Cookies["UserRole"] != null)
//            {
//                var roleCookie = new System.Web.HttpCookie("UserRole")
//                {
//                    Expires = DateTime.Now.AddDays(-1)
//                };
//                Response.Cookies.Add(roleCookie);
//            }

//            LogHelper.LogInformation("El usuario cerró sesión.");
//            return RedirectToAction("Login", "Account");
//        }
//    }
//}

using ProyectoProxyService;
using System;
using System.Web.Mvc;
using SL.Logger;
using Proyecto.MVCPLS.Models;
using SL.Authentication;  // Espacio de nombres para EmailService
using SL.Authorization;  // Espacio de nombres para JwtService
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto.MVCPLS.Controllers
{
    public class AccountController : Controller
    {
        private readonly Proxy _proxy;
        private readonly IEmailService _emailService;

        // Diccionario para almacenar intentos fallidos y tiempo de bloqueo en memoria
        private static readonly Dictionary<string, (int failedAttempts, DateTime? lockoutTime)> loginAttempts = new Dictionary<string, (int, DateTime?)>();

        public AccountController()
        {
            _proxy = new Proxy();
            _emailService = new EmailService();  // Inicialización de EmailService
        }

        // GET: Login
        public ActionResult Login()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            LogHelper.LogInformation("Se mostró el formulario de inicio de sesión.");
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Por favor, complete los campos requeridos.";
                return View(model);
            }

            try
            {
                // Verificar si el usuario está bloqueado
                if (IsUserLocked(model.Email))
                {
                    ViewBag.ErrorMessage = "Tu cuenta está bloqueada temporalmente debido a intentos fallidos.";
                    return View(model);
                }

                var response = _proxy.Authenticate(model.Email, model.Password);

                if (response != null)
                {
                    // Generar token JWT
                    var token = JwtService.GenerateToken(response.Email, response.Role);

                    // Guardar token en sesión
                    Session["AuthToken"] = token;

                    LogHelper.LogInformation($"Login exitoso para el usuario: {model.Email}");

                    // Resetear intentos fallidos en memoria después de un login exitoso
                    ResetFailedAttempts(model.Email);

                    // Redirigir al Home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    LogHelper.LogWarning($"Credenciales inválidas para el correo: {model.Email}");
                    await IncrementFailedAttempts(model.Email); // Incrementar intentos fallidos
                    ViewBag.ErrorMessage = "Credenciales inválidas. Por favor, intente nuevamente.";
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error durante el inicio de sesión", ex);
                ViewBag.ErrorMessage = "Error al procesar la solicitud. Por favor, intente nuevamente.";
            }

            return View(model);
        }

        // Método para verificar si un usuario está bloqueado
        private bool IsUserLocked(string email)
        {
            if (loginAttempts.ContainsKey(email))
            {
                var loginAttempt = loginAttempts[email];
                int failedAttempts = loginAttempt.failedAttempts;
                DateTime? lockoutTime = loginAttempt.lockoutTime;

                if (failedAttempts >= 3 && lockoutTime.HasValue && lockoutTime.Value > DateTime.Now)
                {
                    return true; // Usuario bloqueado
                }
            }
            return false;
        }

        // Método para incrementar los intentos fallidos
        private async Task IncrementFailedAttempts(string email)
        {
            if (loginAttempts.ContainsKey(email))
            {
                var loginAttempt = loginAttempts[email];
                int failedAttempts = loginAttempt.failedAttempts;
                DateTime? lockoutTime = loginAttempt.lockoutTime;

                loginAttempts[email] = (failedAttempts + 1, failedAttempts >= 2 ? DateTime.Now.AddMinutes(5) : lockoutTime);

                // Si se alcanzan 3 intentos fallidos, bloquear al usuario y enviar correo
                if (loginAttempts[email].failedAttempts >= 3 && lockoutTime == null)
                {
                    await SendLockoutEmail(email); // Enviar correo de bloqueo
                }
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

        // Método para enviar el correo de bloqueo de cuenta
        private async Task SendLockoutEmail(string email)
        {
            var subject = "Tu cuenta está bloqueada temporalmente";
            var body = $@"
                <html>
                    <body>
                        <h2>Cuenta bloqueada</h2>
                        <p>Hola,</p>
                        <p>Hemos detectado múltiples intentos fallidos de inicio de sesión en tu cuenta. Como medida de seguridad, tu cuenta ha sido bloqueada temporalmente durante los próximos 5 minutos.</p>
                        <p>Por favor, espera 5 minutos y vuelve a intentar iniciar sesión. Si no has intentado iniciar sesión, por favor, contacta con nuestro soporte.</p>
                        <br />
                        <p>Gracias por tu comprensión,</p>
                        <p>El equipo de Soporte</p>
                    </body>
                </html>";

            await _emailService.SendEmailAsync(email, subject, body);
            LogHelper.LogInformation($"Correo de bloqueo enviado a: {email}");
        }

        // GET: Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Limpiar sesión
            LogHelper.LogInformation("El usuario cerró sesión.");
            return RedirectToAction("Login", "Account");
        }
    }
}
