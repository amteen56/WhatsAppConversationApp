using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsAppConversationApp.DataModel;
using WhatsAppConversationApp.Model;


namespace WhatsAppConversationApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            UserAuthenticationModel user = new UserAuthenticationModel();
            var id = user.Login(form["username"],form["pass"]);
            if (id == -1)
            {
                return View("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
} 