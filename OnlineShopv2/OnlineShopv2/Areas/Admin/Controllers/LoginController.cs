using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopv2.Areas.Admin.Models;
using Model.Dao;
using OnlineShopv2.Common;

namespace OnlineShopv2.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;

                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "Your account is INVALID!!");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Your account is BLOCKED!!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Your password is WRONG!!");
                }
                else
                {
                    ModelState.AddModelError("", "Can not LOG IN !!");
                }
            }
            return View("Index");
            
            
        }
    }
}