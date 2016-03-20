using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using Model.Dao;
using OnlineShopv2.Models;

namespace OnlineShopv2.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var dao = new ProductDao();
            ViewBag.NewProducts = dao.ListNewProduct(4);
            ViewBag.ListFeatureProducts = dao.ListFeatureProduct(4);
            return View();
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            return PartialView(model);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupId(2);
            return PartialView(model);
        }
        public PartialViewResult HeaderCart()
        {
            var cart = Session[Common.CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            
            return PartialView(list);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }
    }
}