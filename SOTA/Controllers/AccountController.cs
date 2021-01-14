using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



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
            Users user = null;
            if (model.Name != null && model.Pass != null)
            {
                user = new Users();
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

                    user = await db.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == model.Name.ToLower() && u.Pass == hashed)
                        .ConfigureAwait(false);
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
                        user = new Users();
                        user = await db.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == model.Name.ToLower()).ConfigureAwait(false);
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
                        user.DateReg = DateTime.Now;
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


                // await Authenticate(user.Name).ConfigureAwait(false); // аутентификация

                await Authenticate(user).ConfigureAwait(false); // аутентификация
                                                                     //string login = HttpContext.User.Identity.Name;
                                                                     //Users user1 = db.Users.Where(p => p.Name == model.Name).First();
                                                                     //ViewBag.rl = user1.Role;

                ViewBag.rl = user.Role;
                return RedirectToAction("Index", "Home");

                //if (user.Role == 0)
                //{
                //    ViewBag.rl = user.Role;
                //    return RedirectToAction("NaznacRabotaList", "Uchen");
                //}
            }


            ModelState.AddModelError("", "Некорректные логин и(или) пароль");



            return View(model);
        }

        public async Task<IActionResult> CheckLog(string name)
        {
            Users user = new Users();
            try
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
          if (user != null)
            {
                if (user.Pass == null)
                {
                    if (user.Kod == "0")
                    {
                        return Json(0);
                    }
                    else
                    {
                        if (user.Role == 0)
                        {
                            return Json(4);
                        }
                        if (user.Role == 1)
                        {
                            return Json(5);
                        }
                        if (user.Role == 2)
                        {
                            return Json(6);
                        }
                        if (user.Role == 3)
                        {
                            return Json(7);
                        }
                    }
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

        private async Task Authenticate(Users user)
        {
            
            //CompositeModel compositeModel=new CompositeModel(db);
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            ViewData["Message"] = remoteIpAddress;
            /* if (remoteIpAddress == "193.242.149.177" || remoteIpAddress == "193.242.149.14" || remoteIpAddress == "::1")
             {     */
            // создаем один claim
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Name)
            
                //new Claim("name", userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                 ClaimsIdentity.DefaultRoleClaimType);
            
            // var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id)).ConfigureAwait(false);
            //  }

       




        }








        public async Task<IActionResult> Logout()
        {
            var email = HttpContext.User.Identity.Name;
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            return RedirectToAction("Index", "Home");
        }
    }
   
}
