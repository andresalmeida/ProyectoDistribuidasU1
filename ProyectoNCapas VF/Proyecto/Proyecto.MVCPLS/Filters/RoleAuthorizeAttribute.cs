using SL.Authorization;
using System;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Filters
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // Verificar si el token está presente en la cookie o sesión
            var token = HttpContext.Current.Session["AuthToken"] as string;

            if (string.IsNullOrEmpty(token) || !JwtService.ValidateToken(token))
            {
                // Si no hay token o no es válido, redirigir al Login
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}
