using CC_Lab_11.Models;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CC_Lab_11.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            Session["UserName"] = null;
            Session["IsValidTwoFactorAuthentication"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (loginModel.UserName == "hasan" && loginModel.Password == "123")
            {
                string key = ConfigurationManager.AppSettings["GoogleAuthKey"];
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                var setupInfo = tfa.GenerateSetupCode("MyApp", loginModel.UserName, key, false, 300);

                ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                ViewBag.SetupCode = setupInfo.ManualEntryKey;
                ViewBag.Status = true;

                Session["UserName"] = loginModel.UserName;

                // Stay on the Login page to show authentication part
                return View();
            }
            else
            {
                ViewBag.Message = "Invalid Credentials";
                ViewBag.Status = false;
            }

            return View();
        }


        [HttpPost]
        public ActionResult VerifyAuthentication(string code)
        {
            string key = ConfigurationManager.AppSettings["GoogleAuthKey"];
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN(key, code);

            if (isCorrectPIN)
            {
                Session["IsValidTwoFactorAuthentication"] = true;

                // Redirect to ScriptCheck controller
                return RedirectToAction("Index", "ScriptCheck");
            }

            ViewBag.Message = "Invalid Authentication Code";
            ViewBag.Status = false;

            return View("Login");
        }


        public ActionResult Welcome()
        {
            if (Session["IsValidTwoFactorAuthentication"] != null &&
                (bool)Session["IsValidTwoFactorAuthentication"] == true)
            {
                return View();
            }

            return RedirectToAction("Login");
        }
    }
}