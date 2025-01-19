using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.Db;
using OnlineShop.Models.ViewModels;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private OnlineShopContext _context;

        public AccountController(OnlineShopContext context)
        {
           _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            user.RegisterDate = DateTime.Now;
            user.IsAdmin = false;
            user.Email = user.Email?.Trim();
            user.Password = user.Password?.Trim();
            user.FullName = user.FullName?.Trim();
            user.RecoveryCode = 0;
            //------------------------
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //------------Valid Email Checking------------
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(user.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(user);
            }
            //-----------Duplictae Email Checking-------------
            var prevUser = _context.Users.Any(x => x.Email == user.Email);
            if (prevUser)
            {
                ModelState.AddModelError("Email", "Email is used");
                return View(user);
            }
            //------------------------------------------------
            _context.Users.Add(user);
            _context.SaveChanges();
            //------------------------
            return RedirectToAction("login");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            //------------
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //------------
            var foundUser = _context.Users.FirstOrDefault(x => x.Email == user.Email.Trim() && x.Password == user.Password.Trim());
            //-----
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email or Password is not valid!");
                return View(user);
            }
            //------------
            // Create claims for the authenticated user
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, foundUser.FullName));
            claims.Add(new Claim(ClaimTypes.Email, foundUser.Email));
            //------------
            if (foundUser.IsAdmin == true)
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "user"));
            }
            //------------
            // Create an identity based on the claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //------------
            // Create a principal based on the identity
            var principal = new ClaimsPrincipal(identity);
            //------------
            // Sign in the user with the created principal
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //------------
            return Redirect("/");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult RecoveryPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecoveryPassword(RecoveryPasswordViewModel recoveryPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ////-------------------------------------------

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(recoveryPassword.Email);
            if (!match.Success)
            {
                ModelState.AddModelError("Email", "Email is not valid");
                return View(recoveryPassword);
            }

            ////-------------------------------------------

            var foundUser = _context.Users.FirstOrDefault(x => x.Email == recoveryPassword.Email.Trim());
            if (foundUser == null)
            {
                ModelState.AddModelError("Email", "Email is not exist");
                return View(recoveryPassword);
            }

            ////-------------------------------------------

            foundUser.RecoveryCode = new Random().Next(10000, 100000);
            _context.Users.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("utewartech@gmail.com");
            mail.To.Add(foundUser.Email);
            mail.Subject = "Recovery code";
            mail.Body = "Your recovery code:" + foundUser.RecoveryCode;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("wartech.ute@gmail.com", "pgye noqg tnnt yqzg");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            ////-------------------------------------------
            return Redirect("/Account/ResetPassword?email=" + foundUser.Email);
        }

        public IActionResult ResetPassword(string email)
        {
            var resetPasswordModel = new ResetPasswordViewModel();
            resetPasswordModel.Email = email;

            return View(resetPasswordModel);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPassword);
            }

            ////-------------------------------------------

            var foundUser = _context.Users.FirstOrDefault(x => x.Email == resetPassword.Email && x.RecoveryCode == resetPassword.RecoveryCode);
            if (foundUser == null)
            {
                ModelState.AddModelError("RecoveryCode", "Recovery code is not valid");
                return View(resetPassword);
            }

            ////-------------------------------------------

            foundUser.Password = resetPassword.NewPassword;

            _context.Users.Update(foundUser);
            _context.SaveChanges();

            ////-------------------------------------------

            return RedirectToAction("ResetPassword");
        }
    }
}
