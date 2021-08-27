using LibraryManagement5.Context;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryManagement5.Controllers
{
    public class LoginController : Controller
    {
        #region Database Initialization
        LibraryEntities15 dbobj2 = new LibraryEntities15();
        #endregion

        #region Action: Show Login Page
        // GET: Login
        public ActionResult Login()
        {
            ViewData["message"] = "";
            return View();
        }
        #endregion

        #region Action: Authenticate & Authorize Users
        [HttpPost]
        public ActionResult Login(User user, string returnUrl)
        {
            ViewData["message"] = "";
            if (IsValid(user))
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(user.username, false);
                if (returnUrl == null)
                    return Redirect("../Student/Books");
                else
                    return Redirect(returnUrl);
            }
            else
            {
                ViewData["message"] = "Email doesn't exist in the database!";
                return View(user);
            }
        }

        public bool IsValid(User user)
        {
            var res = dbobj2.Users.Where(y => y.username == user.username).SingleOrDefault();
            if (res != null)
                return true;
            else
                return false;
        }
        #endregion

        #region Action: Register Users
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                User obj = new User();

                obj.username = user.username;
                obj.creationTime = DateTime.Now;
                dbobj2.Users.Add(obj);
                dbobj2.SaveChanges();
                ModelState.Clear();

                System.Web.Security.FormsAuthentication.SetAuthCookie(user.username, false);
                return Redirect("../Student/Books");
            }
            return View(user);
        }
        #endregion

        #region Action: Logout 
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("../Student/Books");
        }
        #endregion
    }
}