using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Agenda.Filters
{
    public class SessionValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //antes de ejecutar la acción del controlador o incluso antes de entrar al controlador, se ejecuta este código
            //comprobamos que haya una cookie de sesion activa y si no la hay, redirigimos al usuario a login
            var user = context.HttpContext.User;
            //verificar la autenticidad
            if (!user.Identity.IsAuthenticated)
            { //crear la cookie con el mensaje de sesion expirada

                context.HttpContext.Response.Cookies.Append("Mensaje","Tu sesió ha expirado, inicia sesión nuevamente", new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddSeconds(5),
                    HttpOnly = false,
                    Secure = true, //solo en https
                    SameSite = SameSiteMode.Strict //previene accesos desde otros sitios
                });

                context.Result = new RedirectToActionResult("Login","Usuario",null);
            }         
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
