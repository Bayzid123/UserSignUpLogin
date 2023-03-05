using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserSignUpLogin.Models;

namespace UserSignUpLogin.Controllers
{
    public class HomeController : Controller
    {
        ForMyProjectEntities db = new ForMyProjectEntities();
        public ActionResult Index()
        {
            return View(db.TblUsers.ToList());
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TblUser user)
        {
            if (db.TblUsers.Any(x => x.StrUserName == user.StrUserName))
            {

                ViewBag.Notification = "An account has already exist with this User Name! Try something different.";
                return View();
            }
            else
            {
                db.TblUsers.Add(user);
                db.SaveChanges();

                Session["IdUsSS"] = user.IntUserId.ToString();
                Session["UserName"] = user.StrUserName.ToString();

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(TblUser user)
        {
            var checkLogIn = db.TblUsers.Where(x=>x.StrUserName == user.StrUserName && x.StrPassword == user.StrPassword).FirstOrDefault();
            if (checkLogIn != null)
            {
                Session["IdUsSS"] = user.IntUserId.ToString();
                Session["UserName"] = user.StrUserName.ToString();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Wrong UserName or Password! Try Again.";
            }
            return View();
        }
    }
}