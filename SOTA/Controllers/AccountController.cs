using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SOTA.Models;

namespace SOTA.Controllers
{
    public class AccountController : Controller
    {
        private SotaContext db;

        public AccountController(SotaContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {


                string password = model.Pass;

                // generate a 128-bit salt using a secure PRNG
                /*string a = "Соль";

                byte[] salt = Encoding.Default.GetBytes(a);

                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));*/
                string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                Users user = new Users();
                try
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Name == model.Name && u.Pass == password).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                if (user != null)
                {


                    await Authenticate(model.Name).ConfigureAwait(false); // аутентификация
                    return RedirectToAction("Index", "Home");

                }


                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }


        private async Task Authenticate(string userName)
        {

            //CompositeModel compositeModel=new CompositeModel(db);
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            ViewData["Message"] = remoteIpAddress;
            /* if (remoteIpAddress == "193.242.149.177" || remoteIpAddress == "193.242.149.14" || remoteIpAddress == "::1")
             {



     */

            // создаем один claim
            var claims = new[]
 {
    new Claim("name", userName)
};

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            //  }





        }






        public async Task<IActionResult> Logout()
        {
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            return RedirectToAction("Index", "Home");
        }
    }
}