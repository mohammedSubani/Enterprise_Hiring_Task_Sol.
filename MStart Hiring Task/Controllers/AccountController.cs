using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MStart_Hiring_Task.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Validate email and password for empty fields
            if (email == null || password == null || email == "" || password == "")
            {
                ViewBag.ErrorMessage = "*Empty fields";
                return View();
            }

            ClaimsIdentity identity = null;
            string role = null;

            // Using UsersOperations that makes call to UsersMgmt REST API 
            // using endpoints Authenticate and Authorize
            if (UsersOperations.AuthenticateUser(email,password))
                role = UsersOperations.GetAuthorization(email);
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }

            // Making a cookie based authentication Identity ticket
            identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            // Claiming the identity(Activating the ticket)
            var principal = new ClaimsPrincipal(identity);

            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(string userName,string password    ,string re_password, 
                                      string email   ,string phoneNumber ,string gender, 
                                      string birthDate)
        {
            //Validating for null or empty strings input cases 
            if (userName == "" || password == ""    || re_password == "" || 
                email == ""    || phoneNumber == "" || gender == ""      ||
                birthDate == "")
            {
                ViewBag.ErrorMessage2 = "Empty fields !";
                return View();
            }

            if (userName == null || password == null    || re_password == null ||
                email == null    || phoneNumber == null || gender == null ||
                birthDate == null)
            {
                ViewBag.ErrorMessage2 = "Empty fields !";
                return View();
            }

            //Validating email format
            if (!IsValidEmail(email))
            {
                ViewBag.ErrorMessage2 = "Bad email format !";
                return View();
            }

            // Checking for password 
            if (password != re_password)
            {
                ViewBag.ErrorMessage2 = "* Passwords didn't match !";
                return View();
            }

            // Using endpoint register in the UsersMgmt REST API if the call is sucessfull
            // create a cookie based authentication and claim the ticket
            if (UsersOperations.Register(userName,password,gender,email,phoneNumber,birthDate))
            {

                ClaimsIdentity identity = null;
                string role = "User";

                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.ErrorMessage2 = "* REGISTER IS UNSCUCESSFUL! , user name or email already exists !";

            return View();
        }


        // A method using built-in method to validate email format
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
