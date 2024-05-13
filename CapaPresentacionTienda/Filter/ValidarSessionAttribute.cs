using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace CapaPresentacionTienda.Filter
{
    //Esto es para que nos redirija al acceso para logearnos si es que se nos cerro la session.
    public class ValidarSessionAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Cliente"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}