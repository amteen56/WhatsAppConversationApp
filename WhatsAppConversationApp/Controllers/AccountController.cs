using CRM.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatsAppConversationApp.Model;

namespace WhatsAppConversationApp.Controllers
{
    public class AccountController : Controller
    {
        string error = "";
        // GET: Account
        public ActionResult Index()
        {

            return View();
        }


    }
}