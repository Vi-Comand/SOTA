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





            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            Users user = new Users();
            if (model.AddPass == null)
            {

                try
                {
                    string password = model.Pass;

                    //generate a 128 - bit salt using a secure PRNG
                    string a = "ПерестройкаИАЦ";

                    byte[] salt = Encoding.Default.GetBytes(a);

                    // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: password,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8));

                    user = await db.Users.FirstOrDefaultAsync(u => u.Name == model.Name && u.Pass == hashed).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }

            }
            else
            {
                try
                {
                    if (model.AddPass == model.AddPass2 && model.Sogl == true)
                    {
                        user = await db.Users.FirstOrDefaultAsync(u => u.Name == model.Name).ConfigureAwait(false);
                        string password = model.AddPass;

                        //generate a 128 - bit salt using a secure PRNG
                        string a = "ПерестройкаИАЦ";

                        byte[] salt = Encoding.Default.GetBytes(a);

                        // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));

                        user.Pass = hashed;
                        user.Sogl = 1;
                        await db.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }



            if (user != null)
            {


                await Authenticate(model.Name).ConfigureAwait(false); // аутентификация
                return RedirectToAction("Index", "Home");

            }


            //  ModelState.AddModelError("", "Некорректные логин и(или) пароль");


            return View(model);
        }

        public async Task<IActionResult> CheckLog(string name)
        {
            Users user = new Users();
            try
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Name == name).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            if (user != null)
            {
                if (user.Pass == "")
                {
                    return Json(0);
                }
                else
                {
                    return Json(1);
                }



            }
            return Json(9);
        }

        /*    [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> NewPass(LoginModel model)
            {
                Users user = new Users();
                try
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Name == model.Name).ConfigureAwait(false);
                    user.Pass = model.AddPass;
                    await db.Users.AddAsync(user).ConfigureAwait(false);
                    await db.SaveChangesAsync().ConfigureAwait(false);
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


                return View(model);
            }
            */

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