using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using market.Models;
using market.Models.help;

namespace market.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// sign up and validation
        /// </summary>
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Index(User user)
        {
            ///////////////////////////////////////////// Validation
            if (ModelState.IsValid)
            {

                DataBase db = new DataBase();
                User lower = new User
                {
                    Name = user.Name.ToLower(),
                    Phone = user.Phone,
                    Password = user.Password,
                    Address = user.Address,
                    Email = user.Email.ToLower(),
                    Id = user.Id
                };
                if (db.Users.FirstOrDefault(item => item.Email == lower.Email) == null)
                {
                    db.Users.Add(lower);
                    db.SaveChanges();
                    var login = db.Users.First(i => i.Email == user.Email);
                    GlobalVariables.LoginUser = login;
                    var myuser = db.Users.First(item => item.Email == user.Email);
                    return RedirectToAction("Index", new RouteValueDictionary(
                        new { controller = "Home", action = "Index", id = myuser.Id }));
                }
                else
                {
                    ViewBag.errorInEmail = "this email is taken by another account";
                    return View();
                }
            }
            else
            {
                return View(user);
            }
        }




        /// <summary>
        /// Login by email and password
        /// </summary>
        
        public ActionResult RealLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RealLogin(string emailFromUser ,string passwordFromUser)
        {
            DataBase db = new DataBase();

            
            var email = emailFromUser.ToLower();
            var user = db.Users.FirstOrDefault(item => item.Email == email);


            if (user==null)
            {
                return View(model:"email is wrong !! ");
            }

            else
            {

                if (user.Password == passwordFromUser)
                {
                    ViewBag.customer = user;
                    if (user.Email == "adminadmin@admin.com")
                    {
                        GlobalVariables.LoginUser = user;
                        GlobalVariables.admin = true;
                        return RedirectToAction("Index", new RouteValueDictionary(
                            new { controller = "Admin", action = "Index", user.Id  }));
                    }
                    else
                    {
                        GlobalVariables.LoginUser = user;
                        GlobalVariables.admin = false;
                        return RedirectToAction("Index", new RouteValueDictionary(
                            new { controller = "Home", action = "Index", user.Id }));

                    }
                    
                }
                else
                {
                    return View(model: " email or password is wrong !! ");
                }
            }
            
        }


        public ActionResult LogOut()
        {
            GlobalVariables.LoginUser = null;
            GlobalVariables.MyBasket = null;
            GlobalVariables.admin = false;
            GlobalVariables.counter = 0;
            return RedirectToAction("Index", new RouteValueDictionary(
                new { controller = "Home", action = "Index" }));
        }

    }
}