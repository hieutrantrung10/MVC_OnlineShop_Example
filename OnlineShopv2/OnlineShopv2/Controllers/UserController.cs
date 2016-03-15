using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.UI.Mvc;
using Common;
using Model.Dao;
using Model.EF;
using OnlineShopv2.Common;
using OnlineShopv2.Models;

namespace OnlineShopv2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã Captcha không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("","Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("","Email đã được sử dụng");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    var encryptedMd5Pas = Encryptor.MD5Hash(model.Password);
                    user.Password = encryptedMd5Pas;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = false;
                    string token = Guid.NewGuid().ToString();
                    user.CreatedBy = token;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        string callbackUrl =
                            System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                            "/User/Verify/" + token;
                        string mail =
                            System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/ConfirmEmail.html"));
                        mail = mail.Replace("{{Link}}", callbackUrl);

                        new MailHelper().SendMail(model.Email,"Xác nhận tài kooản",mail);
                        return RedirectToAction("Confirmation");
                    }
                    else
                    {
                        ModelState.AddModelError("","Đăng ký thất bại");
                    }
                }
            }
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult Confirmation()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Verify()
        {
            string verifystring = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string tmp = verifystring.Substring(verifystring.LastIndexOf("/"));
            string token = tmp.Split('/')[1];
            var userdao = new UserDao();
            var user = userdao.GetByToken(token);
            userdao.UpdateStatus(user);
            if (user.CreatedBy == token)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                return null;
            }


        }
    }
}