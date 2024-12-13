using ProyectoProxyService;
using System;
using System.Web.Mvc;
using SL.Logger;
using Proyecto.MVCPLS.Models;

namespace Proyecto.MVCPLS.Controllers
{
    public class AccountController : Controller
    {
        private readonly Proxy _proxy;

        public AccountController()
        {
            _proxy = new Proxy();
        }

        // GET: Login
        public ActionResult Login()
        {
            LogHelper.LogInformation("Se mostró el formulario de inicio de sesión.");
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Por favor, complete los campos requeridos.";
                return View(model);
            }

            try
            {
                var response = _proxy.Authenticate(model.Email, model.Password);

                if (response != null)
                {
                    LogHelper.LogInformation($"Login exitoso para el usuario: {model.Email}");
                    Session["UserID"] = response.UserID;
                    Session["Role"] = response.Role;
                    Session["Email"] = response.Email;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    LogHelper.LogWarning($"Credenciales inválidas para el correo: {model.Email}");
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

        // GET: Logout
        public ActionResult Logout()
        {
            Session.Abandon();

            if (Request.Cookies["AuthToken"] != null)
            {
                var authCookie = new System.Web.HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(authCookie);
            }

            if (Request.Cookies["UserRole"] != null)
            {
                var roleCookie = new System.Web.HttpCookie("UserRole")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(roleCookie);
            }

            LogHelper.LogInformation("El usuario cerró sesión.");
            return RedirectToAction("Login", "Account");
        }
    }
}
