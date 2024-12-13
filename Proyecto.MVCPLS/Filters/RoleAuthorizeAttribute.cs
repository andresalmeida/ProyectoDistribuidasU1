using System;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] AllowedRoles { get; set; }

        public RoleAuthorizeAttribute(params string[] roles)
        {
            AllowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Verificar si la sesión contiene el rol del usuario
            var userRole = httpContext.Session["UserRole"] as string;
            if (string.IsNullOrEmpty(userRole))
            {
                return false; // No autorizado si no existe el rol
            }

            // Verificar si el rol del usuario está permitido
            if (AllowedRoles != null && AllowedRoles.Length > 0)
            {
                return Array.Exists(AllowedRoles, role => role.Equals(userRole, StringComparison.OrdinalIgnoreCase));
            }

            return false; // No autorizado si el rol no coincide
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirigir al usuario no autorizado a la página de inicio de sesión
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
                    { "controller", "Account" },
                    { "action", "Login" }
                });
        }
    }
}
